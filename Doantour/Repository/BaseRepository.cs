using Doantour.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Doantour.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly Hachutravelcontext _context;
        public BaseRepository(Hachutravelcontext context)
        {
            _context = context;
        }
        public async Task<T> InsertAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> UpdateAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Modified;
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<T> DeleteAsync(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            entry.State = EntityState.Deleted;
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteManyAsync(List<T> entities)
        {
            var attachedEntities = new List<T>();

            foreach (var entity in entities)
            {
                var entry = _context.Entry(entity);
                if (entry.State == EntityState.Detached)
                {
                    attachedEntities.Add(entity);
                }

                entry.State = EntityState.Deleted;
            }

            _context.Set<T>().AttachRange(attachedEntities);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<T>> ToListAsync()
        {
            return await _context.Set<T>().OrderByDescending(x => x.UpdateDate).Where(x => x.IsDeleted == false).ToListAsync();
        }
        public async Task<List<T>> PendingOrderBy()
        {
            return await _context.Set<T>().OrderByDescending(x => x).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<T?> FindAsync(int id)
        {
            var result = await _context.Set<T>().FindAsync(id);
            if (result == null)
            {
                return null;
            }

            return result;
        }
       
        public async Task<bool> SelectAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<List<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }


    }

}
