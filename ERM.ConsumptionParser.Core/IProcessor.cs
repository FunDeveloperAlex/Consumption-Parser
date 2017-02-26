using System.IO;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Consumption Processor
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Process consumption
        /// </summary>
        /// ///<param name="stream">System.IO.Stream to parse</param>
        /// <param name="origin">filename or description of origin</param>
         void Process(Stream stream, string origin);
    }
}
