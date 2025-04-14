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
                List<EventBase> events = Convertor.ConvertEventBaseToEvent(items);
                account.LoadFromEvents(events);
            }


            return accounts;
        }

    }
}
