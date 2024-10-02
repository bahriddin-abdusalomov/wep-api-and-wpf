using AutoMapper;

using Customer.Service.DTOs;
using Customer.Service.Models;

namespace Customer.Service.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductDTO, Product>();
        }
    }
}
