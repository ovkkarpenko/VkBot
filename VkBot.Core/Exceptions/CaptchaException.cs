using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Core.Exceptions
{
    public class CaptchaException : Exception
    {
        public CaptchaException()
        {
        }

        public CaptchaException(string message) : base(message)
        {
        }
    }
}
