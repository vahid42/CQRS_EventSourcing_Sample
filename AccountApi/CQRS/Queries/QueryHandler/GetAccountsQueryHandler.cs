using System.Text.Json;
using AccountApi.Dtos;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Queries.QueryHandler
{
    public class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, ResponsAccounts>
    {
        private IGenericRepository<Account> accountrepository;
        private IGenericRepository<Event> eventrepository;

        public GetAccountsQueryHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            this.accountrepository = Accountrepository;
            this.eventrepository = Eventrepository;
        }



        public async Task<ResponsAccounts> HandlerAsync(GetAccountsQuery request)
        {

            var accounts = await LoadAccountAsync();
            var response = new ResponsAccounts();
            response.Accounts = new List<ResponseAccuont>();

            foreach (var account in accounts)
            {
                response.Accounts.Add(new ResponseAccuont()
                {
                    Id = account.Id,
                    Balance = account.Balance,
                    Name = account.Name
                });

            }
            return response;
        }

        private async Task<IEnumerable<Account>> LoadAccountAsync()
        {

            IEnumerable<Account> accounts = await accountrepository.GetAllAsync();
            foreach (var account in accounts)
            {
                var items = await eventrepository.FindAsync(c => c.AggregateId == account.Id);
                var events = new List<EventBase>();
                foreach (var item in items.OrderBy(e => e.OccurredOn))
                {
                    switch (item.EventType)
                    {
                        case "Account":
                            var createdEvent = JsonSerializer.Deserialize<CreatedEvent>(item.EventData);
                            events.Add(new CreatedEvent() { initialBalance = createdEvent.initialBalance, Name = createdEvent.Name, NameOf = createdEvent.NameOf });
                            break;
                        case "Withdraw":
                            var withdrawnEvent = JsonSerializer.Deserialize<WithdrawnEvent>(item.EventData);
                            events.Add(new WithdrawnEvent() { Amount = withdrawnEvent.Amount, NameOf = withdrawnEvent.NameOf });
                            break;
                        case "Deposit":
                            var depositedEvent = JsonSerializer.Deserialize<DepositedEvent>(item.EventData);
                            events.Add(new DepositedEvent() { Amount = depositedEvent.Amount, NameOf = depositedEvent.NameOf });
                            break;
                    }
                }
                account.LoadFromEvents(events);
            }


            return accounts;
        }
    }
}
