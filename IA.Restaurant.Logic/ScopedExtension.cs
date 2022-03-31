using IA.Restaurant.Data;
using IA.Restaurant.Data.Mapping;
using IA.Restaurant.Logic.Order;
using IA.Restaurant.Logic.Product;
using IA.Restaurant.Utils.GenericCrud;
using IA.Restaurant.Utils.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IA.Restaurant.Logic
{
    /// <summary>
    /// class add scopes
    /// </summary>
    public static class ScopedExtension
    {
        /// <summary>
        /// Injects all business objects. Put here each element that is created.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddScopes(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IGenericCrudLogic<ProductModel>, ProductLogic>();
            services.AddScoped<IOrderLogic, OrderLogic>();

            services.AddAutoMapper(typeof(RestaurantProfile).Assembly);

            // agrega validadores
            //services = AddValidators(services);
            return services;
        }

    }
}
