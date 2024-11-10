using AutoMapper;
using TestProj.API.DTOs;
using TestProj.Core.Entities;

namespace TestProj.API.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
    }
}
