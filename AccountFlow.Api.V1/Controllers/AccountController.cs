using AccountFlow.Api.V1.Dtos;
using AccountFlow.Api.V1.Service;
using Microsoft.AspNetCore.Mvc;

namespace AccountFlow.Api.V1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService account;

        public AccountController(IAccountService account)
        {
            this.account = account;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccunt([FromBody] RequestAccuont request)
        {
            var result = await account.CreateAsync(request);
            return Ok(result);
        }


        [HttpPut("DepositAsync")]
        public async Task<IActionResult> DepositAsync( [FromBody] RequestDepositWithdraw request)
        {
            var result = await account.DepositAsync(request);
            return Ok(result);
        }

        [HttpPut("WithdrawAsync")]
        public async Task<IActionResult> WithdrawAsync( [FromBody] RequestDepositWithdraw request)
        {
            var result = await account.WithdrawAsync(request);
            return Ok(result);
        }

        [HttpGet("GetBalanceAsync")]
        public async Task<IActionResult> GetBalanceAsync([FromQuery] Guid accountId)
        {
            var result = await account.GetAsync(accountId);
            return Ok(result);
        }

        [HttpGet("GetAccountsAsync")]
        public async Task<IActionResult> GetAccountsAsync()
        {
            var result = await account.GetAccountsAsync();
            return Ok(result);
        }
    }
}
