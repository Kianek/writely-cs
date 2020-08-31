using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using writely.Controllers;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Users
{
    public class UsersControllerTest
    {
        private ILogger<UsersController> _logger;
        private UsersController _controller;

        public UsersControllerTest()
        {
            var mockLogger = new Mock<ILogger<UsersController>>();
            _logger = mockLogger.Object;
        }

        [Fact]
        public async Task Register_UserInfoValid_ReturnsOk()
        {
            var newUser = new UserRegistrationDto
            {
                FirstName = "Bob", LastName = "Loblaw", Email = "bob@loblawlaw.com",
                Username = "bob.loblaw", Password = "Password123!", ConfirmPassword = "Password123!"
            };

            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.Register(It.IsAny<UserRegistrationDto>()))
                .ReturnsAsync(IdentityResult.Success);
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.Register(newUser);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Register_UserInfoInvalid_ReturnsBadRequest()
        {
            var invalidRegistration = new UserRegistrationDto();
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.Register(It.IsAny<UserRegistrationDto>()))
                .ReturnsAsync(IdentityResult.Failed());
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.Register(invalidRegistration);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteAccount_AccountDeleted_ReturnsOk()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.DeleteAccount(It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.DeleteAccount("UserToDeleteId");
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ActivateAccount_UserFound_AccountActivated_ReturnsOk()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.ActivateAccount(It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.ActivateAccount("UserId");
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task ActivateAccount_UserNotFound_ReturnsBadRequest()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.ActivateAccount(It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.ActivateAccount("UserId");
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteAccount_DeletionFailed_ReturnsBadRequest()
        {
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.DeleteAccount(It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.DeleteAccount("InvalidUserId");
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetUserData_UserFound_ReturnsOk()
        {
            var mockDataService = new Mock<IUserDataService>();
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.GetUserData(mockDataService.Object, It.IsAny<string>()))
                .ReturnsAsync(new UserData());
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.GetUserData("UserId", mockDataService.Object);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ChangePassword_UserFound_PasswordChanged_ReturnsOk()
        {
            var updatePasswords = new UpdatePasswordDto("OldPassword123!", "NewPassword123!");
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.ChangePassword("UserId", updatePasswords);
            result.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public async Task ChangePassword_UserFound_PasswordChanged_ReturnsBadRequest()
        {
            var updatePasswords = new UpdatePasswordDto("OldPassword123!", "NewPassword123!");
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.ChangePassword("UserId", updatePasswords);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GetUserData_UserNotFound_ReturnsBadRequest()
        {
            var mockDataService = new Mock<IUserDataService>();
            var mockService = new Mock<IUserService>();
            mockService.Setup(s =>
                    s.GetUserData(mockDataService.Object, It.IsAny<string>()))
                .ReturnsAsync(() => null);
            
            _controller = new UsersController(mockService.Object, _logger);

            var result = await _controller.GetUserData("UserId", mockDataService.Object);
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}