using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
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
            throw new System.NotImplementedException();
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