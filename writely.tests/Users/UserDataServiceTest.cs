using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using writely.Data;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Users
{
    public class UserDataServiceTest
    {
        private Mock<ApplicationDbContext> _mockContext;

        public UserDataServiceTest()
        {
            _mockContext = GenerateMockDbContext();
        }

        [Fact]
        public async void LoadUserData_UserFound_Success()
        {
            // Arrange
            const string userId = "UserId";
            var appUser = new AppUser
            {
                Id = userId
            };

            _mockContext.Setup(ctx =>
                    ctx.Users.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(appUser);
            
            // Act
            var dataService = new UserDataService(_mockContext.Object);
            var result = await dataService.LoadUserData(userId);
            
            // Assert
            result.Should().BeOfType<UserData>().And.NotBeNull();
        }

        [Fact]
        public async void LoadUserData_UserNotFound_Fail()
        {
            // Arrange
            _mockContext.Setup(ctx =>
                    ctx.Users.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            
            // Act
            var service = new UserDataService(_mockContext.Object);
            var result = await service.LoadUserData("userId");
            
            // Assert
            result.Should().BeNull();
        }

        private Mock<ApplicationDbContext> GenerateMockDbContext()
        {
            var dbContextOptions = new DbContextOptions<ApplicationDbContext>();
            var mockAppDbContext = new Mock<ApplicationDbContext>(dbContextOptions);
            return mockAppDbContext;
        }
    }
}