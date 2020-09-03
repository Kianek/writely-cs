using System.Collections.Generic;
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
        private Mock<IJournalService> _mockService;

        public JournalsControllerTest()
        {
            _logger = new Mock<ILogger<JournalsController>>().Object;
            _mockService = new Mock<IJournalService>();
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

            _mockService.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(new JournalDto(journal));
            
            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetOne_JournalNotFound_ReturnsBadRequest()
        {
            _mockService.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(() => null);
            
            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAll_UserFound_ReturnsOkWithList()
        {
            const string userId = "UserId";
            var journals = new List<JournalDto>();
            for (var i = 0; i < 5; i++)
            {
                journals.Add(new JournalDto { Title = $"Journal {i + 1}", UserId = userId });
            }

            _mockService.Setup(s =>
                s.GetAll(It.IsAny<string>(), 5))
                .ReturnsAsync(journals);

            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.GetAll(userId, page: 5);
            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task GetAll_JournalsNotFound_ReturnsBadRequest()
        {
            _mockService.Setup(s =>
                    s.GetAll(It.IsAny<string>(), 5))
                .ReturnsAsync(() => null);

            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.GetAll("UserId", 5);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Add_UserFound_UniqueTitle_JournalAdded_ReturnsOk()
        {
            const string userId = "UserId";
            const string title = "Squeaky New Journal";
            var journalDto = new JournalDto
            {
                Title = title,
                UserId = userId
            };
            _mockService.Setup(s =>
                    s.Add(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(journalDto);

            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.Add(userId, new NewJournalDto {Title = title});
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Add_UserFound_DuplicateTitle_ReturnsBadRequest()
        {
            _mockService.Setup(s =>
                    s.Add(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.Add("UserId", new NewJournalDto {Title = "My Title"});
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Update_JournalFound_UpdateSuccessful_ReturnsOk()
        {
            var journal = new JournalDto
            {
                Id = 1L,
                Title = "Some Journal",
                UserId = "UserId"
            };
            _mockService.Setup(s =>
                    s.Update(It.IsAny<JournalDto>()))
                .ReturnsAsync(journal);
            
            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.Update(journal.Id, journal);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_JournalFound_Deleted_ReturnsOk()
        {
            _mockService.Setup(s =>
                s.Delete(It.IsAny<long>()));
            
            _controller = new JournalsController(_mockService.Object, _logger);

            var result = await _controller.Delete(1L);
            result.Should().BeOfType<OkResult>();
        }
    }
}