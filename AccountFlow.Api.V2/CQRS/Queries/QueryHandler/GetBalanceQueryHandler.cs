
using System.Security.Principal;
using System.Text.Json;
using AccountFlow.Api.V2.Dtos;
using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.CQRS.Queries.QueryHandler
{
    public class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, ResponseAccuont>
    {
        private readonly IEventRepository eventrepository;
        private readonly ISnapshotRepository snapshotRepository;
        private AccountSnapshot accountSnapshot;
        private Account account;
        private List<EventBase> eventBases;
        private IEnumerable<Event> events;

        public GetBalanceQueryHandler(IEventRepository Eventrepository, ISnapshotRepository snapshotRepository)
        {
            this.eventrepository = Eventrepository;
            this.snapshotRepository = snapshotRepository;
            account = new Account();

        }



        public async Task<ResponseAccuont> HandlerAsync(GetBalanceQuery request)
        {

            var account = await LoadAccount.LoadAccountAsync(request.AccountId, eventrepository, snapshotRepository);
            return new ResponseAccuont() { AccuontId = account.Id, Name = account.Name, Balance = account.Balance };
        }

    }
}
