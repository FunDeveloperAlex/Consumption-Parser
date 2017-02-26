

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// contract for generic logging
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// create a log entry
        /// </summary>
        /// <param name="entry">string value to log</param>
        void Log(string entry);
    }
}
