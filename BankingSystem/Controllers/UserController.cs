using BankingSystem.Contracts;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace BankingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] string name)
        {
            var user = _userService.CreateUser(name);
            return user;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (_userService.DeleteUser(id))
            {
                return Ok("User Deleted Successfully");
            }
            return BadRequest("Something went wrong. Please try again.");
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _userService.GetUser(id);
            if (user == null)
            {
                return BadRequest("Account not found.");
            }
            return user;
        }
    }
}
