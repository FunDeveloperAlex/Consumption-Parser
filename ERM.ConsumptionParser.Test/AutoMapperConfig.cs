using AutoMapper;
using ERM.ConsumptionParser.Core;
using ERM.ConsumptionParser.Core.Enumerations;
using System;

namespace ERM.ConsumptionParser.Test
{
    class AutoMapperConfig
    {
        /// <summary>
        /// Create object to object maps
        /// </summary>
        public static void Configure()
        {

            Mapper.Initialize(cfg =>
            {
                // ERM.ConsumptionParser.ConsumptionLineItem
                cfg.CreateMap<string[], ConsumptionLineItem>()
                .ForMember(p => p.MeterPoint, opts => opts.MapFrom(s => s[0]))
                .ForMember(p => p.SerialNumber, opts => opts.MapFrom(s => s[1]))
                .ForMember(p => p.PlantCode, opts => opts.MapFrom(s => s[2]))
                .ForMember(p => p.DateTime, opts => opts.MapFrom(s => s[3]))
                .ForMember(p => p.DataType, opts => opts.MapFrom(s => Enum.Parse(typeof(DataType), s[4].Replace(" ", "").Replace("(", "").Replace(")", ""))))
                .ForMember(p => p.DataValue, opts => opts.MapFrom(s => s[5]));
            });


        }
    }
}
