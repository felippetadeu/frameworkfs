using System;

namespace Framework.CustomException
{
    public class BrokenRulesException : Exception
    {
        public BrokenRulesException() : base()
        {

        }

        public BrokenRulesException(string message) : base(message)
        {
        }

        public BrokenRulesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
