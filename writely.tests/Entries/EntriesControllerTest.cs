using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using writely.Controllers;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Entries
{
    public class EntriesControllerTest
    {
        private ILogger<EntriesController> _logger;
        private EntriesController _controller;
        private Mock<IEntryService> _service;

        public EntriesControllerTest()
        {
            _logger = new Mock<ILogger<EntriesController>>().Object;
            _service = new Mock<IEntryService>();
        }

        [Fact]
        public async Task GetAll_JournalFound_ReturnsOk()
        {
            _service.Setup(s =>
                    s.GetAllByJournal(It.IsAny<long>()))
                .ReturnsAsync(new List<EntryDto>());
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.GetAll(1L);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAll_JournalNotFound_ReturnsNotFound()
        {
            _service.Setup(s =>
                    s.GetAllByJournal(It.IsAny<long>()))
                .ReturnsAsync(() => null);
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.GetAll(1L);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetOne_EntryFound_ReturnsOk()
        {
            _service.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(new EntryDto {Title = "Entry"});
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetOne_EntryNotFound_ReturnsNotFound()
        {
            _service.Setup(s =>
                    s.GetById(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.GetOne(1L);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Add_JournalFound_EntryAdded_ReturnsOk()
        {
            var entry = new EntryDto {Title = "Blah"};
            _service.Setup(s =>
                    s.Add(It.IsAny<long>(), It.IsAny<EntryDto>()))
                .ReturnsAsync(entry);
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.Add("UserId", 1L, entry);
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task Add_JournalNotFound_ReturnsBadRequest()
        {
            _service.Setup(s =>
                    s.Add(It.IsAny<long>(), It.IsAny<EntryDto>()))
                .ReturnsAsync(() => null);
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.Add("UserId", 1L, new EntryDto());
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Update_EntryFound_Updated_ReturnsOk()
        {
            var entry = new EntryDto();
            _service.Setup(s =>
                    s.Update(It.IsAny<EntryDto>()))
                .ReturnsAsync(entry);
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.Update(entry);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Update_EntryNotFound_ReturnsBadRequest()
        {
            _service.Setup(s =>
                    s.Update(It.IsAny<EntryDto>()))
                .ReturnsAsync(() => null);
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.Update(new EntryDto());
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_EntryRemoved_ReturnsOk()
        {
            _service.Setup(s =>
                s.Delete(It.IsAny<long>(), It.IsAny<long>()));
            
            _controller = new EntriesController(_logger, _service.Object);

            var result = await _controller.Delete(1L, 1L);
            result.Should().BeOfType<OkResult>();
        }
    }
}