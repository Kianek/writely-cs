using System;
using System.Collections.Generic;

namespace writely.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModified { get; set; }
        public List<JournalDto> Journals { get; set; } = new List<JournalDto>();

        public UserDto()
        {
        }
    }
}
