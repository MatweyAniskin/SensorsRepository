using AutoMapper;
using TestTaskApi.Models.Data;
using TestTaskApi.Models.Transfer;

namespace TestTaskApi.Models.Mapping
{
    public class AutoMapperProfile : Profile //профайл для библиотеки маппинга
    {
        public AutoMapperProfile() 
        {
            CreateMap<Sensor, SensorLocationMinDto>()
                .ForMember(dest => dest.Sensor_name, opt => opt.MapFrom(src => src.SensorName))
                .ForMember(dest => dest.Values, opt => opt.MapFrom(src => src.Values1));
        }
    }
}
