using BankingSystem.Contracts;
using BankingSystem.Controllers;
using BankingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystemTest.Controller
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _controller = new AccountController(_accountServiceMock.Object);
        }

        [Fact]
        public void CreateAccount_ShouldReturnCreatedAccount()
        {
            // Arrange
            var userId = 1;
            var createdAccount = new Account { Id = 1, UserId = userId, Balance = 100 };
            _accountServiceMock.Setup(x => x.CreateAccount(userId)).Returns(createdAccount);

            // Act
            var result = _controller.CreateAccount(userId) as ActionResult<Account>;
            var createdResult = result.Result as CreatedAtActionResult;
            var account = createdResult.Value as Account;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(createdResult);
            Assert.NotNull(account);
            Assert.Equal(createdAccount.Id, account.Id);
            Assert.Equal(createdAccount.UserId, account.UserId);
            Assert.Equal(createdAccount.Balance, account.Balance);
        }

        [Fact]
        public void CreateAccount_ShouldReturnNotFound_WhenAccountNotCreated()
        {
            // Arrange
            var userId = 1;
            _accountServiceMock.Setup(x => x.CreateAccount(userId)).Returns((Account)null);

            // Act
            var result = _controller.CreateAccount(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteAccount_ShouldReturnOkResult_WhenAccountDeleted()
        {
            // Arrange
            var accountId = 1;
            _accountServiceMock.Setup(x => x.DeleteAccount(accountId)).Returns(true);

            // Act
            var result = _controller.DeleteAccount(accountId) as IActionResult;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal("Account Deleted Successfully", okResult.Value);
        }

        [Fact]
        public void DeleteAccount_ShouldReturnBadRequest_WhenAccountNotDeleted()
        {
            // Arrange
            var accountId = 1;
            _accountServiceMock.Setup(x => x.DeleteAccount(accountId)).Returns(false);

            // Act
            var result = _controller.DeleteAccount(accountId) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Something went wrong. Please try again.", badRequestResult.Value);
        }

        [Fact]
        public void Deposit_ShouldReturnOkResult_WhenDepositSuccessful()
        {
            // Arrange
            var accountId = 1;
            var amount = 500m;
            var updatedAccount = new Account { Id = accountId, UserId = 1, Balance = 600 };
            _accountServiceMock.Setup(x => x.Deposit(accountId, amount)).Returns(true);
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(updatedAccount);

            // Act
            var result = _controller.Deposit(accountId, amount) as IActionResult;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal($"Amount Deposited Successfully. Your updated balance is : {updatedAccount.Balance}", okResult.Value);
        }

        [Fact]
        public void Deposit_ShouldReturnMessageResult_WhenDepositMore()
        {
            // Arrange
            var accountId = 1;
            var amount = 11000m;
            var updatedAccount = new Account { Id = accountId, UserId = 1, Balance = 600 };
            _accountServiceMock.Setup(x => x.Deposit(accountId, amount)).Returns(true);
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(updatedAccount);

            // Act
            var result = _controller.Deposit(accountId, amount) as IActionResult;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal("You cannot deposit more than $10,000 in a single transaction.", okResult.Value);
        }

        [Fact]
        public void Deposit_ShouldReturnBadRequest_WhenDepositFails()
        {
            // Arrange
            var accountId = 1;
            var amount = 500m;
            _accountServiceMock.Setup(x => x.Deposit(accountId, amount)).Returns(false);

            // Act
            var result = _controller.Deposit(accountId, amount) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Something went wrong. Please try again.", badRequestResult.Value);
        }

        [Fact]
        public void Withdraw_ShouldReturnOkResult_WhenWithdrawSuccessful()
        {
            // Arrange
            var accountId = 1;
            var amount = 50m;
            var updatedAccount = new Account { Id = accountId, UserId = 1, Balance = 500 };
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(updatedAccount);
            _accountServiceMock.Setup(x => x.Withdraw(accountId, amount)).Returns(true);

            // Act
            var result = _controller.Withdraw(accountId, amount) as IActionResult;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal($"Amount withdrawn: {amount}", okResult.Value);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            var amount = 50m;
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns((Account)null);

            // Act
            var result = _controller.Withdraw(accountId, amount) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Account not found.", badRequestResult.Value);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenWithdrawExceeds90Percent()
        {
            // Arrange
            var accountId = 1;
            var amount = 91m;
            var account = new Account { Id = accountId, UserId = 1, Balance = 100 };
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(account);

            // Act
            var result = _controller.Withdraw(accountId, amount) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("You cannot withdraw more than 90% of their total balance from an account in a single transaction.", badRequestResult.Value);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenWithdrawLeadsToLessThan100Balance()
        {
            // Arrange
            var accountId = 1;
            var amount = 10m;
            var account = new Account { Id = accountId, UserId = 1, Balance = 109 };
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(account);

            // Act
            var result = _controller.Withdraw(accountId, amount) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Your account cannot have less than $100 at any time.", badRequestResult.Value);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenWithdrawFails()
        {
            // Arrange
            var accountId = 1;
            var amount = 50m;
            var account = new Account { Id = accountId, UserId = 1, Balance = 100 };
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(account);
            _accountServiceMock.Setup(x => x.Withdraw(accountId, amount)).Returns(false);

            // Act
            var result = _controller.Withdraw(accountId, amount) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Your account cannot have less than $100 at any time.", badRequestResult.Value);
        }

        [Fact]
        public void GetAccount_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            var existingAccount = new Account { Id = accountId, UserId = 1, Balance = 100 };
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns(existingAccount);

            // Act
            var result = _controller.GetAccount(accountId) as ActionResult<Account>;
            var account = result.Value as Account;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(account);
            Assert.Equal(existingAccount.Id, account.Id);
            Assert.Equal(existingAccount.UserId, account.UserId);
            Assert.Equal(existingAccount.Balance, account.Balance);
        }

        [Fact]
        public void GetAccount_ShouldReturnBadRequest_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = 1;
            _accountServiceMock.Setup(x => x.GetUserAccount(accountId)).Returns((Account)null);

            // Act
            var result = _controller.GetAccount(accountId) as ActionResult<Account>;
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Account not found.", badRequestResult.Value);
        }
    }
}
