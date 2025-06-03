using System.Linq.Expressions;
using AccountFlow.Api.V1.Entities;

namespace AccountFlow.Api.V1.Repository
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Entities.Account>> GetAllAsync();
        Task<Entities.Account> GetByIdAsync(Guid id);
        Task<Entities.Account> AddAsync(Entities.Account entity);
        Task<bool> UpdateAsync(Entities.Account entity);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Entities.Account>> FindAsync(Expression<Func<Entities.Account, bool>> predicate);
    }
}
