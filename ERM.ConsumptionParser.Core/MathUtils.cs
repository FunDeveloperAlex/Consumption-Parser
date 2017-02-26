using System;
using System.Collections.Generic;
using System.Linq;

namespace ERM.ConsumptionParser.Core
{
    /// <summary>
    /// Generic math utils for consumption domain
    /// </summary>
    public class MathUtils
    {

        /// <summary>
        /// Calculate median of given list
        /// </summary>
        /// <param name="input">list to evaluate</param>
        /// <returns>median value</returns>
        public static float Median(List<float> input) {

            if (input == null)
                throw new ArgumentNullException("input", "Argument cannot be null");
            
            //order asc
            List<float> orderedInput = input.OrderBy(i => i).ToList<float>();

            int count = orderedInput.Count();
            int itemIndex = count / 2;
            float median = 0f;

            if (count % 2 == 0)
                median = (orderedInput[itemIndex] + orderedInput[itemIndex - 1]) / 2; //even number
            else
                median = orderedInput[itemIndex]; //odd number

            orderedInput.Clear();
            orderedInput = null;
            return median;
        }
    }
}
