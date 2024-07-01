using BankingSystem.Contracts;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("{userId}")]
        public ActionResult<Account> CreateAccount(int userId)
        {
            var account = _accountService.CreateAccount(userId);
            if (account == null) return Ok("Invalid User. Please try again.");
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            if (_accountService.DeleteAccount(id))
            {
                return Ok("Account Deleted Successfully");
            }
            return BadRequest("Something went wrong. Please try again.");
        }

        [HttpPost("deposit/{id}")]
        public IActionResult Deposit(int id, [FromBody] decimal amount)
        {
            if(amount > 10000m)
            {
                return Ok("You cannot deposit more than $10,000 in a single transaction.");
            }
            if (_accountService.Deposit(id, amount))
            {
                var account = _accountService.GetUserAccount(id);
                return Ok($"Amount Deposited Successfully. Your updated balance is : {account.Balance}");
            }
            return BadRequest("Something went wrong. Please try again.");
        }

        [HttpPost("withdraw/{id}")]
        public IActionResult Withdraw(int id, [FromBody] decimal amount)
        {
            var account = _accountService.GetUserAccount(id);

            if (account == null)
            {
                return BadRequest("Account not found.");
            }

            if (amount > account.Balance * 0.9m)
            {
                return BadRequest("You cannot withdraw more than 90% of their total balance from an account in a single transaction.");
            }

            if (account.Balance - amount < 100)
            {
                return BadRequest("Your account cannot have less than $100 at any time.");
            }

            if (_accountService.Withdraw(id, amount))
            {
                return Ok($"Amount withdrawn: {amount}");
            }

            return BadRequest("Withdrawal failed.");
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = _accountService.GetUserAccount(id);
            if (account == null)
            {
                return BadRequest("Account not found.");
            }
            return account;
        }
    }
}
