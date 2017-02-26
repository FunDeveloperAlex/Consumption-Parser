

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Encapsulates orchestration of consumption reading activities / workflow.s
    /// </summary>
    public interface ICoordinator
    {
        /// <summary>
        /// Process power consumption
        /// </summary>
        void ProcessConsumption();
    }
}
