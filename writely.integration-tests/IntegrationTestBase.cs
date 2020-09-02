using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using writely.Data;
using writely.Models;
using writely.Models.Dto;
using writely.Services;

namespace writely.integration_tests
{
    public class IntegrationTestBase : IDisposable
    {
        protected HttpClient _client;
        protected IServiceScope _scope;
        protected IServiceProvider _services;

        public IntegrationTestBase(WebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    BaseAddress = new Uri("http://localhost:5000")
                });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            _scope = factory.Services.CreateScope();
            _services = _scope.ServiceProvider;
        }

        public async Task<ApplicationDbContext> GetContext() => _services.GetRequiredService<ApplicationDbContext>();
        
        public async Task<IUserService> GetUserService() => _services.GetRequiredService<IUserService>();

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var context = await GetContext();

            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task SignInUser(UserRegistrationDto user)
        {
            var credentials = new UserLoginDto
                {Email = user.Email, Password = user.Password, RememberMe = false};
            await _client.PostAsync("/api/auth/login", credentials.AsStringContent());
        }

        public async Task<AppUser> SetUpUser()
        {
            var newUser = Helpers.CreateRegistrationDto();
            var service = await GetUserService();
            await service.Register(newUser);
            await SignInUser(newUser);

            return await GetUserByEmail(newUser.Email);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}