using AutoMapper;
using OICAR.DTOs;
using OICAR.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Cart, CartDTO>().ReverseMap();
        CreateMap<CartItem, CartItemDTO>().ReverseMap();
        CreateMap<Item, ItemDTO>().ReverseMap();
        CreateMap<ItemCategory, ItemCategoryDTO>().ReverseMap();
        CreateMap<Order, OrderDTO>().ReverseMap();
        CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
        CreateMap<Status, StatusDTO>().ReverseMap();
        CreateMap<Tag, TagDTO>().ReverseMap();
    }
}