using Doantour.Helpers.Page;
using System.Linq.Expressions;

namespace Doantour.Service
{
    public interface IBaseService<TEntity, TDto>
    {

        Task<List<TDto>> ToListAsync();
        Task<List<TDto>> SearchAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TDto> FindAsync(int id);
        Task<TDto> InsertAsync(TDto obj);
        Task<TDto> UpdateAsync(int id, TDto obj);
        Task<TDto> DeleteAsync(int id);
        Task<PageResult<TEntity>> GetAllPagination(Pagination? pagination);
    }
}


