using AccountFlow.Api.V2.Data;
using AccountFlow.Api.V2.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountFlow.Api.V2.Repository
{
    public interface ISnapshotRepository
    {
        Task<AccountSnapshot> GetLatestAsync(Guid accountId);
        Task<bool> SaveAsync(AccountSnapshot snapshot);

    }
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly AccountDbContext context;
        private readonly DbSet<AccountSnapshot> dbSet;

        public SnapshotRepository(AccountDbContext context)
        {
            this.context = context;
            dbSet = context.Set<AccountSnapshot>();
        }
        public async Task<AccountSnapshot> GetLatestAsync(Guid accountId)
        {
            return await dbSet.OrderByDescending(x => x.CurrentVersion).FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task<bool> SaveAsync(AccountSnapshot snapshot)
        {
            dbSet.Add(snapshot);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
