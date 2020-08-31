using System.Net.Http;
using System.Threading.Tasks;
using writely.Models.Dto;
using Xunit;

namespace writely.integration_tests.Auth
{
    public class AuthControllerTest : IClassFixture<WebAppFactory<writely.Startup>>
    {
        private HttpClient _client;

        public AuthControllerTest(WebAppFactory<writely.Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Login_CredentialsValid_ReturnsOk()
        {
            var regInfo = new UserRegistrationDto
            {
                Email = "bob@gmail.com",
                FirstName = "Bob",
                LastName = "Loblaw",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Username = "boblin123"
            };
            var res = await _client.PostAsync("/api/users", regInfo.AsStringContent());
            
            var creds = new UserLoginDto
            {
                Email = regInfo.Email,
                Password = regInfo.Password,
                RememberMe = false
            };
            var result = await _client.PostAsync(
                "/api/auth/login", creds.AsStringContent());
            result.EnsureSuccessStatusCode();
        }
    }
}