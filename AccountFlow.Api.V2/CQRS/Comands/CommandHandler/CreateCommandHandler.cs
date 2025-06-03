using AccountFlow.Api.V2.Entities;
using AccountFlow.Api.V2.Repository;

namespace AccountFlow.Api.V2.CQRS.Comands.CommandHandler
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Guid>
    {
        private readonly IEventRepository eventrepository;

        public CreateCommandHandler( IEventRepository Eventrepository)
        {
            eventrepository = Eventrepository;
        }

        public async Task<Guid> HandlerAsync(CreateCommand request)
        {
            var account = new Account(request.Name, request.InitialBalance);
            List<Event> events = Convertor.ConvertAccountToEvent(account);
            await eventrepository.AppendEventsAsync(account.Id, events, 0);
            account.ClearChanges();
            return account.Id;
        }


    }
}
