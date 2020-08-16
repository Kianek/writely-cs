using System.Collections.Generic;
using System.Net.Mime;
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

        public Journal Add(string userId, string title)
        {
            throw new System.NotImplementedException();
        }

        public Journal Update(JournalDto updatedJournal)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new System.NotImplementedException();
        }

        public List<Journal> GetAll(string userId, int limit = 0)
        {
            throw new System.NotImplementedException();
        }

        public Journal GetById(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}