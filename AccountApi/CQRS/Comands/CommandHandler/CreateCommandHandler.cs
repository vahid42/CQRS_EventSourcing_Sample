using System.Text.Json;
using System.Text.Unicode;
using AccountApi.Entities;
using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Guid>
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public CreateCommandHandler(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task<Guid> HandlerAsync(CreateCommand request)
        {
            var account = new Account(request.Name, request.InitialBalance);
            if (request.Bonus > 0)
                account.Deposit(1000);
            var result = await accountrepository.AddAsync(account);
            List<Event> events = Convertor.ConvertAccountToEvent(account);
            await eventrepository.AddAsync(events);
            account.ClearChanges();
            return result.Id;
        }

        
    }
}
