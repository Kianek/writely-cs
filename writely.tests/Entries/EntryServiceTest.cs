using System;
using System.Threading.Tasks;
using FluentAssertions;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Entries
{
    public class EntryServiceTest : IDisposable
    {
        private DatabaseFixture _fixture;

        public EntryServiceTest()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async Task GetById_EntryFound_ReturnsDto()
        {
            var journal = CreateJournal();
            var entry = new Entry
            {
                Title = "Blah", Body = "Body", Id = 1L, 
                UserId = "UserId", Username = "Skippy", JournalId = journal.Id,
                Journal = journal
            };
            journal.Entries.Add(entry);

            await using var context = _fixture.CreateContext();
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
            
            var service = new EntryService(context);
            var result = await service.GetById(entry.Id);

            result.Should().NotBeNull();
            result.Title.Should().Be(entry.Title);
        }

        [Fact]
        public async Task Add_JournalFound_EntryAdded()
        {
            var journal = CreateJournal();
            var entry = CreateEntry();

            await using var context = _fixture.CreateContext();
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
            
            var service = new EntryService(context);
            var result = await service.Add(journal.Id, new EntryDto(entry));

            result.Should().NotBeNull();
            result.Title.Should().Be(entry.Title);
        }
        
        [Fact]
        public async Task Add_JournalNotFound_ReturnsNull()
        {
            await using var context = _fixture.CreateContext();
            var entry = CreateEntry();
            
            var service = new EntryService(context);
            var result = await service.Add(1L, new EntryDto(entry));

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllByJournal_JournalFound_ReturnsEntries()
        {
            var journal = CreateJournal();
            Entry entry;
            for (var i = 0; i < 5; i++)
            {
                entry = CreateEntry();
                entry.Journal = journal;
                entry.JournalId = journal.Id;
                journal.Entries.Add(entry);
            }
            
            await using var context = _fixture.CreateContext();
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
            
            var service = new EntryService(context);
            var result = await service.GetAllByJournal(journal.Id);
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(5);
        }
        
        [Fact]
        public async Task GetAllByJournal_JournalNotFound_ReturnsNull()
        {
            await using var context = _fixture.CreateContext();
            var service = new EntryService(context);
            var result = await service.GetAllByJournal(1L);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_JournalFound_EntryFoundAndDeleted()
        {
            var journal = CreateJournal();
            var entry = CreateEntry();
            entry.Journal = journal;
            entry.JournalId = journal.Id;
            journal.Entries.Add(entry);

            await using var context = _fixture.CreateContext();
            context.Journals.Add(journal);
            await context.SaveChangesAsync();
            
            var service = new EntryService(context);
            await service.Delete(journal.Id, entry.Id);

            var result = await context.Entries.FindAsync(entry.Id);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_JournalNotFound_ReturnsNull()
        {
            const long entryId = 1L;
            await using var context = _fixture.CreateContext();
            var service = new EntryService(context);

            await service.Delete(1L, entryId);

            var result = await context.Entries.FindAsync(entryId);
            result.Should().BeNull();
        }

        [Fact]
        public async Task MoveEntryToJournal_JournalFound_EntryMoved()
        {
            var destJournal = CreateJournal();
            destJournal.Id = 2L;
            destJournal.Title = "Journal 2";
            
            var srcJournal = CreateJournal();
            var entry = CreateEntry();
            entry.Journal = srcJournal;
            entry.JournalId = srcJournal.Id;
            srcJournal.Entries.Add(entry);

            await using var context = _fixture.CreateContext();
            context.Journals.AddRange(srcJournal, destJournal);
            await context.SaveChangesAsync();
            var service = new EntryService(context);

            var result = await service.MoveEntryToJournal(entry.Id, destJournal.Id);
            result.JournalId.Should().Be(destJournal.Id);
        }

        [Fact]
        public async Task MoveEntryToJournal_JournalNotFound_ReturnsNull()
        {
            await using var context = _fixture.CreateContext();
            var service = new EntryService(context);

            var result = await service.MoveEntryToJournal(1L, 1L);
            result.Should().BeNull();
        }

        public void Dispose()
        {
            _fixture.Dispose();
        }

        private Entry CreateEntry() => new Entry
        {
            UserId = "UserId", Username = "NameyMcNameface",
            Title = "Entry Title", Body = "Skippity bee bop"
        };

        private Journal CreateJournal() => new Journal {UserId = "UserId", Id = 1L, Title = "Journal 1"};
    }
}