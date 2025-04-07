using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DataContext dbContext)
        {
            _dataContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<bool> DeleteById(int id)
        {
            var entity = await _dataContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;  // Entity not found
            }

            _dataContext.Set<T>().Remove(entity);
            await _dataContext.SaveChangesAsync();  // Save changes to the database
            return true;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dataContext.SaveChangesAsync();  // Save changes to the database
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dataContext.SaveChangesAsync();  // Save changes to the database
            return entity;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

    }
}
