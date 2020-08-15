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
        [Fact]
        public async void LoadUserData_UserFound_Success()
        {
            const string userId = "UserId";
            var appUser = new AppUser
            {
                Id = userId
            };

            var mockContext = GenerateMockDbContext();
            mockContext.Setup(ctx =>
                    ctx.Users.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(appUser);
            
            var dataService = new UserDataService(mockContext.Object);
            var result = await dataService.LoadUserData(userId);
            result.Should().BeOfType<UserData>().And.NotBeNull();
        }

        [Fact]
        public async void LoadUserData_UserNotFound_Fail()
        {
            var mockContext = GenerateMockDbContext();
            mockContext.Setup(ctx =>
                    ctx.Users.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);
            
            var service = new UserDataService(mockContext.Object);
            var result = await service.LoadUserData("userId");
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