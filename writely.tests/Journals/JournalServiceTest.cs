using System;
using System.Threading.Tasks;
using FluentAssertions;
using writely.Data;
using writely.Exceptions;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Journals
{
    public class JournalServiceTest :  IDisposable
    {
        private readonly DatabaseFixture _fixture;
        private ApplicationDbContext _context;
        
        public JournalServiceTest()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async Task Add_UniqueTitle_JournalCreated()
        {
            // Arrange
            const string userId = "UserId";
            const string title = "Super Nifty Journal";

            await PrepareDatabase();
            
            var service = new JournalService(_context);
            
            // Act
            var result = await service.Add(userId, title);
            var addedJournal = await _context.Journals.FindAsync(result.Id);
            
            // Assert
            addedJournal.Title.Should().Be(title);
        }

        [Fact]
        public async void Add_ExistingTitle_ThrowsDuplicateJournalException()
        {
            // Arrange
            const string userId = "UserId";
            const string title = "Super Samey Title";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {UserId = userId, Title = title});
            await _context.SaveChangesAsync();
            
            // Act & assert
            var service = new JournalService(_context);
            service
                .Invoking(js => js.Add(userId, title))
                .Should()
                .Throw<DuplicateJournalException>();
        }
        
        [Fact]
        public async void Update_ExistingJournal_TitleUpdated()
        {
            // Arrange
            const string userId = "UserId";
            const string title = "Old Title";
            const string newTitle = "New Title";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {Id = 1L, UserId = userId, Title = title});
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            var updatedJournal = new JournalDto
            {
                Title = newTitle,
                UserId = userId,
                Id = 1L
            };
            var result = await service.Update(updatedJournal);
            var updJournal = await _context.Journals.FindAsync(updatedJournal.Id);

            // Assert
            result.Title.Should().Be(updJournal.Title);
        }
        
        [Fact]
        public async void Update_JournalNotFound_NotUpdated()
        {
            // Arrange
            await PrepareDatabase();
            
            var service = new JournalService(_context);
            var journal = new JournalDto {Id = 1L};
            
            // Act & assert
            service.Invoking(s => s.Update(journal))
                .Should()
                .Throw<JournalNotFoundException>();
        }

        [Fact]
        public async Task Update_JournalFound_DuplicateTitle_ThrowsDuplicateJournalException()
        {
            // Arrange
            await PrepareDatabase();
            PopulateJournals();

            // Act & assert
            var journal = new JournalDto { Id = 1L, Title = "Journal 1", UserId = "UserId"};
            var service = new JournalService(_context);
            service
                .Invoking(s => s.Update(journal))
                .Should()
                .Throw<DuplicateJournalException>();
        }
        
        [Fact]
        public async void Delete_JournalFound_JournalRemoved()
        {
            // Arrange
            var journal = new Journal { Id = 1L, Title = "Blah", UserId = "UserId"};

            await PrepareDatabase();

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            await service.Delete(journal.Id);
            var result = await _context.Journals.FindAsync(journal.Id);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async void GetAll_UserFound_NoJournalLimit_Success()
        {
            // Arrange
            const string userId = "UserId";

            await PrepareDatabase();

            _context.Journals.AddRange(
                new Journal {Title = "One", UserId = userId},
                new Journal {Title = "Two", UserId = userId},
                new Journal {Title = "Three", UserId = userId}
                );
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            var result = await service.GetAll(userId);

            // Assert
            result.Count.Should().Be(3);
        }
        
        [Fact]
        public async void GetAll_UserFound_JournalLimit_ReturnsLimitedJournals()
        {
            // Arrange
            const string userId = "UserId";
            
            await PrepareDatabase();
            
            PopulateJournals(5);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            var result = await service.GetAll(userId, page: 3);

            // Assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public async void GetAll_UserFound_NoLimit_ReturnsAllJournals()
        {
            // Arrange
            const string userId = "UserId";

            await PrepareDatabase();
            
            PopulateJournals(4);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            var result = await service.GetAll(userId);

            // Assert
            result.Should().HaveCount(4);
        }
        
        [Fact]
        public async void GetById_JournalFound_ReturnsJournalDto()
        {
            // Arrange
            const long id = 1L;
            const string title = "Blah";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {Id = id, Title = title, UserId = "UserId"});
            await _context.SaveChangesAsync();
            
            // Act
            var service = new JournalService(_context);
            var result = await service.GetById(id);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(title);
        }

        [Fact]
        public async Task GetById_JournalNotFound_ThrowsJournalNotFoundException()
        {
            // Arrange
            await PrepareDatabase();
            
            // Act & assert
            var service = new JournalService(_context);
            service
                .Invoking(s => s.GetById(1L))
                .Should()
                .Throw<JournalNotFoundException>();
        }

        private void PopulateJournals(int numOfJournals = 1)
        {
            for (var i = 0; i < numOfJournals; i++)
            {
                _context.Journals.Add(new Journal {Title = $"Journal {i+1}", Id = i+1, UserId = "UserId"});
            }
        }

        private async Task PrepareDatabase()
        {
            _context = _fixture.CreateContext();
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }

        public void Dispose()
        {
            _fixture.Dispose();
            _context.Dispose();
        }
    }
}