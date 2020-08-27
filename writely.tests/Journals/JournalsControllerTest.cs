using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using writely.Controllers;
using Xunit;

namespace writely.tests.Journals
{
    public class JournalsControllerTest
    {
        private JournalsController _controller;
        private ILogger<JournalsController> _logger;

        public JournalsControllerTest()
        {
            _logger = new Mock<ILogger<JournalsController>>().Object;
        }

        [Fact]
        public async Task GetOne_JournalFound_ReturnsOk()
        {
            
        }

        [Fact]
        public async Task GetOne_JournalNotFound_ReturnsBadRequest()
        {
        }

        [Fact]
        public async Task GetAll_UserFound_ReturnsOkWithList()
        {
        }
        
        [Fact]
        public async Task GetAll_JournalsNotFound_ReturnsBadRequest()
        {
        }

        [Fact]
        public async Task Add_UserFound_UniqueTitle_JournalAdded_ReturnsOk()
        {
        }

        [Fact]
        public async Task Add_UserFound_DuplicateTitle_ReturnsBadRequest()
        {
        }

        [Fact]
        public async Task Add_UserNotFound_ReturnsBadRequest()
        {
        }

        [Fact]
        public async Task Update_JournalFound_UpdateSuccessful_ReturnsOk()
        {
        }

        [Fact]
        public async Task Delete_JournalFound_Deleted_ReturnsOk()
        {
        }

        [Fact]
        public async Task Delete_JournalNotFound_ReturnsBadRequest()
        {
        }
    }
}