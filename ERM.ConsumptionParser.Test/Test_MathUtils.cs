using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ERM.ConsumptionParser.Core;

namespace ERM.ConsumptionParser.Test
{
    [TestClass]
    public class Test_MathUtils
    {
        /// <summary>
        /// Basic test with no edge cases
        /// </summary>
        [TestMethod]
        public void Test_MathUtils_Median()
        {
            List<float> input = new List<float>() { 0.150000f, 0.150000f, 0.146000f, 0.146000f, 0.004000f, 0.004000f, 0.146000f, 0.146000f };
            const float EXPECTED = 0.146f;
            float actual = MathUtils.Median(input);
            Assert.IsTrue(EXPECTED == actual );
        }

        /// <summary>
        /// Parameter validation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),"Expected ArgumentNullException")]
        public void Test_MathUtils_Median_ParameterException() {
            List<float> input = null;
            MathUtils.Median(input);
            Assert.Fail();
        }

        /// <summary>
        /// List containing only one value
        /// </summary>
        [TestMethod]
        public void Test_MathUtils_Median_MinimumListCountOf1() {
            List<float> input = new List<float>() {0.1f};
            const float EXPECTED = 0.1f;
            float actual = MathUtils.Median(input);
            Assert.IsTrue(actual == EXPECTED);
        }

        /// <summary>
        /// List containing negative numbers
        /// </summary>
        [TestMethod]
        public void Test_MathUtils_Median_NegativeNumbers() {
            List<float> input = new List<float>() { -1.5f, -1, 3 };
            const float EXPECTED = -1f;
            float actual = MathUtils.Median(input);
            Assert.IsTrue(actual == EXPECTED);
        }        

    }
}
