using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERM.ConsumptionParser.Test.Mocks;
using ERM.ConsumptionParser.Core;
using System.Configuration;
using System.IO;
using System.Text;
using System.Globalization;

namespace ERM.ConsumptionParser.Test
{
    [TestClass]
    public class Test_IProcessor
    {
        #region AbnormalDataProcesssor

        public const string FILE_COLUMN_HEADERS = "MeterPoint Code, Serial Number,Plant Code, Date/ Time,Data Type, Data Value,Units,Status";
        public const string MOCK_FILE_NAME = "mock.txt";

        /// <summary>
        /// Default constructor
        /// </summary>
        public Test_IProcessor() {
            AutoMapperConfig.Configure();
        }

        /// <summary>
        /// Null parameter exceptions
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Argument cannot be null")]
        public void Test_AbnormalDataProcessor_Process_NullParameter()
        {
            MockLogger logger = new MockLogger();
            AbnormalDataProcessor processor = new AbnormalDataProcessor(
                logger,
                int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"]),
                0.2f,
                3);

            processor.Process(null, String.Empty);
            Assert.Fail();

        }

        /// <summary>
        /// Trigger minimum viable records condition. 
        /// //Expected: condition triggered and no content logged via logger. 
        /// </summary>
        [TestMethod]
        public void Test_AbnormalDataProcessor_Process_MinimumViableRecords()
        {
            const int MINIMUM_VIABLE_RECORDS = 3;

            MockLogger logger = new MockLogger();
            AbnormalDataProcessor processor = new AbnormalDataProcessor(
                logger,
                int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"]),
                0.2f,
                MINIMUM_VIABLE_RECORDS);

            StringBuilder input = new StringBuilder();
            input.AppendLine(FILE_COLUMN_HEADERS);
            input.AppendLine(@"210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,0.000000,kwh,");
           

            File.WriteAllText(MOCK_FILE_NAME, input.ToString());
            using (FileStream stream = new FileStream(MOCK_FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                processor.Process(stream, MOCK_FILE_NAME);
            }

            Assert.IsTrue(String.IsNullOrEmpty(logger.Content));
        }
        

        /// <summary>
        /// Trigger minimum viable records condition. 
        /// //Expected: no log content. 
        /// </summary>
        [TestMethod]
        public void Test_AbnormalDataProcessor_Process_MinimumViableRecords_NoAbnormalities()
        {
            const int MINIMUM_VIABLE_RECORDS = 3;

            MockLogger logger = new MockLogger();
            AbnormalDataProcessor processor = new AbnormalDataProcessor(
                logger,
                int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"]),
                0.2f,
                MINIMUM_VIABLE_RECORDS);

                StringBuilder input = new StringBuilder();
                input.AppendLine(FILE_COLUMN_HEADERS);
                input.AppendLine(@"210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,1.1,kwh,");
                input.AppendLine(@"210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,1.2,kwh,");
                input.AppendLine(@"210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,1.3,kwh,");
            
                File.WriteAllText(MOCK_FILE_NAME, input.ToString());
                using (FileStream stream = new FileStream(MOCK_FILE_NAME, FileMode.Open, FileAccess.Read))
                {
                    processor.Process(stream, MOCK_FILE_NAME);
                }
                Assert.IsTrue(String.IsNullOrEmpty(logger.Content));
            
        }

        /// <summary>
        /// Input contains odd number of items. 
        /// //Expected: 2 abnormalities found. 
        /// </summary>
        [TestMethod]
        public void Test_AbnormalDataProcessor_Process_OddNumber()
        {
            const int MINIMUM_VIABLE_RECORDS = 3;
            const float VARIANCE_PERCENT = 0.2f;
            StringBuilder expected = new StringBuilder();
            StringBuilder input = new StringBuilder();

            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 1 2"));
            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 3 2"));
            

            input.AppendLine(FILE_COLUMN_HEADERS);
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,1,kwh,");
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,2,kwh,");
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,3,kwh,");

            MockLogger logger = new MockLogger();
            AbnormalDataProcessor processor = new AbnormalDataProcessor(
                logger,
                int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"]),
                VARIANCE_PERCENT,
                MINIMUM_VIABLE_RECORDS);

            File.WriteAllText(MOCK_FILE_NAME, input.ToString());
            using (FileStream stream = new FileStream(MOCK_FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                processor.Process(stream, MOCK_FILE_NAME);
            }
            Assert.IsTrue(String.Equals(logger.Content, expected.ToString(), StringComparison.CurrentCulture));
        }

        /// <summary>
        /// Input contains even number of items. 
        /// //Expected: 2 abnormalities found. 
        /// </summary>
        [TestMethod]
        public void Test_AbnormalDataProcessor_Process_EvenNumber()
        {
            const int MINIMUM_VIABLE_RECORDS = 3;
            const float VARIANCE_PERCENT = 0.2f;
            StringBuilder expected = new StringBuilder();
            StringBuilder input = new StringBuilder();

            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 1 2.5"));
            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 2 2.5"));
            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 3 2.5"));
            expected.Append(string.Format("{0} {1}", MOCK_FILE_NAME, "31/08/2015 00:45:00 4 2.5"));


            input.AppendLine(FILE_COLUMN_HEADERS);
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,1,kwh,");
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,2,kwh,");
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,3,kwh,");
            input.AppendLine("210095893,210095893,ED031000001,31 / 08 / 2015 00:45:00,Import Wh Total,4,kwh,");

            MockLogger logger = new MockLogger();
            AbnormalDataProcessor processor = new AbnormalDataProcessor(
                logger,
                int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"]),
                VARIANCE_PERCENT,
                MINIMUM_VIABLE_RECORDS);

            File.WriteAllText(MOCK_FILE_NAME, input.ToString());
            using (FileStream stream = new FileStream(MOCK_FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                processor.Process(stream, MOCK_FILE_NAME);
            }
            Assert.IsTrue(String.Equals(logger.Content, expected.ToString(), StringComparison.CurrentCulture));
        }
        
        #endregion
    }
}
