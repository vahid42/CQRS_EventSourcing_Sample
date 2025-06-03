using AccountFlow.Client.Dto;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AccountFlow.Client.Service
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient httpClient;

        public AccountService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("AccountAPI");
        }

        public async Task<AccountDto> CreateAsync(RequestAccuont request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("Account/Create", request);
                return await response.Content.ReadFromJsonAsync<AccountDto>();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<AccountDto> DepositAsync(RequestDepositWithdraw request)
        {
            var account = new AccountDto();
            var response = await httpClient.PutAsJsonAsync($"Account/DepositAsync/{request.AccountId}", request);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            if (result)
                account = await httpClient.GetFromJsonAsync<AccountDto>($"Account/GetBalanceAsync?accountId={request.AccountId}");
            return account;
        }

        public async Task<AccountDto> WithdrawAsync(RequestDepositWithdraw request)
        {
            var account = new AccountDto();
            var response = await httpClient.PutAsJsonAsync($"Account/WithdrawAsync/{request.AccountId}", request);
            var result = await response.Content.ReadFromJsonAsync<bool>();
            if (result)
                account = await httpClient.GetFromJsonAsync<AccountDto>($"Account/GetBalanceAsync?accountId={request.AccountId}");
            return account;
        }

        public async Task<AccountDto> GetAsync(Guid accountId)
        {
            var response = await httpClient.GetFromJsonAsync<AccountDto>($"Account/GetBalanceAsync?accountId={accountId}");
            return response;
        }

        public async Task<AccountDtos> GetAccountsAsync()
        {
            var response = await httpClient.GetFromJsonAsync<AccountDtos>("Account/GetAccountsAsync");
            return response;
        }
    }
}
