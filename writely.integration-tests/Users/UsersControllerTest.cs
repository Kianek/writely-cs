using System.Threading.Tasks;
using Xunit;

namespace writely.integration_tests.Users
{
    public class UsersControllerTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
    {
        public UsersControllerTest(WebAppFactory<Startup> factory) : base(factory)
        {}

        [Fact]
        public async Task RegisterUser()
        {
            var newUserJson = Helpers.CreateRegistrationDto();

            using var response = await _client.PostAsync("/api/users", newUserJson.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task ActivateAccount()
        {
            var user = await SetUpUser();
            var response = await _client.PatchAsync($"/api/users/{user?.Id}/activate", null);
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteAccount()
        {
            var user = await SetUpUser();
            var response = await _client.DeleteAsync($"api/users/{user?.Id}");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DisableAccount()
        {
            var user = await SetUpUser();
            var response = await _client.PatchAsync($"/api/users/{user?.Id}/disable", null);
            response.EnsureSuccessStatusCode();
        }
    }
}