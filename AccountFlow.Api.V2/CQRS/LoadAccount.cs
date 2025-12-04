using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.CQRS
{
    public static class LoadAccount
    {
        public static async Task<Account> LoadAccountAsync(Guid accountId, 
            IEventRepository eventrepository, 
            ISnapshotRepository snapshotRepository)
        {
            Account account = new Account();
            AccountSnapshot accountSnapshot = new AccountSnapshot();
            List<EventBase> eventBases = new List<EventBase>();
            IEnumerable<Event> events = new List<Event>();

            accountSnapshot = await snapshotRepository.GetLatestAsync(accountId);
            if (accountSnapshot != null)
            {
                account.InitializeFromSnapshot(accountSnapshot);
                events = await eventrepository.GetEventsAsync(accountId, accountSnapshot.CurrentVersion + 1);
            }
            else
            {
                events = await eventrepository.GetEventsAsync(accountId, 0);
            }
            eventBases = Convertor.ConvertEventBaseToEvent(events);
            account.LoadFromEvents(eventBases);
            return account;
        }
    }
}
