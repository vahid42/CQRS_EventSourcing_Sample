
using System.Text.Json;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Queries.QueryHandler
{
    public class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, decimal>
    {
        private IGenericRepository<Account> accountrepository;
        private IGenericRepository<Event> eventrepository;

        public GetBalanceQueryHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            this.accountrepository = Accountrepository;
            this.eventrepository = Eventrepository;
        }



        public async Task<decimal> HandlerAsync(GetBalanceQuery request)
        {

            var account = await LoadAccountAsync(request.AccountId);
            return account.Balance;
        }

        private async Task<Account> LoadAccountAsync(Guid accountId)
        {
            var items = await eventrepository.FindAsync(c => c.AggregateId == accountId);
            List<EventBase> events = Convertor.ConvertEventBaseToEvent(items);
            var account = await accountrepository.GetByIdAsync(accountId);
            account.LoadFromEvents(events);
            return account;
        }
    }
}
