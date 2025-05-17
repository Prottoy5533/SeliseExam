using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Infrastructure.Persistent
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<TEntity> EditAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);
        Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entityList);
        Task<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entityList);
        Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entityList);
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(long id);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteByPropertyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
