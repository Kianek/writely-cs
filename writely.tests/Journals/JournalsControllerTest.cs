using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using writely.Controllers;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
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
            var journal = new Journal
            {
                Id = 1L,
                Title = "My Journal",
                UserId = "UserId"
            };

            var mockService = new Mock<IJournalService>();
            mockService.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(new JournalDto(journal));
            
            _controller = new JournalsController(mockService.Object, _logger);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetOne_JournalNotFound_ReturnsBadRequest()
        {
            var mockService = new Mock<IJournalService>();
            mockService.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(() => null);
            
            _controller = new JournalsController(mockService.Object, _logger);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<BadRequestResult>();
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