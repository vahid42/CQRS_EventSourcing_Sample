using AccountApi.Dtos;
using AccountApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccountApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccunt([FromBody] RequestAccuont request)
        {
            var dd = await accountService.CreateAsync(new CQRS.CreateCommand() { InitialBalance = request.Amount, Name = request.Name });
            return Ok(dd);
        }
        [HttpPut("DepositAsync")]
        public async Task<IActionResult> DepositAsync([FromBody] RequestAccuont request)
        {
            await accountService.DepositAsync(new CQRS.DepositCommand() { AccountId = request.Id, Amount = request.Amount });
            return Ok();
        }

        [HttpPut("WithdrawAsync")]
        public async Task<IActionResult> WithdrawAsync([FromBody] RequestAccuont request)
        {
            await accountService.WithdrawAsync(new CQRS.WithdrawCommand() { AccountId = request.Id, Amount = request.Amount });
            return Ok();
        }

        [HttpGet("GetBalanceAsync")]
        public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid accountId)
        {
            var dd = await accountService.GetBalanceAsync(new CQRS.GetBalanceQuery() { AccountId = accountId });
            return Ok(dd);
        }
    }
}
