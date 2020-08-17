using System.Collections.Generic;
using System.Linq;
using writely.Models;
using writely.Models.Dto;

namespace writely.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<JournalDto> MapJournalToDto(this IQueryable<Journal> journals)
        {
            return journals.Select(j => new JournalDto(j));
        }

        public static IQueryable<EntryDto> MapEntryToDto(this IQueryable<Entry> entries)
        {
            return entries.Select(e => new EntryDto(e));
        }
    }
}