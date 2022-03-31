using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace IA.Restaurant.Utils.GenericRepository
{
    /// <summary>
    /// Implementación de interfaz de repositorio genérico.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public GenericRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        /// <inheritdoc/>
        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }
        /// <inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        /// <inheritdoc/>
        public virtual IQueryable<TEntity> GetIQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }
            else
            {
                return query;
            }
        }
        /// <inheritdoc/>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }
        /// <inheritdoc/>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }
        /// <inheritdoc/>
        public void BulkDelete(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }
        /// <inheritdoc/>
        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
        /// <inheritdoc/>
        public virtual async Task InsertAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
        }
        /// <inheritdoc/>
        public async Task BulkInsert(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }
        /// <inheritdoc/>
        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }
        /// <inheritdoc/>
        public virtual void SetModified(TEntity entityToEntry, IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            foreach (var expression in expressions)
                context.Entry(entityToEntry).Property(expression).IsModified = true;
        }
        /// <inheritdoc/>
        public virtual async Task UpdateAsync(TEntity entityToUpdate, Expression<Func<TEntity, bool>> expression)
        {
            TEntity original = await dbSet.Where(expression).FirstOrDefaultAsync();
            var originalEntry = context.Entry(original);
            foreach (var property in originalEntry.Metadata.GetProperties())
            {
                // remove CreationDate and ModificationDate fields 
                if (property.Name != "CreationDate" && property.Name != "ModificationDate")
                {
                    //Set current value and original value to verify if not are equals
                    // if not are equals set original value from current value
                    var proposedValue = entityToUpdate.GetType()
                        .GetProperty(property.Name).GetValue(entityToUpdate, null);
                    var originalValue = originalEntry
                        .Property(property.Name).OriginalValue;
                    if (proposedValue != null)
                        if (!proposedValue.Equals(originalValue))
                            original.GetType().GetProperty(property.Name)
                                .SetValue(original, proposedValue);
                }

            }
        }
        /// <inheritdoc/>
        public void BulkUpdate(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }
    }
}
