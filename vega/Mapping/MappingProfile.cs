using AutoMapper;
using vega.Controllers.Resources;
using vega.Models;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();
        }
    }
}