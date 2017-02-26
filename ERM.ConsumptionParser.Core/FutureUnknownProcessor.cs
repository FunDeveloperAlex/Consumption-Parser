using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Core
{

    /// <summary>
    /// processor that has not been invented yet as it is not the future
    /// </summary>
    public class FutureUnknownProcessor : Processor
    {

        ///<summary>
        ///default constructor
        ///</summary>
        ///<param name="stream">System.IO.Stream to parse</param>
        ///<param name="logger">ILogger for processor output.</param>
        ///<param name="streamReaderOptimalBuffer">Optimal buffer for StreamReader</param>
        ///<param name="variancePercent">variance percent specified as float e.g. 20% would be given as 0.2 </param>
        ///<param name="minimumViableRecords">minimum viable records to compute result</param>
        public FutureUnknownProcessor(ILogger logger, int streamReaderOptimalBuffer) : base(logger, streamReaderOptimalBuffer)
        {
            //TODO TODO TODO TODO TODO in future......
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Process consumption
        /// </summary>
        /// ///<param name="stream">System.IO.Stream to parse</param>
        /// <param name="origin">filename or description of origin</param>
        public override void Process(Stream stream, string origin)
        {

            //TODO TODO TODO TODO TODO in future......
            //throw new NotImplementedException();
        }
    }
}
