using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Core.Enumerations
{
    /// <summary>
    /// Consumption Data Type
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// Import WH total
        /// </summary>
        ImportWhTotal,

        /// <summary>
        /// Import varh total
        /// </summary>
        ImportvarhTotal,

        /// <summary>
        /// Export varh total
        /// </summary>
        ExportvarhTotal,
        
        /// <summary>
        /// Export WH total
        /// </summary>
        ExportWhTotal,

        /// <summary>
        /// Phase Angle B
        /// </summary>
        PhaseAngleB,

        /// <summary>
        /// Phase Angle B
        /// </summary>
        PhaseAngleC,

        /// <summary>
        /// Phase Angle A
        /// </summary>
        PhaseAngleA,

        /// <summary>
        /// VoltagePhaseCMin
        /// </summary>
        VoltagePhaseCMin,
        /// <summary>
        /// VoltagePhaseCMax
        /// </summary>
        VoltagePhaseCMax,
        /// <summary>
        /// ReacivePowerTotal(vars)
        /// </summary>
        ReactivePowerTotalvars,

        /// <summary>
        /// ApparantPowerTotalVA
        /// </summary>
        ApparantPowerTotalVA,

        /// <summary>
        /// ActuveOiwerTitakWatts
        /// </summary>
        ActivePowerTotalWatts,

        /// <summary>
        /// CurrentTOtal
        /// </summary>
        CurrentTotal,

        /// <summary>
        /// VoltageTotal
        /// </summary>
        VoltageTotal,

        /// <summary>
        /// VoltagePhaseBMax
        /// </summary>
        VoltagePhaseBMax,

        /// <summary>
        /// CurrentPhaseBMin
        /// </summary>
        CurrentPhaseBMin,

        /// <summary>
        /// CurrentPhaseBMax
        /// </summary>
        CurrentPhaseBMax,
        /// <summary>
        /// VoltagePhaseAMax
        /// </summary>
        VoltagePhaseAMax,
        /// <summary>
        /// 
        /// </summary>
        VoltagePhaseAMin,
        /// <summary>
        /// VoltagePhaseAMin
        /// </summary>
        VoltagePhaseBMin,
        /// <summary>
        /// CurrentPhaseAMax
        /// </summary>
        CurrentPhaseAMax,
        /// <summary>
        /// CurrentPhaseAMin
        /// </summary>
        CurrentPhaseAMin,
        /// <summary>
        ///  CurrentPhaseCMin
        /// </summary>
        CurrentPhaseCMin,
        /// <summary>
        /// CurrentPhaseCMax
        /// </summary>
        CurrentPhaseCMax,
        /// <summary>
        ///  TOUCH1ExportWhTotal
        /// </summary>
        TOUCH1ExportWhTotal

            
    }
}
