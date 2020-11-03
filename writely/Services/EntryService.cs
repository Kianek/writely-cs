using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using writely.Data;
using writely.Exceptions;
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
            if (entry == null)
            {
                throw new EntryNotFoundException($"Entry not found: {id}");
            }
            return new EntryDto(entry);
        }

        public async Task<EntryDto> Add(long journalId, EntryDto entryDto)
        {
            var journal = await _context.Journals.AsTracking().FirstOrDefaultAsync(j => j.Id == journalId);
            if (journal == null)
            {
                throw new JournalNotFoundException($"Journal not found: {journalId}");
            }
            
            var entry = new Entry
            {
                Title = entryDto.Title, Body = entryDto.Body,
                Journal = journal, JournalId = journalId,
                UserId = journal.UserId
            };
            
            journal.Entries.Add(entry);
            _context.Journals.Update(journal);
            await _context.SaveChangesAsync();
            
            return new EntryDto(entry);
        }

        public async Task<List<EntryDto>> GetAllByJournal(long journalId)
        {
            var journal = await _context.Journals
                .Where(j => j.Id == journalId)
                .Include(j => j.Entries)
                .FirstOrDefaultAsync();

            return journal?.Entries.Select(e => new EntryDto(e)).ToList();
        }

        public async Task Delete(long journalId, long entryId)
        {
            var journal = await _context.Journals
                .AsTracking()
                .Where(j => j.Id == journalId)
                .Include(j => j.Entries)
                .FirstOrDefaultAsync();
            if (journal == null)
            {
                throw new JournalNotFoundException($"Journal not found: {journalId}");
            }
            
            journal.Entries = journal.Entries.Where(e => e.Id != entryId).ToList();
            
            _context.Update(journal);
            await _context.SaveChangesAsync();
        }

        public async Task<EntryDto> Update(EntryDto entryDto)
        {
            var existingEntry = await _context.Entries.FindAsync(entryDto.Id);
            if (existingEntry == null)
            {
                // TODO: throw new EntryNotFoundException
                return null;
            }

            bool didUpdate = false;
            if (ShouldUpdate(existingEntry.Title, entryDto.Title))
            {
                existingEntry.Title = entryDto.Title;
                didUpdate = true;
            }

            if (ShouldUpdate(existingEntry.Body, entryDto.Body))
            {
                existingEntry.Body = entryDto.Body;
                didUpdate = true;
            }

            if (didUpdate)
            {
                existingEntry.LastModified = DateTime.Now;
                _context.UpdateRange(existingEntry);
                await _context.SaveChangesAsync();
            }

            return new EntryDto(existingEntry);

            bool ShouldUpdate(string originalValue, string newValue)
                => !string.IsNullOrEmpty(newValue) && originalValue != newValue;
        }

        public async Task<EntryDto> MoveEntryToJournal(long entryId, long destinationJournalId)
        {
            var destinationJournal = await _context.Journals.FindAsync(destinationJournalId);
            var entry = await _context.Entries.FindAsync(entryId);
            var sourceJournal = await _context.Journals.FindAsync(entry?.JournalId);
            
            var values = new List<object> { destinationJournal, sourceJournal, entry};
            if (values.Any(val => val == null))
            {
                // TODO: throw MoveEntryException
                return null;
            }

            // Link entry to destination journal
            entry.JournalId = destinationJournalId;
            entry.Journal = destinationJournal;
            entry.LastModified = DateTime.Now;
            
            destinationJournal.Entries.Add(entry);
            destinationJournal.LastModified = DateTime.Now;
            
            // Remove entry from source journal
            sourceJournal.Entries = sourceJournal.Entries.FindAll(e => e.Id != entryId);
            sourceJournal.LastModified = DateTime.Now;
            
            _context.UpdateRange(entry, destinationJournal, sourceJournal);
            await _context.SaveChangesAsync();
            
            return new EntryDto(entry);
        }
    }
}