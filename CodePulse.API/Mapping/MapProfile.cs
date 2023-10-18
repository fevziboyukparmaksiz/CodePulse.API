using AutoMapper;
using CodePulse.API.Models.Domain;
using CodePulse.API.Models.Dtos;

namespace CodePulse.API.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CreateCategoryRequestDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();

        }
    }
}
