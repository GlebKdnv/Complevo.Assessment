using AutoMapper;
using Complevo.Assesment.Data.Entities;
using Complevo.Assesment.Services.Dto;

namespace Complevo.Assesment.Services.MapperProfiles
{
    internal class ProductAutoMapperProfile : Profile
    {
        public ProductAutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
