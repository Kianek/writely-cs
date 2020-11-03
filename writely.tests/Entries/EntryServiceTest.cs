using System;
using System.Threading.Tasks;
using FluentAssertions;
using writely.Data;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.tests.Entries
{
    public class EntryServiceTest : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public EntryServiceTest()
        {
            _context = new DatabaseFixture().CreateContext();
        }

        [Fact]
        public async Task GetById_EntryFound_ReturnsDto()
        {
            // Arrange
            var journal = CreateJournal();
            var entry = new Entry
            {
                Title = "Blah", Body = "Body", Id = 1L, 
                UserId = "UserId", Username = "Skippy", JournalId = journal.Id,
                Journal = journal
            };
            journal.Entries.Add(entry);

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.GetById(entry.Id);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(entry.Title);
        }

        [Fact]
        public async Task Add_JournalFound_EntryAdded()
        {
            // Arrange
            var journal = CreateJournal();
            var entry = CreateEntry();

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.Add(journal.Id, new EntryDto(entry));

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be(entry.Title);
        }
        
        [Fact]
        public async Task Add_JournalNotFound_ReturnsNull()
        {
            // Arrange
            var entry = CreateEntry();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.Add(1L, new EntryDto(entry));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllByJournal_JournalFound_ReturnsEntries()
        {
            // Arrange
            var journal = CreateJournal();
            Entry entry;
            for (var i = 0; i < 5; i++)
            {
                entry = CreateEntry();
                entry.Journal = journal;
                entry.JournalId = journal.Id;
                journal.Entries.Add(entry);
            }
            
            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.GetAllByJournal(journal.Id);
            
            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(5);
        }
        
        [Fact]
        public async Task GetAllByJournal_JournalNotFound_ReturnsNull()
        {
            // Act
            var service = new EntryService(_context);
            var result = await service.GetAllByJournal(1L);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_JournalFound_EntryFoundAndDeleted()
        {
            // Arrange
            var journal = CreateJournal();
            var entry = CreateEntry();
            entry.Journal = journal;
            entry.JournalId = journal.Id;
            journal.Entries.Add(entry);

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            await service.Delete(journal.Id, entry.Id);
            var result = await _context.Entries.FindAsync(entry.Id);
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_JournalNotFound_ReturnsNull()
        {
            // Arrange
            const long entryId = 1L;
            var service = new EntryService(_context);

            // Act
            await service.Delete(1L, entryId);
            var result = await _context.Entries.FindAsync(entryId);
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Update_JournalAndEntryFound_Updated_ReturnsOk()
        {
            // Arrange
            var journal = new Journal
            {
                Id = 1L,
                Title = "My Journal",
                UserId = "UserId"
            };
            journal.Entries.Add(new Entry
            {
                Id = 1L,
                Title = "My Entry",
                Body = "Skippity doo da",
                Journal = journal,
                JournalId = journal.Id,
                Username = "bob.loblaw",
                UserId = journal.UserId
            });
            
            var entry = new EntryDto
            {
                Id = 1L,
                Title = "Shiny New Title",
            };

            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.Update(entry);
            
            // Assert
            result.Title.Should().Be(entry.Title);
        }

        [Fact]
        public async Task Update_JournalNotFound_ReturnsNull()
        {
            // Arrange
            var entry = new EntryDto
            {
                Id = 1L,
                Title = "Blah",
                Body = "Blah dee frickin' blah",
                JournalId = 1L,
                UserId = "UserId"
            };
            
            // Act
            var service = new EntryService(_context);
            var result = await service.Update(entry);
            
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task MoveEntryToJournal_JournalFound_EntryMoved()
        {
            // Arrange
            var destJournal = CreateJournal();
            destJournal.Id = 2L;
            destJournal.Title = "Journal 2";
            
            var srcJournal = CreateJournal();
            var entry = CreateEntry();
            entry.Journal = srcJournal;
            entry.JournalId = srcJournal.Id;
            srcJournal.Entries.Add(entry);

            _context.Journals.AddRange(srcJournal, destJournal);
            await _context.SaveChangesAsync();
            
            // Act
            var service = new EntryService(_context);
            var result = await service.MoveEntryToJournal(entry.Id, destJournal.Id);
            
            // Assert
            result.JournalId.Should().Be(destJournal.Id);
        }

        [Fact]
        public async Task MoveEntryToJournal_JournalNotFound_ReturnsNull()
        {
            // Act
            var service = new EntryService(_context);
            var result = await service.MoveEntryToJournal(1L, 1L);
            
            // Assert
            result.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private Entry CreateEntry() => new Entry
        {
            UserId = "UserId", Username = "NameyMcNameface",
            Title = "Entry Title", Body = "Skippity bee bop"
        };

        private Journal CreateJournal() => new Journal {UserId = "UserId", Id = 1L, Title = "Journal 1"};
    }
}