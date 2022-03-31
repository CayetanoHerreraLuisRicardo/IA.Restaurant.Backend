using IA.Restaurant.Utils.QueryFilter;

namespace IA.Restaurant.Utils.GenericCrud
{
    /// <summary>
    /// Generic interface with CRUD actions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericCrudLogic<T> where T : class
    {
        /// <summary>
        /// create item
        /// </summary>
        /// <param name="item">item to create</param>
        Task<T> Create(T item);
        /// <summary>
        /// get item by id
        /// </summary>
        Task<T> ReadById(int id);
        /// <summary>
        /// update item by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="item">item</param>
        Task<T> Update(int id, T item);
        /// <summary>
        /// delete item by id
        /// </summary>
        /// <param name="item">item</param>
        Task Delete(T item);
        /// <summary>
        /// get items by filter
        /// <param name="filter">filter to apply</param>
        /// </summary>
        /// <returns>T collection</returns>
        Task<IEnumerable<T>> Read(List<FilterExpression> filter);
    }
}
