using AccountApi.CQRS;
using AccountApi.CQRS.Comands;
using AccountApi.CQRS.Comands.CommandHandler;
using AccountApi.CQRS.Queries;
using AccountApi.CQRS.Queries.QueryHandler;
using AccountApi.Dtos;
using AccountApi.Entities;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AccountApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> CreateAccunt([FromBody] RequestAccuont request, [FromServices] ICommandHandler<CreateCommand, Guid> command)
        {
            var dd = await command.HandlerAsync(new CreateCommand() { InitialBalance = request.Amount, Name = request.Name, Bonus = request.Bonus });
            return Ok(dd);
        }


        [HttpPut("DepositAsync/{AccountId}")]
        public async Task<IActionResult> DepositAsync([FromRoute] Guid AccountId, [FromBody] RequestDepositWithdraw request, 
            [FromServices] ICommandHandler<DepositCommand, bool> command)
        {
            var result = await command.HandlerAsync(new DepositCommand() { Amount = request.Amount, AccountId = AccountId });
            return Ok(result);
        }

        [HttpPut("WithdrawAsync/{AccountId}")]
        public async Task<IActionResult> WithdrawAsync([FromRoute] Guid AccountId, [FromBody] RequestDepositWithdraw request, 
            [FromServices] ICommandHandler<WithdrawCommand, bool> command)
        {
            var result = await command.HandlerAsync(new WithdrawCommand() { Amount = request.Amount, AccountId = AccountId });
            return Ok(result);
        }

        [HttpGet("GetBalanceAsync")]
        public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid accountId, [FromServices] IQueryHandler<GetBalanceQuery, decimal> query)
        {
            var dd = await query.HandlerAsync(new GetBalanceQuery() { AccountId = accountId });
            return Ok(dd);
        }

        [HttpGet("GetAccountsAsync")]
        public async Task<IActionResult> GetAccountsAsync( [FromServices] IQueryHandler<GetAccountsQuery, ResponsAccounts> query)
        {
            var dd = await query.HandlerAsync(new GetAccountsQuery());
            return Ok(dd);
        }
    }
}
