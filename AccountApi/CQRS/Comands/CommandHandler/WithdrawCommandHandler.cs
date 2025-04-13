using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;
using System.Text.Json;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class WithdrawCommandHandler : ICommandHandler<WithdrawCommand, bool>
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public WithdrawCommandHandler(IGenericRepository<Account> Accountrepository, 
            IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task<bool> HandlerAsync(WithdrawCommand request)
        {
            var account = await LoadAccountAsync(request.AccountId);
            account.Withdraw(request.Amount);
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