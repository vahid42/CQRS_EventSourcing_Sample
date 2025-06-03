using System.Data;
using System;
using System.Linq.Expressions;
using AccountFlow.Api.V2.Entities;
using Microsoft.EntityFrameworkCore;
using AccountFlow.Api.V2.Data;

namespace AccountFlow.Api.V2.Repository
{
    public interface IEventRepository

    {
        Task AppendEventsAsync(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId, int fromVersion = 0);
        Task<IEnumerable<Event>> GetListAggregateAsync();
        Task<int> GetLatestVersionAsync(Guid aggregateId);
        Task<bool> AggregateExistsAsync(Guid aggregateId);
    }

    public class EventRepository : IEventRepository
    {
        private readonly AccountDbContext context;
        private readonly DbSet<Event> dbset;

        public EventRepository(AccountDbContext context)
        {
            this.context = context;
            dbset = context.Set<Event>();
        }

        public async Task AppendEventsAsync(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // بررسی همروندی
                var currentVersion = await GetLatestVersionAsync(aggregateId);

                if (expectedVersion >= 0 && currentVersion != expectedVersion)
                {
                    throw new Exception(
                        $"Concurrency conflict for aggregate {aggregateId}. " +
                        $"Expected version: {expectedVersion}, " +
                        $"Current version: {currentVersion}");
                }

                expectedVersion = expectedVersion + 1;
                foreach (var @event in events)
                {
                    @event.EventVersion = expectedVersion++;
                    await dbset.AddAsync(@event);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<Event>> GetEventsAsync(Guid aggregateId, int fromVersion = 0)
        {
            return await dbset
                .Where(e => e.AggregateId == aggregateId && e.EventVersion >= fromVersion)
                .OrderBy(e => e.EventVersion)
                .ToListAsync();
        }

        public async Task<int> GetLatestVersionAsync(Guid aggregateId)
        {
            return await dbset
                .Where(e => e.AggregateId == aggregateId)
                .MaxAsync(e => (int?)e.EventVersion) ?? 0;
        }

        public async Task<bool> AggregateExistsAsync(Guid aggregateId)
        {
            return await dbset.AnyAsync(e => e.AggregateId == aggregateId);
        }

        public async Task<IEnumerable<Event>> GetListAggregateAsync()
        {

            return await dbset.GroupBy(e => e.AggregateId).Select(c => new Event() { AggregateId = c.Key }).ToListAsync();
        }
    }
}
