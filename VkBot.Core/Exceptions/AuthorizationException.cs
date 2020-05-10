using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Core.Exceptions
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string message) : base(message)
        {
        }
    }
}
