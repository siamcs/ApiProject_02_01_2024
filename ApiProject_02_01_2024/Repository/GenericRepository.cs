using ApiProject_02_01_2024.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


using System.Collections;
using System.Linq.Expressions;

namespace ApiProject_02_01_2024.Repository
{
 
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        protected readonly AppDBContext _context;
      
        public GenericRepository(AppDBContext context)
        {
            _context = context;
         
        }

        public IQueryable<T> All()
        {
            return _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }


    

        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            var transaction =  _context.Database.CurrentTransaction;
            if (transaction != null)
            {
                await transaction.CommitAsync();
                await transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            var transaction =  _context.Database.CurrentTransaction;
            if (transaction != null)
            {
                await transaction.RollbackAsync();
                await transaction.DisposeAsync();
            }
        }

       
    }

}





