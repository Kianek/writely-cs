using System;

namespace writely.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(string message) : base(message)
        {
        }
    }
}