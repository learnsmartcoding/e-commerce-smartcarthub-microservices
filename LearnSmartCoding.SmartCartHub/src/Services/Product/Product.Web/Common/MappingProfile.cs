using AutoMapper;
using Products.Core.Entities;
using Products.Core.Models;

namespace Products.Web.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            CreateMap<ProductImage, ProductImageModel>();
            CreateMap<ProductImageModel, ProductImage>();
        }
    }
}
