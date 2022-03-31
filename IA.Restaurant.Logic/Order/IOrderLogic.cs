using IA.Restaurant.Utils.Models;
using IA.Restaurant.Utils.QueryFilter;

namespace IA.Restaurant.Logic.Order
{
    public interface IOrderLogic
    {
        /// <summary>
        /// create order
        /// </summary>
        /// <param name="items">collection of products to add to the order</param>
        Task<OrderModel> Create(List<ProductModel> items);
        /// <summary>
        /// get orders by filter
        /// <param name="filter">filter to apply</param>
        /// </summary>
        /// <returns>collection of OrderModel</returns>
        Task<List<OrderModel>> Get(List<FilterExpression> filter);
        /// <summary>
        /// update status order by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="item">OrderStatusModel</param>
        Task<OrderStatusModel> UpdateStatus(int id, OrderStatusModel item);
        /// <summary>
        /// get products by filters
        /// </summary>
        /// <param name="filters">filter to aply</param>
        /// <returns></returns>
        IEnumerable<ProductModel> GetProducts(List<FilterExpression> filters);
    }
}
