using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Core.Exceptions
{
    public class NeedValidationException : Exception
    {
        public NeedValidationException()
        {
        }

        public NeedValidationException(string message) : base(message)
        {
        }
    }
}
