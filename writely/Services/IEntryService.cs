using System.Collections.Generic;
using System.Threading.Tasks;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IEntryService
    {
        Task<EntryDto> GetById(long id);
        Task<List<EntryDto>> GetAllByJournal(long journalId);
        Task<EntryDto> Add(long journalId, EntryDto entry);
        Task<EntryDto> Update(EntryDto entry);
        Task Delete(long journalId, long id);
        Task<EntryDto> MoveEntryToJournal(long entryId, long journalId);
    }
}
