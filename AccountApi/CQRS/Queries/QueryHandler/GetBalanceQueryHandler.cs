
using AccountApi.Services;

namespace AccountApi.CQRS.Queries.QueryHandler
{
    public class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, decimal>
    {
        private readonly IAccountService accountService;

        public GetBalanceQueryHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task<decimal> HandlerAsync(GetBalanceQuery request)
        {
            return await accountService.GetBalanceAsync(request);
        }
    }
}
