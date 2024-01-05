using AutoMapper;
using Orders.Core.Entities;
using Orders.Core.Models;
using Orders.Web.ViewModels;

namespace Orders.Web.Common
{
    public class AutoMapperProductProfile : Profile
    {
        public AutoMapperProductProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            CreateMap<ProductImage, ProductImageModel>();
            CreateMap<ProductImageModel, ProductImage>();
        }
    }

    public class AutoMapperOrderProfile : Profile
    {
        public AutoMapperOrderProfile()
        {
            CreateMap<CreateOrder, OrderModel>()
                .ForMember(dest => dest.OrderItemsModel, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderModel>()
                .ForMember(dest => dest.OrderItemsModel, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.OrderStatusesModel, opt => opt.MapFrom(src => src.OrderStatuses));

            CreateMap<OrderModel, Order>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItemsModel))
              .ForMember(dest => dest.OrderStatuses, opt => opt.MapFrom(src => src.OrderStatusesModel));

            CreateMap<OrderItem, OrderItem>().ReverseMap();
            ;
        }
    }




}
