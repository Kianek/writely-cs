using System;

namespace writely.Exceptions
{
    public class DuplicateJournalException : Exception
    {
        public DuplicateJournalException(string message) : base(message)
        {
        }
    }
}