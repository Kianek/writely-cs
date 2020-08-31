using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using writely.Data;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.integration_tests.Users
{
    public class UsersControllerTest : IClassFixture<WebAppFactory<Startup>>, IDisposable
    {
        private HttpClient _client;
        private WebAppFactory<Startup> _factory;
        private IServiceScope _scope;
        private IServiceProvider _services;

        public UsersControllerTest(WebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5000");
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _services = _scope.ServiceProvider;
        }

        [Fact]
        public async Task RegisterUser()
        {
            var newUserJson = new UserRegistrationDto
            {
                Email = "bob@loblawlaw.com",
                FirstName = "Bob",
                LastName = "Loblaw",
                Username = "bob.loblaw",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            using var response = await _client.PostAsync("/api/users", newUserJson.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteAccount()
        {
            var newUser = new UserRegistrationDto
            {
                Email = "john@doemail.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Username = "j.doe"
            };

            var context = _services.GetRequiredService<ApplicationDbContext>();
            var service = _services.GetRequiredService<IUserService>();
            await service.Register(newUser);
            var user = await context.Users.FirstOrDefaultAsync(u
                => u.Email == newUser.Email);

            var response = await _client.DeleteAsync($"api/users/{user?.Id}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DisableAccount()
        {
            var newUser = new UserRegistrationDto
            {
                Email = "john@doemail.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Username = "j.doe"
            };

            var context = _services.GetRequiredService<ApplicationDbContext>();
            var service = _services.GetRequiredService<IUserService>();
            await service.Register(newUser);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);

            var response = await _client.PutAsync($"/api/users/{user?.Id}/disable", null);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}