using System;
using System.Threading.Tasks;
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
                InitializeTestHost();
                var newUserJson = Helpers.CreateRegistrationDto();

                var response = await _client.PostAsync("/api/users", newUserJson.AsStringContent());
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
                await ArrangeTest();
                var passwordUpdateDto = new UpdatePasswordDto(
                    "Password123!", "Skibbitydibbity123!");
                var request = passwordUpdateDto.AsStringContent();
                var response = await _client.PatchAsync($"/api/users/{_user?.Id}/change-password", request);
                response.EnsureSuccessStatusCode();
            }
        }
        
        public class RegisteredUserActivationTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>, IDisposable
        {
            public RegisteredUserActivationTest(WebAppFactory<Startup> factory) : base(factory)
            {
            }
            
            [Fact]
            public async Task ActivateAccount()
            {
                await ArrangeTest();
                var response = await _client.PatchAsync($"/api/users/{_user?.Id}/activate", null);
                response.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task DisableAccount()
            {
                await ArrangeTest();
                var response = await _client.PatchAsync($"/api/users/{_user?.Id}/disable", null);
                response.EnsureSuccessStatusCode();
            }
        }

        public class RegisteredUserTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
        {
            public RegisteredUserTest(WebAppFactory<Startup> factory) : base(factory)
            {
            }

            [Fact]
            public async Task DeleteAccount()
            {
                await ArrangeTest();
                var response = await _client.DeleteAsync($"/api/users/{_user?.Id}");
                response.EnsureSuccessStatusCode();
            }

            [Fact]
            public async Task GetUserData()
            {
                await ArrangeTest();
                var response = await _client.GetAsync($"/api/users/{_user?.Id}");
                response.EnsureSuccessStatusCode();
            }
        }
    }
}