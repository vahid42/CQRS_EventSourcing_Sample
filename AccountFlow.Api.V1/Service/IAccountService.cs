using AccountFlow.Api.V1.Data;
using AccountFlow.Api.V1.Dtos;

namespace AccountFlow.Api.V1.Service
{
    public interface IAccountService
    {
        Task<ResponseAccuont> CreateAsync(RequestAccuont RequestAccuont);
        Task<bool> DepositAsync(RequestDepositWithdraw request);
        Task<bool> WithdrawAsync(RequestDepositWithdraw request);
        Task<ResponseAccuont> GetAsync(Guid accountId);
        Task<ResponsAccounts> GetAccountsAsync();
    }

}
