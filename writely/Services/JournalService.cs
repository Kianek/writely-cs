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
            throw new System.NotImplementedException();
        }

        public async Task Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<JournalDto>> GetAll(string userId, int limit = 0)
        {
            throw new System.NotImplementedException();
        }

        public async Task<JournalDto> GetById(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}