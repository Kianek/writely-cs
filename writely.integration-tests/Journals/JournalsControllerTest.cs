using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.integration_tests.Journals
{
    public class JournalsControllerTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
    {
        private AppUser _user;
        private IJournalService _service;
        private string _baseUrl;
        
        public JournalsControllerTest(WebAppFactory<Startup> factory) : base(factory)
        {
            _service = _services.GetRequiredService<IJournalService>();
        }

        [Fact]
        public async Task GetOne()
        {
            _user = await SetUpUser();
            var journal = await _service.Add(_user.Id, "Super Duper Journal");
            
            var response = await _client.GetAsync(URL(_user.Id, journal.Id));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll()
        {
            _user = await SetUpUser();
            await _service.Add(_user.Id, "Some Journal");
            await _service.Add(_user.Id, "Some Other Journal");
            await _service.Add(_user.Id, "Squishy Journal");

            var response = await _client.GetAsync(URL(_user.Id));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Add()
        {
            _user = await SetUpUser();
            var journal = new NewJournalDto {Title = "Shiny New Title"};
            
            var response = await _client.PostAsync(URL(_user.Id), journal.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update()
        {
            _user = await SetUpUser();
            const string title = "Look at My New Title";

            var journal = await _service.Add(_user.Id, "Stodgy, Old Title");
            journal.Title = title;

            var response = await _client.PatchAsync(URL(_user.Id, journal.Id), journal.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete()
        {
            _user = await SetUpUser();
            var journal = await _service.Add(_user.Id, "Lookie Here");

            var response = await _client.DeleteAsync(URL(_user.Id, journal.Id));
            response.EnsureSuccessStatusCode();
        }

        private string URL(string userId, long journalId) => $"{URL(userId)}/{journalId}";
        private string URL(string userId) => $"/api/users/{userId}/journals";
    }
}