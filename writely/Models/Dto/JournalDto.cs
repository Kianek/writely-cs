using System;
using System.Collections.Generic;

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
    }
}
