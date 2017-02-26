using ERM.ConsumptionParser.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Test.Mocks
{
    public class MockLogger : ILogger
    {

        public MockLogger() { }

        /// <summary>
        /// StreamReader to read what was logged
        /// </summary>
        public string Content { get; set; }

        public void Log(string entry)
        {
            //save to our internal tracking property
            this.Content += entry;
        }
    }
}
