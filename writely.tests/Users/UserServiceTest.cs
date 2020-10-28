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
            _mockUserManager.Setup(
                    um =>
                        um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UserService(_mockUserManager.Object);
            var result = await service.Register(_registration);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void Register_PasswordMismatch_Fail()
        {
            string password2 = "Password123jawefioj";
            _registration.ConfirmPassword = password2;

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
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());

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
            const string userId = "UserId";
            var user = new AppUser()
            {
                Id = userId
            };

            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(userId))
                .ReturnsAsync(() => user);

            var service = new UserService(_mockUserManager.Object);
            var result = await service.GetSignedInUser(userId);

            result.Should().BeOfType<UserDto>();
        }

        [Fact]
        public void GetSignedInUser_SignInUnsuccessful_ThrowsUserNotFoundException()
        {
            var email = "jim@gmail.com";
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(email))
                .ReturnsAsync(() => null);
            
            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.GetSignedInUser(email))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void DeleteAccount_UserDeleted_Successful()
        {
            const string userId = "UserId";
            var user = new AppUser
            {
                Id = userId
            };

            SetupFindById(user);

            _mockUserManager.Setup(um =>
                    um.DeleteAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UserService(_mockUserManager.Object);
            var result = await service.DeleteAccount(userId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void DeleteAccount_UserNotFound_ThrowsUserNotFoundException()
        {
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.DeleteAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void ActivateAccount_UserFound_Activate_Successful()
        {
            var user = new AppUser { Id = "UserId", IsAccountActive = false };
            SetupFindById(user);
            SetupUpdateSuccessful();

            var service = new UserService(_mockUserManager.Object);
            var result = await service.ActivateAccount(user.Id);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void ActivateAccount_UserNotFound_Fail()
        {
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.ActivateAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void DisableAccount_UserFound_Disable_Successful()
        {
            const string userId = "UserId";
            SetupFindById(new AppUser { Id = userId, IsAccountActive = true });
            SetupUpdateSuccessful();

            var service = new UserService(_mockUserManager.Object);
            var result = await service.DisableAccount(userId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void DisableAccount_UserNotFound_Fail()
        {
            _mockUserManager.Setup(um =>
                um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var service = new UserService(_mockUserManager.Object);
            service
                .Invoking(us => us.DisableAccount("UserId"))
                .Should()
                .Throw<UserNotFoundException>();
        }

        [Fact]
        public async void ChangePassword_UserFound_PasswordChanged_Successful()
        {
            const string oldPassword = "SpiffyNewPassword123";
            const string newPassword = "SpiffyNewPassword123";
            _mockUserManager.Setup(um =>
                    um.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UserService(_mockUserManager.Object);
            var result = await service.ChangePassword("UserId", oldPassword, newPassword);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void ChangePassword_UserNotFound_Fail()
        {
            _mockUserManager.Setup(um =>
                    um.ChangePasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var service = new UserService(_mockUserManager.Object);
            var result = await service.ChangePassword("UserId", "Blah123", "Newblah123");
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async void GetUserData_UserFound_DataReturned_Success()
        {
            var userData = new UserData(new AppUser { Id = "UserId" });
            var mockUserDataService = new Mock<IUserDataService>();
            mockUserDataService.Setup(uds =>
                    uds.LoadUserData(It.IsAny<string>()))
                .ReturnsAsync(userData);

            var service = new UserService(GetMockUserManager().Object);
            var result = await service.GetUserData(
                mockUserDataService.Object, It.IsAny<string>());
            result.Should().BeOfType<UserData>().Which.Id.IsSameOrEqualTo("UserId");
        }

        [Fact]
        public async void GetUserData_UserNotFound_Fail()
        {
            var mockUserDataService = new Mock<IUserDataService>();
            mockUserDataService.Setup(uds =>
                    uds.LoadUserData(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var service = new UserService(GetMockUserManager().Object);
            var result = await service.GetUserData(
                mockUserDataService.Object, It.IsAny<string>());
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
