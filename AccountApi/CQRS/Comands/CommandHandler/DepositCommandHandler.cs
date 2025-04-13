
using System.Text.Json;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class DepositCommandHandler : ICommandHandler<DepositCommand, bool>
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public DepositCommandHandler(IGenericRepository<Account> Accountrepository,
            IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task<bool> HandlerAsync(DepositCommand request)
        {
            var account = await LoadAccountAsync(request.AccountId);
            account.Deposit(request.Amount);
            await accountrepository.UpdateAsync(account);
            List<Event> events = Convertor.ConvertAccountToEvent(account);
            await eventrepository.AddAsync(events);
            account.ClearChanges();
            return true;
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
