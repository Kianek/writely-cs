using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using writely.Controllers;
using Xunit;

namespace writely.tests.Entries
{
    public class EntriesControllerTest
    {
        private ILogger<EntriesController> _logger;
        private EntriesController _controller;

        public EntriesControllerTest()
        {
            _logger = new Mock<ILogger<EntriesController>>().Object;
        }
        
        [Fact]
        public async Task GetAll_JournalFound_ReturnsOk() {}
        
        [Fact]
        public async Task GetAll_JournalNotFound_ReturnsBadRequest() {}
        
        [Fact]
        public async Task GetOne_EntryFound_ReturnsOk() {}
        
        [Fact]
        public async Task GetOne_EntryNotFound_ReturnsNotFound() {}
        
        [Fact]
        public async Task Add_JournalFound_EntryAdded_ReturnsOk() {}
        
        [Fact]
        public async Task Add_JournalNotFound_ReturnsBadRequest() {}
        
        [Fact]
        public async Task Update_EntryFound_Updated_ReturnsOk() {}
        
        [Fact]
        public async Task Update_EntryNotFound_ReturnsBadRequest() {}
        
        [Fact]
        public async Task Delete_EntryRemoved_ReturnsOk() {}
    }
}