using System.Collections.Generic;
using System.Threading.Tasks;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IJournalService
    {
        Task<JournalDto> Add(string userId, string title);
        Task<JournalDto> Update(JournalDto updatedJournal);
        Task Delete(long id);
        Task<List<JournalDto>> GetAll(string userId, int limit = 0);
        Task<JournalDto> GetById(long id);
    }
}
