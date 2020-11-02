using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using FluentAssertions.Common;
using writely.Exceptions;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Users
{
    public class UserServiceTest
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly UserRegistrationDto _registration;

        public UserServiceTest()
        {
            _mockUserManager = GetMockUserManager();
            _registration = new UserRegistrationDto
            {
                FirstName = "Bob",
                LastName = "Loblaw",
                Email = "bob@gmail.com",
                Username = "bob.loblaw",
                Password = "Password123",
                ConfirmPassword = "Password123"
            };
        }

        [Fact]
        public async void Register_UniqueUser_Successful()
        {
            // Arrange
            _mockUserManager.Setup(
                    um =>
                        um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.Register(_registration);
            
            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void Register_PasswordMismatch_Fail()
        {
            // Arrange
            _registration.ConfirmPassword = "Password123jawefioj";

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.Register(_registration))
                .Should()
                .Throw<PasswordMismatchException>()
                .WithMessage("Passwords must match");
        }

        [Fact]
        public async void Register_UserNotUnique_ThrowsDuplicateEmailException()
        {
            // Arrange
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.Register(_registration))
                .Should()
                .Throw<DuplicateEmailException>()
                .WithMessage("*: bob@gmail.com");
        }

        [Fact]
        public async void GetSignedInUser_SignInSuccessful_ReturnsUserDto()
        {
            // Arrange
            const string userId = "UserId";
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(userId))
                .ReturnsAsync(() => new AppUser() { Id = userId });

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.GetSignedInUser(userId);

            // Assert
            result.Should().BeOfType<UserDto>();
        }

        [Fact]
        public void GetSignedInUser_SignInUnsuccessful_ThrowsUserNotFoundException()
        {
            // Arrange
            var email = "jim@gmail.com";
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(email))
                .ReturnsAsync(() => null);
            
            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.GetSignedInUser(email))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void DeleteAccount_UserDeleted_Successful()
        {
            // Arrange
            const string userId = "UserId";
            var user = new AppUser
            {
                Id = userId
            };
            SetupFindById(user);

            _mockUserManager.Setup(um =>
                    um.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.DeleteAccount(userId);
            
            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void DeleteAccount_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.DeleteAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async Task ActivateAccount_UserFound_Activate_Successful()
        {
            // Arrange
            var user = new AppUser { Id = "UserId", IsAccountActive = false };
            SetupFindById(user);
            SetupUpdateSuccessful();

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.ActivateAccount(user.Id);
            
            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void ActivateAccount_UserNotFound_Fail()
        {
            // Arrange
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.ActivateAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async Task DisableAccount_UserFound_Disable_Successful()
        {
            // Arrange
            const string userId = "UserId";
            SetupFindById(new AppUser { Id = userId, IsAccountActive = true });
            SetupUpdateSuccessful();

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.DisableAccount(userId);
            
            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void DisableAccount_UserNotFound_Fail()
        {
            // Arrange
            _mockUserManager.Setup(um =>
                um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.DisableAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void ChangePassword_UserFound_PasswordChanged_Successful()
        {
            // Arrange 
            const string userId = "UserId";
            const string oldPassword = "SpiffyPassword123";
            const string newPassword = "SpiffyNewPassword123";

            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(userId))
                .ReturnsAsync(new AppUser {Id = userId});
            _mockUserManager.Setup(um =>
                    um.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var service = new UserService(_mockUserManager.Object);
            var result = await service.ChangePassword(userId, oldPassword, newPassword);
            
            // Assert
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public void ChangePassword_UserNotFound_Fail()
        {
            // Arrange
            _mockUserManager.Setup(um =>
                    um.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            // Act & assert
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.ChangePassword("UserId", "Blah123", "Newblah123"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async Task GetUserData_UserFound_DataReturned_Success()
        {
            // Arrange
            var userData = new UserData(new AppUser { Id = "UserId" });
            var mockUserDataService = new Mock<IUserDataService>();
            mockUserDataService.Setup(uds =>
                    uds.LoadUserData(It.IsAny<string>()))
                .ReturnsAsync(userData);

            // Act
            var service = new UserService(GetMockUserManager().Object);
            var result = await service.GetUserData(
                mockUserDataService.Object, It.IsAny<string>());
            
            // Assert
            result.Should().BeOfType<UserData>().Which.Id.IsSameOrEqualTo("UserId");
        }

        [Fact]
        public async Task GetUserData_UserNotFound_Fail()
        {
            // Arrange
            var mockUserDataService = new Mock<IUserDataService>();
            mockUserDataService.Setup(uds =>
                    uds.LoadUserData(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var service = new UserService(GetMockUserManager().Object);
            var result = await service.GetUserData(
                mockUserDataService.Object, It.IsAny<string>());
            
            // Assert
            result.Should().BeNull();
        }

        private void SetupFindById(AppUser user)
        {
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
        }

        private void SetupUpdateSuccessful()
        {
            _mockUserManager.Setup(um =>
                    um.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
        }

        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
