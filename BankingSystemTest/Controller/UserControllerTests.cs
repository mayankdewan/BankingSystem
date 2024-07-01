using BankingSystem;
using BankingSystem.Controllers;
using BankingSystem.Models;
using BankingSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BankingSystem.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemTest.Controller
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public void CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var userName = "John Doe";
            var createdUser = new User { Id = 1, Name = userName };
            _userServiceMock.Setup(us => us.CreateUser(userName)).Returns(createdUser);

            // Act
            var result = _controller.CreateUser(userName) as ActionResult<User>;
            var user = result.Value as User;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(user);
            Assert.Equal(createdUser.Id, user.Id);
            Assert.Equal(createdUser.Name, user.Name);
        }

        [Fact]
        public void DeleteUser_ShouldReturnOkResult_WhenUserDeleted()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(us => us.DeleteUser(userId)).Returns(true);

            // Act
            var result = _controller.DeleteUser(userId) as IActionResult;
            var okResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(okResult);
            Assert.Equal("User Deleted Successfully", okResult.Value);
        }

        [Fact]
        public void DeleteUser_ShouldReturnBadRequest_WhenUserNotDeleted()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(us => us.DeleteUser(userId)).Returns(false);

            // Act
            var result = _controller.DeleteUser(userId) as IActionResult;
            var badRequestResult = result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Something went wrong. Please try again.", badRequestResult.Value);
        }

        [Fact]
        public void GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User { Id = userId, Name = "John Doe" };
            _userServiceMock.Setup(us => us.GetUser(userId)).Returns(existingUser);

            // Act
            var result = _controller.GetUser(userId) as ActionResult<User>;
            var user = result.Value as User;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(user);
            Assert.Equal(existingUser.Id, user.Id);
            Assert.Equal(existingUser.Name, user.Name);
        }

        [Fact]
        public void GetUser_ShouldReturnBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(us => us.GetUser(userId)).Returns((User)null);

            // Act
            var result = _controller.GetUser(userId) as ActionResult<User>;
            var badRequestResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(badRequestResult);
            Assert.Equal("Account not found.", badRequestResult.Value);
        }
    }
}
