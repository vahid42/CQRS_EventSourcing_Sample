using AccountFlow.Api.V1.Dtos;
using AccountFlow.Api.V1.Repository;

namespace AccountFlow.Api.V1.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository repository;

        public AccountService(IAccountRepository repository)
        {
            this.repository = repository;
        }
        public async Task<ResponseAccuont> CreateAsync(RequestAccuont RequestAccuont)
        {

            var result = await repository.AddAsync(new Entities.Account(RequestAccuont.Name, RequestAccuont.Amount));
            return new ResponseAccuont()
            {
                Name = RequestAccuont.Name,
                Balance = result.Balance,
                Id = result.Id,
            };
        }

        public async Task<bool> WithdrawAsync(RequestDepositWithdraw request)
        {

            var account = await repository.GetByIdAsync(request.AccountId);
            account.Withdraw(request.Amount);
            await repository.UpdateAsync(account);
            return true;

        }
        public async Task<bool> DepositAsync(RequestDepositWithdraw request)
        {

            var account = await repository.GetByIdAsync(request.AccountId);
            account.Deposit(request.Amount);
            await repository.UpdateAsync(account);
            return true;
        }

        public async Task<ResponseAccuont> GetAsync(Guid accountId)
        {
            var result = await repository.GetByIdAsync(accountId);
            return new ResponseAccuont() { Balance = result.Balance, Id = result.Id, Name = result.Name };
        }

        public async Task<ResponsAccounts> GetAccountsAsync()
        {
            var list = new ResponsAccounts() { Accounts = new List<ResponseAccuont>() };
            var result = await repository.GetAllAsync();
            foreach (var account in result)
            {
                list.Accounts.Add(new ResponseAccuont()
                {
                    Balance = account.Balance,
                    Id = account.Id,
                    Name = account.Name
                });
            }
            return list;
        }


    }

}
