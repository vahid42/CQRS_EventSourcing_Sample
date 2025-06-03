
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Channels;
using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Events;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.CQRS.Comands.CommandHandler
{
    public class DepositCommandHandler : ICommandHandler<DepositCommand, bool>
    {
        private readonly IEventRepository eventrepository;
        private readonly ISnapshotRepository snapshotRepository;
        private const int SnapshotInterval = 5;



        public DepositCommandHandler(IEventRepository Eventrepository, ISnapshotRepository snapshotRepository)
        {
            eventrepository = Eventrepository;
            this.snapshotRepository = snapshotRepository;
           
        }

        public async Task<bool> HandlerAsync(DepositCommand request)
        {
            var account = await LoadAccount.LoadAccountAsync(request.AccountId,eventrepository,snapshotRepository);
            account.Deposit(request.Amount);
            List<Event> events = Convertor.ConvertAccountToEvent(account);
            await eventrepository.AppendEventsAsync(request.AccountId, events, account.CurrentVersion - account.Changes.Count);// expectedVersion = نسخه قبل از تغییرات
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
