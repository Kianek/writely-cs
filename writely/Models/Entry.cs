using System;
using writely.Data;

namespace writely.Models
{
    public class Entry : Entity
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        // Navigation properties
        public long JournalId { get; set; }
        public Journal Journal { get; set; }

        public Entry()
        {
        }

        public Entry(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
