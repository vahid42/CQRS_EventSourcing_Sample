using AccountApi.CQRS;
using AccountApi.Entities;
using AccountApi.Repository;

namespace AccountApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Account> accountrepository;
        private readonly IGenericRepository<Event> eventrepository;

        public AccountService(IGenericRepository<Account> Accountrepository, IGenericRepository<Event> Eventrepository)
        {
            accountrepository = Accountrepository;
            eventrepository = Eventrepository;
        }

        public async Task DepositAsync(DepositCommand command)
        {
            var account = await LoadAccountAsync(command.AccountId);
            await account.DepositAsync(command.Amount, eventrepository);
        }

        public async Task WithdrawAsync(WithdrawCommand command)
        {
            var account = await LoadAccountAsync(command.AccountId);
            await account.WithdrawAsync(command.Amount, eventrepository);
        }

        public async Task<decimal> GetBalanceAsync(GetBalanceQuery query)
        {
            var account = await LoadAccountAsync(query.AccountId);
            return account.Balance;
        }

        private async Task<Account> LoadAccountAsync(Guid accountId)
        {
            var events = await eventrepository.FindAsync(c => c.AggregateId == accountId);
            var account = await accountrepository.GetByIdAsync(accountId);
            account.LoadFromEvents(events);
            return account;
        }

        public async Task<string> Create(CreateCommand createCommand)
        {
            var account = await (new AccountFactory(eventrepository) { }).CreateAccountAsync(createCommand.Name, createCommand.InitialBalance);
            var result = await accountrepository.AddAsync(account);
            return result.Id.ToString();
        }
    }
}
