using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using writely.Data;
using writely.Exceptions;
using writely.Models;
using writely.Models.Dto;
using writely.Services;

namespace writely.integration_tests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        private WebAppFactory<Startup> _factory;
        private IServiceScope _scope;
        private IServiceProvider _services;
        protected HttpClient _client;
        protected AppUser _user;

        protected IntegrationTestBase(WebAppFactory<Startup> factory)
        {
            _factory = factory;
        }

        protected void InitializeTestHost()
        {
            _client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    BaseAddress = new Uri("http://localhost:5000")
                });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            _scope = _factory.Services.CreateScope();
            _services = _scope.ServiceProvider;
        }

        protected virtual async Task ArrangeTest()
        {
                InitializeTestHost();
                _user = await SetUpUser();
        }
        
        protected T GetService<T>() => _services.GetRequiredService<T>();
        
        private void ResetDatabase()
        {
            var context = GetContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private ApplicationDbContext GetContext() => _services.GetRequiredService<ApplicationDbContext>();

        private async Task<AppUser> GetUserByEmail(string email)
        {
            var context = GetContext();

            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        private async Task SignInUser(UserRegistrationDto user)
        {
            var credentials = new UserLoginDto
                {Email = user.Email, Password = user.Password, RememberMe = false};
            await _client.PostAsync("/api/auth/login", credentials.AsStringContent());
        }

        private async Task<AppUser> SetUpUser()
        {
            ResetDatabase();
            var newUser = Helpers.CreateRegistrationDto();
            var service = GetService<IUserService>();

            try
            {
                await service.Register(newUser);
            }
            catch (DuplicateEmailException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                await SignInUser(newUser);
            }

            return await GetUserByEmail(newUser.Email);
        }
        
        public virtual void Dispose()
        {
            _client?.Dispose();
            _user = null;
        }
    }
}