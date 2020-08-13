using System.Collections.Generic;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IJournalService
    {
        Journal Add(string title);
        Journal Update(JournalDto updatedJournal);
        void Delete(long id);
        List<Journal> GetAll(int limit = 0);
        Journal GetById(long id);
    }
}
