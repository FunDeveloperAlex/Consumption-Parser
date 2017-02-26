using System;
using System.IO;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// encapsulates abstract behaviour for Processor
    /// </summary>
    public abstract class Processor : IProcessor
    {
               
        /// <summary>
        /// Logger to log results
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// Optimal buffer size to use with StreamReader class
        /// </summary>
        public int StreamReaderOptimalBuffer { get; set; }

        ///<summary>
        ///default constructor
        ///</summary>
        
        ///<param name="logger">ILogger for processor output.</param>
        ///<param name="variancePercent"></param>
        public Processor( ILogger logger, int streamReaderOptimalBuffer)
        {
            
            if (logger == null)
                throw new ArgumentNullException("logger", "Argument cannot be null");
            
            this.Logger = logger;
            this.StreamReaderOptimalBuffer = streamReaderOptimalBuffer;

    }

        /// <summary>
        /// Process consumption
        /// </summary>
        /// ///<param name="stream">System.IO.Stream to parse</param>
        /// <param name="origin">filename or description of origin</param>
        public abstract void Process(Stream stream, string origin);
    }
}
