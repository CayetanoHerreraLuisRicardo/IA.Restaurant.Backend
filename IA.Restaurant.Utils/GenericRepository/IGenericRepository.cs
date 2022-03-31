using System.Linq.Expressions;

namespace IA.Restaurant.Utils.GenericRepository
{
    /// <summary>
    /// Interfaz de repositorio genérico. 
    /// Creado para un futuro que se necesite implementar pruebas unitarias.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// get element by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByID(object id);
        /// <summary>
        /// get data asynchronously
        /// </summary>
        /// <param name="filter">filters to apply</param>
        /// <param name="orderBy">order by</param>
        /// <param name="includeProperties">properties to include of the entity</param>
        /// <returns>IEnumerable<TEntity></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");
        /// <summary>
        /// get data as a IQueryable
        /// </summary>
        /// <param name="filter">filters to apply</param>
        /// <param name="orderBy">order by</param>
        /// <param name="includeProperties">properties to include of the entity</param>
        /// <returns>IEnumerable<TEntity></returns>
        IQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");
        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id entity</param>
        void Delete(object id);
        /// <summary>
        /// delete by entitiy
        /// </summary>
        /// <param name="entityToDelete">entity object to delete</param>
        void Delete(TEntity entityToDelete);
        /// <summary>
        /// bulk delete
        /// </summary>
        /// <param name="entities">collection of entities to delete</param>
        void BulkDelete(IEnumerable<TEntity> entities);
        /// <summary>
        /// insert a new record
        /// </summary>
        /// <param name="entity">new entity to insert</param>
        void Insert(TEntity entity);
        /// <summary>
        /// insert a new record asynchronously
        /// </summary>
        /// <param name="entity">new entity object to insert</param>
        Task InsertAsync(TEntity entity);
        /// <summary>
        /// bulk insert
        /// </summary>
        /// <param name="entities">collecion of entities to insert</param>
        Task BulkInsert(IEnumerable<TEntity> entities);
        /// <summary>
        /// update a entity
        /// </summary>
        /// <param name="entityToUpdate">entity object to update</param>
        void Update(TEntity entityToUpdate);
        /// <summary>
        /// update entities by expression
        /// </summary>
        /// <param name="entityToEntry">entity object to update</param>
        /// <param name="expressions">expression with the list of fields to affect </param>
        void SetModified(TEntity entityToEntry, IEnumerable<Expression<Func<TEntity, object>>> expressions);
        /// <summary>
        /// update a entity asynchronously
        /// </summary>
        /// <param name="entityToUpdate">entity object with the data to update</param>
        /// <param name="expression">expression to validate records to update</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entityToUpdate, Expression<Func<TEntity, bool>> expression);
        void BulkUpdate(IEnumerable<TEntity> entities);
    }
}
