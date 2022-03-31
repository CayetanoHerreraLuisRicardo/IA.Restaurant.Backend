using IA.Restaurant.Entities.Tables;
using IA.Restaurant.Utils.GenericRepository;

namespace IA.Restaurant.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly RestaurantContext _restaurantContext;
        /// <summary>
        /// DotnetCore auto-inject context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContextAccessor"></param>
        public UnitOfWork(RestaurantContext context)
        {
            _restaurantContext = context;
        }
        /// <inheritdoc/>
        public IGenericRepository<Products> ProductRepository => new GenericRepository<Products>(_restaurantContext);

        /// <inheritdoc/>
        public IGenericRepository<Orders> OrderRepository => new GenericRepository<Orders>(_restaurantContext);

        /// <inheritdoc/>
        public IGenericRepository<Status> StatusRepository => new GenericRepository<Status>(_restaurantContext);
        /// <inheritdoc/>
        public IGenericRepository<OrderDetails> OrderDetailRepository => new GenericRepository<OrderDetails>(_restaurantContext);
        /// <inheritdoc/>
        public void Save()
        {
            //_kioscoContext.SaveChanges();
        }

        private bool disposed = false;
        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _restaurantContext.Dispose();
                }
            }
            this.disposed = true;
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <inheritdoc/>
        public async Task SaveAsync()
        {
            await _restaurantContext.SaveChangesAsync();
        }
    }
}
