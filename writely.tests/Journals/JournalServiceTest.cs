using System;
using System.Threading.Tasks;
using FluentAssertions;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Journals
{
    public class JournalServiceTest :  IDisposable
    {
        private readonly DatabaseFixture _fixture;
        
        public JournalServiceTest()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async Task Add_UniqueTitle_JournalCreated()
        {
            const string userId = "UserId";
            const string title = "Super Nifty Journal";

            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            
            var service = new JournalService(context);
            var result = await service.Add(userId, title);
    
            var addedJournal = await context.Journals.FindAsync(result.Id);
            addedJournal.Title.Should().Be(title);
        }

        [Fact]
        public async void Add_ExistingTitle_JournalNotAdded()
        {
            const string userId = "UserId";
            const string title = "Super Samey Title";
            
            await using var context = _fixture.CreateContext();
            context.Journals.Add(new Journal {UserId = userId, Title = title});
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var result = await service.Add(userId, title);

            result.Should().BeNull();
        }
        
        [Fact]
        public async void Update_ExistingJournal_TitleUpdated()
        {
            const string userId = "UserId";
            const string title = "Old Title";
            const string newTitle = "New Title";
            
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Journals.Add(new Journal {Id = 1L, UserId = userId, Title = title});
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var updatedJournal = new JournalDto
            {
                Title = newTitle,
                UserId = userId,
                Id = 1L
            };
            var result = await service.Update(updatedJournal);
            var updJournal = await context.Journals.FindAsync(updatedJournal.Id);

            result.Title.Should().Be(updJournal.Title);
        }
        
        [Fact]
        public async void Update_JournalNotFound_NotUpdated()
        {
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            var service = new JournalService(context);
            var journal = new JournalDto {Id = 1L};
            
            var result = await service.Update(journal);
            result.Should().BeNull();
        }
        
        [Fact]
        public async void Delete_JournalFound_JournalRemoved()
        {
            var journal = new Journal { Id = 1L, Title = "Blah", UserId = "UserId"};

            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            await service.Delete(journal.Id);
            
            var result = await context.Journals.FindAsync(journal.Id);
            result.Should().BeNull();
        }
        
        [Fact]
        public async void GetAll_UserFound_NoJournalLimit_Success()
        {
            const string userId = "UserId";

            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Journals.AddRange(
                new Journal {Title = "One", UserId = userId},
                new Journal {Title = "Two", UserId = userId},
                new Journal {Title = "Three", UserId = userId}
                );
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var result = await service.GetAll(userId);

            result.Count.Should().Be(3);
        }
        
        [Fact]
        public async void GetAll_UserFound_JournalLimit_ReturnsLimitedJournals()
        {
            const string userId = "UserId";
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            for (var i = 0; i < 8; i++)
            {
                context.Journals.Add(new Journal {Title = $"Journal {i+1}", UserId = userId});
            }
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var result = await service.GetAll(userId, limit: 3);

            result.Should().HaveCount(3);
        }

        [Fact]
        public async void GetAll_UserFound_NoLimit_ReturnsAllJournals()
        {
            const string userId = "UserId";
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            for (var i = 0; i < 8; i++)
            {
                context.Journals.Add(new Journal {Title = $"Journal {i+1}", UserId = userId});
            }
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var result = await service.GetAll(userId);

            result.Should().HaveCount(8);
        }
        
        [Fact]
        public async void GetById_JournalFound_ReturnsJournalDto()
        {
            const long id = 1L;
            const string title = "Blah";
            await using var context = _fixture.CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            context.Journals.Add(new Journal {Id = id, Title = title, UserId = "UserId"});
            await context.SaveChangesAsync();
            
            var service = new JournalService(context);
            var result = await service.GetById(id);

            result.Should().NotBeNull();
            result.Title.Should().Be(title);
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }
    }
}