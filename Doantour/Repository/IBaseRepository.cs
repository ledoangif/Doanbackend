using System.Linq.Expressions;

namespace Doantour.Repository
{
    public interface IBaseRepository<BaseDTO>
    {
        Task<List<BaseDTO>> ToListAsync();
        Task<List<BaseDTO>> SearchAsync(Expression<Func<BaseDTO, bool>> predicate);
        Task<BaseDTO?> FindAsync(int id);
        Task<BaseDTO> InsertAsync(BaseDTO entity);
        Task<BaseDTO> UpdateAsync(BaseDTO entity);
        Task<BaseDTO> DeleteAsync(BaseDTO entity);
        Task<int> DeleteManyAsync(List<BaseDTO> entities);
        Task<bool> SelectAsync(Expression<Func<BaseDTO, bool>> predicate);
    }
}
