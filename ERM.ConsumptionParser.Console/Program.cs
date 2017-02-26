
using ERM.ConsumptionParser.Core;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERM.ConsumptionParser.Console
{
    /// <summary>
    /// Entry point class to console application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">args</param>
        static void Main(string[] args)
        {

            //Configure object to object mapping
            AutoMapperConfig.Configure();

            //Resolve starting point of application
            IUnityContainer container = UnityConfig.Configure();
            ICoordinator coordinator = container.Resolve<ICoordinator>();

            //Process power consumption and apply rules
            coordinator.ProcessConsumption();

            System.Console.Read();

        }
    }
}
