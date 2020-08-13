using System;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IEntryService
    {
        Entry GetById(long id);
        Entry Add(EntryDto entry);
        Entry Delete(long id);
        Entry MoveEntryToJournal(long entryId, long journalId);
    }
}
