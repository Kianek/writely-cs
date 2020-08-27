using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using writely.Data;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public class JournalService : IJournalService
    {
        private readonly ApplicationDbContext _context;

        public JournalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<JournalDto> Add(string userId, string title)
        {
            var journal = await _context.Journals
                .Where(j => j.UserId == userId)
                .SingleOrDefaultAsync();
            if (journal != null)
            {
                return null;
            }
            
            journal = new Journal {Title = title, UserId = userId};
            _context.Journals.Add(journal);
            await _context.SaveChangesAsync();

            return new JournalDto(journal);
        }

        public async Task<JournalDto> Update(JournalDto updatedJournal)
        {
            var journal = await _context.Journals.FindAsync(updatedJournal.Id);
            if (journal == null)
            {
                return null;
            }
            
            journal.Title = updatedJournal.Title;
            journal.LastModified = DateTimeOffset.Now;
            _context.Journals.Update(journal);
            await _context.SaveChangesAsync();
            
            return new JournalDto(journal);
        }

        public async Task Delete(long id)
        {
            var journal = await _context.Journals
                .Where(j => j.Id == id)
                .Include(j => j.Entries)
                .FirstOrDefaultAsync();

            if (journal != null)
            {
                _context.Journals.Remove(journal);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<JournalDto>> GetAll(string userId, int page = 0)
        {
            var query = _context.Journals
                .Where(j => j.UserId == userId)
                .Include(j => j.Entries)
                .Select(j => new JournalDto(j));
            
            return page > 0 ? await query.Take(page).ToListAsync() : await query.ToListAsync();
        }

        public async Task<JournalDto> GetById(long id)
        {
            return await _context.Journals
                .Where(j => j.Id == id)
                .Include(j => j.Entries)
                .Select(j => new JournalDto(j))
                .SingleOrDefaultAsync();
        }
    }
}