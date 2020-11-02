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
            const string userId = "UserId";
            const string title = "Super Nifty Journal";

            await PrepareDatabase();
            
            var service = new JournalService(_context);
            var result = await service.Add(userId, title);
    
            var addedJournal = await _context.Journals.FindAsync(result.Id);
            addedJournal.Title.Should().Be(title);
        }

        [Fact]
        public async void Add_ExistingTitle_ThrowsDuplicateJournalException()
        {
            const string userId = "UserId";
            const string title = "Super Samey Title";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {UserId = userId, Title = title});
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            service
                .Invoking(js => js.Add(userId, title))
                .Should()
                .Throw<DuplicateJournalException>();
        }
        
        [Fact]
        public async void Update_ExistingJournal_TitleUpdated()
        {
            const string userId = "UserId";
            const string title = "Old Title";
            const string newTitle = "New Title";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {Id = 1L, UserId = userId, Title = title});
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            var updatedJournal = new JournalDto
            {
                Title = newTitle,
                UserId = userId,
                Id = 1L
            };
            var result = await service.Update(updatedJournal);
            var updJournal = await _context.Journals.FindAsync(updatedJournal.Id);

            result.Title.Should().Be(updJournal.Title);
        }
        
        [Fact]
        public async void Update_JournalNotFound_NotUpdated()
        {
            await PrepareDatabase();
            
            var service = new JournalService(_context);
            var journal = new JournalDto {Id = 1L};
            
            service.Invoking(s => s.Update(journal))
                .Should()
                .Throw<JournalNotFoundException>();
        }
        
        [Fact]
        public async void Delete_JournalFound_JournalRemoved()
        {
            var journal = new Journal { Id = 1L, Title = "Blah", UserId = "UserId"};

            await PrepareDatabase();

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            await service.Delete(journal.Id);
            
            var result = await _context.Journals.FindAsync(journal.Id);
            result.Should().BeNull();
        }
        
        [Fact]
        public async void GetAll_UserFound_NoJournalLimit_Success()
        {
            const string userId = "UserId";

            await PrepareDatabase();

            _context.Journals.AddRange(
                new Journal {Title = "One", UserId = userId},
                new Journal {Title = "Two", UserId = userId},
                new Journal {Title = "Three", UserId = userId}
                );
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            var result = await service.GetAll(userId);

            result.Count.Should().Be(3);
        }
        
        [Fact]
        public async void GetAll_UserFound_JournalLimit_ReturnsLimitedJournals()
        {
            const string userId = "UserId";
            
            await PrepareDatabase();
            
            PopulateJournals(userId);
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            var result = await service.GetAll(userId, page: 3);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async void GetAll_UserFound_NoLimit_ReturnsAllJournals()
        {
            const string userId = "UserId";

            await PrepareDatabase();
            
            PopulateJournals(userId);
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            var result = await service.GetAll(userId);

            result.Should().HaveCount(8);
        }
        
        [Fact]
        public async void GetById_JournalFound_ReturnsJournalDto()
        {
            const long id = 1L;
            const string title = "Blah";

            await PrepareDatabase();
            
            _context.Journals.Add(new Journal {Id = id, Title = title, UserId = "UserId"});
            await _context.SaveChangesAsync();
            
            var service = new JournalService(_context);
            var result = await service.GetById(id);

            result.Should().NotBeNull();
            result.Title.Should().Be(title);
        }

        private void PopulateJournals(string userId)
        {
            for (var i = 0; i < 8; i++)
            {
                _context.Journals.Add(new Journal {Title = $"Journal {i+1}", UserId = userId});
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