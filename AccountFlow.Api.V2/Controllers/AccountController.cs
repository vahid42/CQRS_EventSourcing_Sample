using AccountFlow.Api.V2.CQRS;
using AccountFlow.Api.V2.CQRS.Comands;
using AccountFlow.Api.V2.CQRS.Comands.CommandHandler;
using AccountFlow.Api.V2.CQRS.Queries;
using AccountFlow.Api.V2.CQRS.Queries.QueryHandler;
using AccountFlow.Api.V2.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Api.V2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RequestAccuont request, [FromServices] ICommandHandler<CreateCommand, Guid> command)
        {
            var dd = await command.HandlerAsync(new CreateCommand() { InitialBalance = request.Amount, Name = request.Name, Bonus = request.Bonus });

            return Ok(new ResponseAccuont() { Balance = request.Amount + request.Bonus, Name = request.Name, AccuontId = dd });
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
        public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid accountId, [FromServices] IQueryHandler<GetBalanceQuery, ResponseAccuont> query)
        {
            var dd = await query.HandlerAsync(new GetBalanceQuery() { AccountId = accountId });
            return Ok(dd);
        }

        [HttpGet("GetAccountsAsync")]
        public async Task<IActionResult> GetAccountsAsync([FromServices] IQueryHandler<GetAccountsQuery, ResponsAccounts> query)
        {
            var dd = await query.HandlerAsync(new GetAccountsQuery());
            return Ok(dd);
        }
    }
}
