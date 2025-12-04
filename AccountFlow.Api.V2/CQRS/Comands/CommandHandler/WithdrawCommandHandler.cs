using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;
using System.Text.Json;

namespace AccountFlow.Api.V2.CQRS.Comands.CommandHandler
{
    public class WithdrawCommandHandler : ICommandHandler<WithdrawCommand, bool>
    {
        private readonly IEventRepository eventrepository;
        private readonly ISnapshotRepository snapshotRepository;
        private const int SnapshotInterval = 5;
        private AccountSnapshot accountSnapshot;
        private Account account;
        private List<EventBase> eventBases;
        private IEnumerable<Event> events;

        public WithdrawCommandHandler(IEventRepository Eventrepository, ISnapshotRepository snapshotRepository)
        {
            eventrepository = Eventrepository;
            this.snapshotRepository = snapshotRepository;
            account = new Account();
        }

        public async Task<bool> HandlerAsync(WithdrawCommand request)
        {
            var account = await LoadAccount.LoadAccountAsync(request.AccountId, eventrepository, snapshotRepository);
            account.Withdraw(request.Amount);
            List<Event> events = Convertor.ConvertAccountToEvent(account);
            await eventrepository.AppendEventsAsync(request.AccountId, events, 
                account.CurrentVersion - account.Changes.Count);// expectedVersion = نسخه قبل از تغییرات
            await SaveSnapshotIfNeeded(account);
            account.ClearChanges();
            return true;
        }


        private async Task SaveSnapshotIfNeeded(Account account)
        {
            if (account.CurrentVersion % SnapshotInterval == 0)
            {
                await snapshotRepository.SaveAsync(Convertor.ConvertAccountToSnapShot(account));
            }
        }


    }
}