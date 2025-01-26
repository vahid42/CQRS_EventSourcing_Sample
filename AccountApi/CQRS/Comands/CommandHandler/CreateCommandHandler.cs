using AccountApi.Services;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class CreateCommandHandler : ICommandHandler<CreateCommand, Guid>
    {
        private readonly IAccountService accountService;

        public CreateCommandHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<Guid> HandlerAsync(CreateCommand request)
        {
            var result = await accountService.CreateAsync(request);
            return result;
        }
    }
}
