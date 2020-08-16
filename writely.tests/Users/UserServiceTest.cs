using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using FluentAssertions.Common;
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
                .ReturnsAsync(IdentityResult.Success).Verifiable();

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
            var result = await service.Register(_registration);
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async void Register_UserNotUnique_Fail()
        {
            _mockUserManager.Setup(um =>
                    um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser());
            
            var service = new UserService(_mockUserManager.Object);
            var result = await service.Register(_registration);
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async void DeleteAccount_UserDeleted_Successful()
        {
            const string userId = "UserId";
            var user = new AppUser
            {
                Id = userId
            };
            
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            _mockUserManager.Setup(um =>
                    um.DeleteAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            
            var service = new UserService(_mockUserManager.Object);
            var result = await service.DeleteAccount(userId);
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async void DeleteAccount_UserNotFound_Fail()
        {
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _mockUserManager.Setup(um =>
                    um.DeleteAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Failed());
            
            var service = new UserService(_mockUserManager.Object);
            var result = await service.DeleteAccount("UserId");
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async void DisableAccount_UserFound_Disable_Successful()
        {
            const string userId = "UserId";
            _mockUserManager.Setup(um =>
                    um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new AppUser { Id = userId, IsAccountActive = true});

            _mockUserManager.Setup(um =>
                    um.UpdateAsync(It.IsAny<AppUser>()))
                .ReturnsAsync(IdentityResult.Success);
            
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
            var result = await service.DisableAccount("UserId");
            result.Succeeded.Should().BeFalse();
        }

        [Fact]
        public async void GetUserData_UserFound_DataReturned_Success()
        {
            var userData = new UserData(new AppUser {Id = "UserId"});
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

        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }    
    }
}
