using AccountApi.Services;

namespace AccountApi.CQRS.Comands.CommandHandler
{
    public class WithdrawCommandHandler : ICommandHandler<WithdrawCommand, bool>
    {
        private readonly IAccountService accountService;
        public WithdrawCommandHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<bool> HandlerAsync(WithdrawCommand request)
        {
            await accountService.WithdrawAsync(request);
            return true;
        }
    }
}