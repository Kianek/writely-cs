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
        
        public JournalsControllerTest(WebAppFactory<Startup> factory) : base(factory)
        {
            _user = SetUpUser().Result;
            _service = _services.GetRequiredService<IJournalService>();
        }
        
        [Fact]
        public async Task GetOne()
        {}
        
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