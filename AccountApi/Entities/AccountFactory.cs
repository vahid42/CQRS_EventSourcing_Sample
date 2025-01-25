using AccountApi.Events;
using AccountApi.Repository;

namespace AccountApi.Entities
{
    public class AccountFactory
    {
        private readonly IGenericRepository<Event> _eventStore;

        public AccountFactory(IGenericRepository<Event> eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Account> CreateAccountAsync(string name, decimal initialBalance)
        {
            var account = await Account.CreateAccountAsync(name, initialBalance, _eventStore);
            return account;
        }
    }
}
