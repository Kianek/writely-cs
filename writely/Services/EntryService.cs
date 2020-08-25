using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using writely.Data;
using writely.Extensions;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public class EntryService : IEntryService
    {
        private readonly ApplicationDbContext _context;

        public EntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EntryDto> GetById(long id)
        {
            var entry = await _context.Entries.FindAsync(id);
            return new EntryDto(entry);
        }

        public async Task<EntryDto> Add(long journalId, EntryDto entryDto)
        {
            var journal = await _context.Journals.FindAsync(journalId);
            if (journal == null)
            {
                return null;
            }
            
            var entry = new Entry
            {
                Title = entryDto.Title, Body = entryDto.Body,
                Journal = journal, JournalId = journalId,
                UserId = journal.UserId, Username = entryDto.Username
            };
            
            journal.Entries.Add(entry);
            _context.Update(journal);
            await _context.SaveChangesAsync();
            
            return new EntryDto(entry);
        }

        public async Task<List<EntryDto>> GetAllByJournal(long journalId)
        {
            var journal = await _context.Journals
                .Where(j => j.Id == journalId)
                .Include(j => j.Entries)
                .FirstOrDefaultAsync();
            if (journal == null)
            {
                return null;
            }

            return journal.Entries.Select(e => new EntryDto(e)).ToList();
        }

        public async Task Delete(long journalId, long id)
        {
            var journal = await _context.Journals
                .AsTracking()
                .Where(j => j.Id == journalId)
                .Include(j => j.Entries)
                .FirstOrDefaultAsync();
            if (journal == null)
            {
                return;
            }
            
            journal.Entries = journal.Entries.Where(e => e.Id != id).ToList();
            
            _context.Update(journal);
            await _context.SaveChangesAsync();
        }

        public async Task<EntryDto> MoveEntryToJournal(long entryId, long destinationJournalId)
        {
            var destinationJournal = await _context.Journals.FindAsync(destinationJournalId);
            var entry = await _context.Entries.FindAsync(entryId);
            var sourceJournal = await _context.Journals.FindAsync(entry?.JournalId);
            
            var values = new List<object> { destinationJournal, sourceJournal, entry};
            if (values.Any(val => val == null))
            {
                return null;
            }

            // Link entry to destination journal
            entry.JournalId = destinationJournalId;
            entry.Journal = destinationJournal;
            entry.LastModified = DateTimeOffset.Now;
            
            destinationJournal.Entries.Add(entry);
            destinationJournal.LastModified = DateTimeOffset.Now;
            
            // Remove entry from source journal
            sourceJournal.Entries = sourceJournal.Entries.FindAll(e => e.Id != entryId);
            sourceJournal.LastModified = DateTimeOffset.Now;
            
            _context.UpdateRange(entry, destinationJournal, sourceJournal);
            await _context.SaveChangesAsync();
            
            return new EntryDto(entry);
        }
    }
}