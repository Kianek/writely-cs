using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using writely.Models.Dto;
using Xunit;

namespace writely.integration_tests
{
    public class UsersControllerTest : IClassFixture<WebAppFactory<writely.Startup>>
    {
        private HttpClient _client;

        public UsersControllerTest(WebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5000");
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
    }
}