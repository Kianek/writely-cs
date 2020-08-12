using System;
using System.Collections.Generic;
using writely.Data;

namespace writely.Models
{
    public class Journal : Entity
    {
        public string Title { get; set; }

        // Navigation properties
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public List<Entry> Entries { get; set; } = new List<Entry>();

        public Journal()
        {
        }

        public Journal(string title, List<Entry> entries)
        {
            Title = title;
            Entries = entries;
        }
    }
}
