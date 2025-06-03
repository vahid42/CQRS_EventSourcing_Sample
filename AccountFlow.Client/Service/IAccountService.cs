using AccountFlow.Client.Dto;

namespace AccountFlow.Client.Service
{

    public interface IAccountService
    {
        Task<AccountDto> CreateAsync(RequestAccuont request);
        Task<AccountDto> DepositAsync(RequestDepositWithdraw request);
        Task<AccountDto> WithdrawAsync(RequestDepositWithdraw request);
        Task<AccountDto> GetAsync(Guid accountId);
        Task<AccountDtos> GetAccountsAsync();
    }
}
