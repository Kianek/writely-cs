using System;
namespace writely.Models.Dto
{
    public class EntryDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public long JournalId { get; set; }
        public string UserId { get; set; }

        public EntryDto()
        {
        }

        public EntryDto(Entry entry)
        {
            Id = entry.Id;
            Title = entry.Title;
            Body = entry.Body;
            CreatedAt = entry.CreatedAt;
            LastModified = entry.LastModified;
            JournalId = entry.JournalId;
            UserId = entry.UserId;
        }
    }
}
