using System;
using System.Collections.Generic;

namespace writely.Models.Dto
{
    public class UserData
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<JournalDto> Journals { get; set; }

        public UserData()
        {
        }

        public UserData(AppUser user)
        {
            Id = user.Id;
            Username = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Journals = new List<JournalDto>();
        }
    }
}
