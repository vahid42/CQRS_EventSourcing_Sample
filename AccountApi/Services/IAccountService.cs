using AccountApi.CQRS;
using AccountApi.CQRS.Comands;

namespace AccountApi.Services
{
    public interface IAccountService
    {
        Task<Guid> CreateAsync(CreateCommand createCommand);
        Task DepositAsync(DepositCommand command);
        Task WithdrawAsync(WithdrawCommand command);
        Task<decimal> GetBalanceAsync(GetBalanceQuery query);

    }
}
