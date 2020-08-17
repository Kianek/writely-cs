using System;
using writely.Data;

namespace writely.Models
{
    public class Entry : Entity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

        // Navigation properties
        public long JournalId { get; set; }
        public Journal Journal { get; set; }

        public Entry()
        {
        }

        public Entry(string title, string body, long journalId, string userId, string username)
        {
            Title = title;
            Body = body;
            JournalId = journalId;
            UserId = userId;
            Username = username;
        }
    }
}
