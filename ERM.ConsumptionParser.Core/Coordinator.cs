
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Encapsulates orchestration of consumption reading activities / workflow.
    /// </summary>
    public class Coordinator : ICoordinator
    {
        private List<IProcessor> _processors;
        private string _importDirectory;
        private ILogger _logger;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="processors">list of processors to execute on consumption data.</param>
        /// <param name="importDir">fullpath to find files to process.</param>
        /// <param name="logger">logger for exceptions.</param>
        public Coordinator(List<IProcessor> processors, string importDir, ILogger logger)
        {

            if (processors == null)
                throw new ArgumentNullException("processors", "Argument cannot be null");
            if (String.IsNullOrEmpty(importDir))
                throw new ArgumentNullException("importDir", "Argument cannot be null");
            if (logger == null)
                throw new ArgumentNullException("logger", "Argument cannot be null");

            this._processors = processors;
            this._importDirectory = importDir;
            this._logger = logger;
        }

        /// <summary>
        /// Process power consumption
        /// </summary>
        public void ProcessConsumption()
        {
            //Parallel process each file.
            DateTime start = DateTime.Now;
            Parallel.ForEach(Directory.EnumerateFiles(_importDirectory, "*.csv", SearchOption.TopDirectoryOnly), (file) =>
            {
                //loop processors
                foreach (IProcessor proc in _processors)
                {
                    try
                    {
                        using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                        {
                            //Process
                            proc.Process(stream, Path.GetFileName(file));
                        }
                        GC.Collect();
                    }
                    catch (IOException e)
                    {
                        //log error
                        this._logger.Log(e.Message);
                    }
                }
            });
            Debug.Print(String.Format("{0}{1}", " Application Finished in (seconds) --> ", (DateTime.Now - start).TotalSeconds));
        }
    }
}

    




