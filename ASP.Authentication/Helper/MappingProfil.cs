using ASP.Authentication.DTOs;
using AutoMapper;
using Core.Entities;

namespace ASP.Authentication.Helper
{
    public class MappingProfil:Profile
    {
        public MappingProfil()
        {

            CreateMap<UserForRegisterationDto, User>()
           .ForMember(dest => dest.UserName,
               opt => opt.MapFrom(src => src.Email));

            CreateMap<Client, Clients_Management>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            CreateMap< OrderDto,Orders_Management>()
               // .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate));
            // .ForMember(dest => dest., opt => opt.MapFrom(src => src.ClientId));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()));

        }
    }
}
