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

        public EntryDto()
        {
        }
    }
}
