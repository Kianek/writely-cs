using System;

namespace writely.Exceptions
{
    public class JournalNotFoundException : Exception
    {
        public JournalNotFoundException(string message) : base(message)
        {
        }
    }
}