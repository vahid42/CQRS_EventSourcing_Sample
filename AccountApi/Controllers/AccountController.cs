using AccountApi.CQRS;
using AccountApi.CQRS.Comands;
using AccountApi.CQRS.Comands.CommandHandler;
using AccountApi.CQRS.Queries.QueryHandler;
using AccountApi.Dtos;
using AccountApi.Services;
using Microsoft.AspNetCore.Mvc;
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
            var dd = await command.HandlerAsync(new CreateCommand() { InitialBalance = request.Amount, Name = request.Name });
            return Ok(dd);
        }


        [HttpPut("DepositAsync")]
        public async Task<IActionResult> DepositAsync([FromBody] RequestAccuont request, [FromServices] ICommandHandler<DepositCommand, bool> command)
        {
            var result = await command.HandlerAsync(new DepositCommand() { AccountId = request.Id, Amount = request.Amount });
            return Ok(result);
        }

        [HttpPut("WithdrawAsync")]
        public async Task<IActionResult> WithdrawAsync([FromBody] RequestAccuont request, [FromServices] ICommandHandler<WithdrawCommand, bool> command)
        {
            var result = await command.HandlerAsync(new WithdrawCommand() { AccountId = request.Id, Amount = request.Amount });
            return Ok(result);
        }

        [HttpGet("GetBalanceAsync")]
        public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid accountId, [FromServices] IQueryHandler<GetBalanceQuery, decimal> command)
        {
            var dd = await command.HandlerAsync(new GetBalanceQuery() { AccountId = accountId });
            return Ok(dd);
        }
    }
}
