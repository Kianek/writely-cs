using System;
using System.Collections.Generic;
using System.Linq;

namespace writely.Models.Dto
{
    public class JournalDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<EntryDto> Entries { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public JournalDto()
        {
        }

        public JournalDto(Journal journal)
        {
            Id = journal.Id;
            Title = journal.Title;
            UserId = journal.UserId;
            CreatedAt = journal.CreatedAt;
            LastModified = journal.LastModified;
            Entries = journal.Entries?.Select(e => new EntryDto(e)).ToList();
        }
    }
}
