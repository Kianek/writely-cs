using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using writely.Models;
using writely.Models.Dto;
using Xunit;

namespace writely.integration_tests.Users
{
    public class UsersControllerTest
    {
        public class UnregisteredUserTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
        {
            public UnregisteredUserTest(WebAppFactory<Startup> factory) : base(factory)
            {
            }

            [Fact]
            public async Task RegisterUser()
            {
                var newUserJson = Helpers.CreateRegistrationDto();

                using var response = await _client.PostAsync("/api/users", newUserJson.AsStringContent());
                response.EnsureSuccessStatusCode();
            }
        }


        public class ChangePasswordTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
        {
            public ChangePasswordTest(WebAppFactory<Startup> factory) : base(factory)
            {
            }

            [Fact]
            public async Task ChangePassword()
            {
                var user = await SetUpUser();
                var passwordUpdateDto = new UpdatePasswordDto(
                    "Password123!", "Skibbitydibbity123!");
                var request = passwordUpdateDto.AsStringContent();
                var response = await _client.PatchAsync($"/api/users/{user?.Id}/change-password", request);
                response.EnsureSuccessStatusCode();
            }
        }

        public class RegisteredUserTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
        {
            private readonly AppUser _user;

            public RegisteredUserTest(WebAppFactory<Startup> factory) : base(factory)
            {
                _user = SetUpUser().Result;
                _scope = factory.Services.CreateScope();
                _services = _scope.ServiceProvider;
            }

            [Fact]
            public async Task ActivateAccount()
            {
                var response = await _client.PatchAsync($"/api/users/{_user?.Id}/activate", null);
                response.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task DisableAccount()
            {
                var response = await _client.PatchAsync($"/api/users/{_user?.Id}/disable", null);
                response.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task DeleteAccount()
            {
                var response = await _client.DeleteAsync($"/api/users/{_user?.Id}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}