using System.Collections.Generic;
using System.Threading.Tasks;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IJournalService
    {
        Task<Journal> Add(string userId, string title);
        Task<Journal> Update(JournalDto updatedJournal);
        Task Delete(long id);
        Task<List<Journal>> GetAll(string userId, int limit = 0);
        Task<Journal> GetById(long id);
    }
}
