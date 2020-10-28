using System;

namespace writely.Exceptions
{
    public class MoveEntryException : Exception
    {
        public MoveEntryException(string message) : base(message)
        {
        }
    }
}