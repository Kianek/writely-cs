using System.Threading.Tasks;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.integration_tests.Journals
{
    public class JournalsControllerTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
    {
        private IJournalService _service;
        
        public JournalsControllerTest(WebAppFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetOne()
        {
            await ArrangeTest();
            var journal = await _service.Add(_user.Id, "Super Duper Journal");
            
            var response = await _client.GetAsync(URL(_user.Id, journal.Id));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll()
        {
            await ArrangeTest();
            await _service.Add(_user.Id, "Some Journal");
            await _service.Add(_user.Id, "Some Other Journal");
            await _service.Add(_user.Id, "Squishy Journal");

            var response = await _client.GetAsync(URL(_user.Id));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Add()
        {
            await ArrangeTest();
            var journal = new NewJournalDto {Title = "Shiny New Title"};
            
            var response = await _client.PostAsync(URL(_user.Id), journal.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update()
        {
            await ArrangeTest();
            const string title = "Look at My New Title";

            var journal = await _service.Add(_user.Id, "Stodgy, Old Title");
            journal.Title = title;

            var response = await _client.PatchAsync(URL(_user.Id, journal.Id), journal.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete()
        {
            await ArrangeTest();
            var journal = await _service.Add(_user.Id, "Lookie Here");

            var response = await _client.DeleteAsync(URL(_user.Id, journal.Id));
            response.EnsureSuccessStatusCode();
        }

        protected override async Task ArrangeTest()
        {
            await base.ArrangeTest();
            _service = GetService<IJournalService>();
        }

        private string URL(string userId, long journalId) => $"{URL(userId)}/{journalId}";
        private string URL(string userId) => $"/api/users/{userId}/journals";
    }
}