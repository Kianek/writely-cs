using System;

namespace writely.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException(string message) : base(message)
        {
        }
    }
}