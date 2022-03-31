using IA.Restaurant.Entities.Tables;
using IA.Restaurant.Utils.GenericRepository;

namespace IA.Restaurant.Data
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Order Repository
        /// </summary>
        IGenericRepository<Orders> OrderRepository { get; }
        /// <summary>
        /// Product Repository
        /// </summary>
        IGenericRepository<Products> ProductRepository { get; }
        /// <summary>
        /// Status Repository
        /// </summary>
        IGenericRepository<Status> StatusRepository { get; }
        /// <summary>
        /// Status Repository
        /// </summary>
        IGenericRepository<OrderDetails> OrderDetailRepository { get; }
        /// <summary>
        /// Metod defined in Dispose
        /// </summary>
        void Dispose();
        /// <summary>
        /// Save Changes
        /// </summary>
        void Save();
        /// <summary>
        /// Save Changes asynchronously
        /// </summary>
        Task SaveAsync();
    }
}
