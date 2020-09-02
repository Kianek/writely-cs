using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using writely.Models;
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
            _user = SetUpUser().Result;
            _baseUrl = $"/api/users/{_user.Id}/journals";
            _service = _services.GetRequiredService<IJournalService>();
        }

        [Fact]
        public async Task GetOne()
        {
            var journal = await _service.Add(_user.Id, "Super Duper Journal");
            var url = $"{_baseUrl}/{journal.Id}";
            
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        
        [Fact]
        public async Task GetAll()
        {}

        [Fact]
        public async Task Add()
        {}
        
        [Fact]
        public async Task Update()
        {}

        [Fact]
        public async Task Delete()
        {}
    }
}