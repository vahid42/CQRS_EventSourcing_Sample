using System.Linq.Expressions;
using AccountFlow.Api.V1.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Api.V1.Repository
{
    public class AccountRepository : IAccountRepository
    {
        protected readonly AccountDbContext context;
        private readonly DbSet<Entities.Account> dbSet;

        public AccountRepository(AccountDbContext context)
        {
            this.context = context;
            dbSet = context.Set<Entities.Account>();
        }
        public async Task<Entities.Account> AddAsync(Entities.Account entity)
        {
            context.Accounts.Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateAsync(Entities.Account entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Entities.Account>> FindAsync(Expression<Func<Entities.Account, bool>> predicate)
         => await dbSet.Where(predicate).ToListAsync();
        public async Task<IEnumerable<Entities.Account>> GetAllAsync() => await dbSet.ToListAsync();
        public Task<Entities.Account> GetByIdAsync(Guid id) => dbSet.FirstOrDefaultAsync(x => x.Id == id);

       
    }
}
