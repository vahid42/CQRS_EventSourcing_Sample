
using AccountApi.Services;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class DepositCommandHandler : ICommandHandler<DepositCommand, bool>
    {
        private readonly IAccountService accountService;
        public DepositCommandHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<bool> HandlerAsync(DepositCommand request)
        {
            await accountService.DepositAsync(request);
            return true;
        }
    }
}
