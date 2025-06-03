using System.Text.Json;
using AccountFlow.Api.V2.Dtos;
using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.CQRS.Queries.QueryHandler
{
    public class GetAccountsQueryHandler : IQueryHandler<GetAccountsQuery, ResponsAccounts>
    {
        private readonly IEventRepository eventrepository;
        private readonly ISnapshotRepository snapshotRepository;
        private AccountSnapshot accountSnapshot;
        private Account account;
        private List<EventBase> eventBases;
        private IEnumerable<Event> events;
        private List<Account> accounts;

        public GetAccountsQueryHandler(IEventRepository Eventrepository, ISnapshotRepository snapshotRepository)
        {
            this.eventrepository = Eventrepository;
            this.snapshotRepository = snapshotRepository;
           
            accounts=new List<Account>();

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
                    AccuontId = account.Id,
                    Balance = account.Balance,
                    Name = account.Name
                });

            }
            return response;
        }

        private async Task<IEnumerable<Account>> LoadAccountAsync()
        {
            var events = await eventrepository.GetListAggregateAsync();
            foreach (var @event in events)
            {
                var account = await LoadAccount.LoadAccountAsync(@event.AggregateId, eventrepository, snapshotRepository);
                accounts.Add(account);
            }


            return accounts;
        }


    }
}
