using AccountApi.CQRS;

namespace AccountApi.Services
{
    public interface IAccountService
    {
        Task<string> Create(CreateCommand createCommand);
        Task DepositAsync(DepositCommand command);
        Task WithdrawAsync(WithdrawCommand command);
        Task<decimal> GetBalanceAsync(GetBalanceQuery query);

    }
}
