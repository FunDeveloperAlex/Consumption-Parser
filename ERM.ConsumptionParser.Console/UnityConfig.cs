using ERM.ConsumptionParser.Core;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace ERM.ConsumptionParser.Console
{
    /// <summary>
    /// Configure Unity container for IOC
    /// </summary>
    public static class UnityConfig
    {
        /// <summary>
        /// Register all our types for application
        /// </summary>
        /// <returns></returns>
        public static UnityContainer Configure()
        {         
            var container = new UnityContainer();
            
            //Logger
            container.RegisterType<ILogger, Infrastructure.ConsoleLogger>();

            //AbnormalDataProcessor
            container.RegisterType<AbnormalDataProcessor>(
                new InjectionConstructor(
                    new InjectionParameter(container.Resolve<ILogger>()),
                    new InjectionParameter(int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"], CultureInfo.InvariantCulture.NumberFormat)),
                    new InjectionParameter(float.Parse(ConfigurationManager.AppSettings["AbnormalDataProcess_variancePercent"], CultureInfo.InvariantCulture.NumberFormat)),
                    new InjectionParameter(int.Parse(ConfigurationManager.AppSettings["AbnormalDataProcess_minimum_viable_records"], CultureInfo.InvariantCulture.NumberFormat))
                    ));

            //FutureUnknownProcessor
            container.RegisterType<FutureUnknownProcessor>(
                new InjectionConstructor(
                    new InjectionParameter(container.Resolve<ILogger>()),
                    new InjectionParameter(int.Parse(ConfigurationManager.AppSettings["Processor_StreamReaderOptimalBuffer"], CultureInfo.InvariantCulture.NumberFormat))
                    ));


            //Coordinator
            container.RegisterType<ICoordinator, Core.Coordinator>(
                new InjectionConstructor(
                new InjectionParameter(
                    
                    //This is the only thing we need to do to add processors in the future.
                    new List<IProcessor>(){
                        container.Resolve<AbnormalDataProcessor>(),
                        container.Resolve<FutureUnknownProcessor>()
                    }
                    ),
                    new InjectionParameter(ConfigurationManager.AppSettings["ImportDirectory"]),
                    new InjectionParameter( container.Resolve<ILogger>())));           
      
            return container;
        }
    }
}
