using AutoMapper;
using IntegrationService.IServices.IMapper;
using OfficeOpenXml.Filter;

namespace IntegrationService.ConcreteServices.Automapper
{
    public class Mapper<Tsource, Tdestination> : IServices.IMapper.IMapper<Tsource, Tdestination>
        where Tsource : class, new() where Tdestination : class, new()
    {
        public static MapperConfiguration Configure()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Tsource, Tdestination>();
            });
            return mapperConfiguration;
        }

    }


}