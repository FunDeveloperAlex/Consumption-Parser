using ERM.ConsumptionParser.Core.Enumerations;
using System;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Encapsulates a Consumption line items
    /// </summary>
    public class ConsumptionLineItem
    {
        /// <summary>
        /// MeterPoint Code
        /// </summary>
        public int MeterPoint { get; set; }

        /// <summary>
        /// Serial Number
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// Plant Code
        /// </summary>
        public string PlantCode { get; set; }


        /// <summary>
        /// DateTime
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// DataType
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// Data value / Energy reading
        /// </summary>
        public float DataValue { get; set; }

        /// <summary>
        /// Units
        /// </summary>
        public Unit Units { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public Status Status { get; set; }
    }
}
