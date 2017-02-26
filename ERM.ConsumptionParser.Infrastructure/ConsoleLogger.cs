using ERM.ConsumptionParser.Core;

namespace ERM.ConsumptionParser.Infrastructure
{
    /// <summary>
    /// Encapsulates logging logic specific to the console
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        /// <summary>
        /// Log entry to console
        /// </summary>
        /// <param name="entry">string value to log</param>
        public void Log(string entry)
        {
            System.Console.WriteLine(entry);
        }
    }
}
