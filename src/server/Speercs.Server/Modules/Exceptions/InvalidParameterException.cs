using System;

namespace Speercs.Server.Modules.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string message) : base(message)
        {
        }
    }
}