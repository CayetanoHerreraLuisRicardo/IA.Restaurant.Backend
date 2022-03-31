using AutoMapper;
using IA.Restaurant.Entities.Tables;
using IA.Restaurant.Utils.Models;

namespace IA.Restaurant.Data.Mapping
{
    public class RestaurantProfile : Profile
    {
        /// <summary>
        /// Constructor. Aquí se mapean de Entities a Models y de Models a Entities
        /// </summary>
        public RestaurantProfile()
        {
            CreateMap<Orders, OrderModel>().ReverseMap();
            CreateMap<Products, ProductModel>().ReverseMap();
        }
    }
}
