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

        public Task<Journal> Add(string userId, string title)
        {
            throw new System.NotImplementedException();
        }

        public Task<Journal> Update(JournalDto updatedJournal)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Journal>> GetAll(string userId, int limit = 0)
        {
            throw new System.NotImplementedException();
        }

        public Task<Journal> GetById(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}