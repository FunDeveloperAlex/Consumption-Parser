using AutoMapper;
using ERM.ConsumptionParser.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Processor to identify abnormal power readings in consumption
    /// </summary>
    public class AbnormalDataProcessor : Processor
    {
        const int DATA_VALUE_COLUMN_INDEX = 5;

        /// <summary>
        /// variance percent specified as float e.g. 20% would be given as 0.2
        /// </summary>
        private float VariancePercent { get; set; }

        /// <summary>
        /// minimum viable records to compute result
        /// </summary>
        private int MinimumViableRecords { get; set; }
        
        ///<summary>
        ///default constructor
        ///</summary>
        ///<param name="logger">ILogger for processor output.</param>
        ///<param name="streamReaderOptimalBuffer">Optimal buffer for StreamReader</param>
        ///<param name="variancePercent">variance percent specified as float e.g. 20% would be given as 0.2 </param>
        ///<param name="minimumViableRecords">minimum viable records to compute result</param>
        public AbnormalDataProcessor(ILogger logger, int streamReaderOptimalBuffer, float variancePercent, int minimumViableRecords) : base(logger, streamReaderOptimalBuffer)
        {

            //variancePercent must be valid percent 0 to 1
            if (variancePercent > 0.99 || variancePercent < 0.01)
                throw new ArgumentException("Invalid variance percent specified for abnormal data processor", "variancePercent");
            
            this.VariancePercent = variancePercent;
            this.MinimumViableRecords = minimumViableRecords;
            
        }

        /// <summary>
        /// Process consumption
        /// </summary>
        /// ///<param name="stream">System.IO.Stream to parse</param>
        /// <param name="origin">filename or description of origin</param>
        public override void Process(Stream stream, string origin)
        {
            if (String.IsNullOrEmpty(origin))
                throw new ArgumentNullException("origin", "Argument cannot be null");
            if (stream == null)
                throw new ArgumentNullException("stream", "Argument cannot be null");

            //calculate the median
            bool meetsMinimumRequirements = true;
            float median = CalculateMedian(stream, out meetsMinimumRequirements);

            //exit if required
            if (!meetsMinimumRequirements)
                return;

            //reset stream to begin again
            stream.Position = 0;

            //Identify 'Abnormal' lines
            List<ConsumptionLineItem> abnormalLines = GetAbnormalLines(stream, median);


            //Format and log results
            foreach (ConsumptionLineItem line in abnormalLines)
            {
                this.Logger.Log(Format(line, origin, median));
            }

            //cleanup
            if (abnormalLines != null) { abnormalLines.Clear(); abnormalLines = null; }
           

        }

        /// <summary>
        /// Sserialize ConsumptionLineItem to formatted string
        /// </summary>
        /// <param name="items">ConsumptionLineItem to serialize</param>
        /// <param name="filename">filename for consumption report</param>
        /// </param name="median">median for report</param>
        private string Format(ConsumptionLineItem item, string filename, float median)
        {
            return String.Format("{0} {1} {2} {3}", filename, item.DateTime.ToString("dd/MM/yyyy HH:mm:ss"), item.DataValue, median);
        }

        /// <summary>
        /// calculate median from stream
        /// </summary>
        /// <param name="stream">stream to process</param>
        /// <param name="meetsMinimumRequirements">meets minimum requirements for mean calculation</param>
        ///<remarks>
        /// [Assumption / Business Rule] - report containing less than minimum_viable_records to be ignored
        /// [Assumption / Business RUle] - report containing only zero values to be ignoreds
        ///</remarks>
        /// <returns>
        ///    Success: median value and meetsMinimumRequirements = true
        ///    Failure: zero and meetsMinimumRequirementts = falsse
        ///</returns>
        private float CalculateMedian(Stream stream, out bool meetsMinimumRequirements)
        {
            
            List<float> values = new List<float>();
            List<ConsumptionLineItem> lines = new List<ConsumptionLineItem>();
            StreamReader reader = null;
            try
            {
                //Create reader that will NOT close stream. Garbage collector will dispose reader
                reader = new StreamReader(  stream,
                                            Encoding.GetEncoding((int)ConsumptionEncoding.Default),         //encoding
                                            true,                                                           //detect encoding from byte order marks
                                            this.StreamReaderOptimalBuffer,                                 //buffer size when reading stream,
                                            true                                                            //Leave underlying stream open we need to reuse it!!! 
                                            );

                //read header lines
                reader.ReadLine();

                //read line items
                while (!reader.EndOfStream)
                    values.Add(float.Parse(reader.ReadLine().Split(',')[DATA_VALUE_COLUMN_INDEX], CultureInfo.InvariantCulture.NumberFormat));  

            }
            //minimum exception handling
            catch (IOException e1) {
                this.Logger.Log(e1.Message);
            }
            catch (OutOfMemoryException e2)
            {
                this.Logger.Log(e2.Message);
            }
            finally {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //business rule [ minimum records ]
            if (values.Count < this.MinimumViableRecords)
            {
                meetsMinimumRequirements = false;
                return 0;
            }

            //business rule [ no valid readings ]
            if (values.Distinct().Count() == 1 && values[0] == 0)
            {
                meetsMinimumRequirements = false;
                return 0;
            }

            float median = MathUtils.Median(values);

            values.Clear();
            values = null;
            meetsMinimumRequirements = true;
            return median;

        }

        /// <summary>
        /// Identify line items readings <> 20% variance of the median reading for given data set
        /// </summary>
        /// <param name="stream">input stream of which to examine</param>
        /// <param name="median">median by which to evaluate definition of 'Abnormal'</param>
        /// <returns>list of <see cref="ERM.ConsumptionParser.Core.ConsumptionLineItem"/></see></returns>
        private List<ConsumptionLineItem> GetAbnormalLines(Stream stream, float median)
        {

            List<ConsumptionLineItem> results = new List<ConsumptionLineItem>();
            StreamReader reader = null;

            //Calculate abnormal boundary
            float minBoundary = median - (median * this.VariancePercent);
            float maxBoundary = median + (median * this.VariancePercent);

            try
            {
                //instantiate reader that will not close stream.
               reader = new StreamReader(   stream,
                                            Encoding.GetEncoding((int)ConsumptionEncoding.Default),         //encoding
                                            true,                                                           //detect encoding from byte order marks
                                            this.StreamReaderOptimalBuffer,                                 //buffer size when reading stream,
                                            true                                                            //Leave underlying stream open we need to reuse it!!! 
                                            );

                //read header line
               reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    //deserialize line item
                    string[] rawLine = reader.ReadLine().Split(',');
                    if ((float.Parse(rawLine[DATA_VALUE_COLUMN_INDEX], CultureInfo.InvariantCulture.NumberFormat) <= minBoundary) ||
                        (float.Parse(rawLine[DATA_VALUE_COLUMN_INDEX], CultureInfo.InvariantCulture.NumberFormat) >= maxBoundary))
                        results.Add(Mapper.Map<ConsumptionLineItem>(rawLine));
                    }

            }
            //minimum exception handling
            catch (IOException e1)
            {
                this.Logger.Log(e1.Message);
            }
            catch (OutOfMemoryException e2)
            {
                this.Logger.Log(e2.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return results;



        }


    }
}




