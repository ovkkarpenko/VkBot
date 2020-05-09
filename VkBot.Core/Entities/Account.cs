using System;
using VkBot.Core.Types;

namespace VkBot.Core.Entities
{
    public class Account
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string token { get; set; }
        public string fullName { get; set; }
        public string country { get; set; }
        public string userAgent { get; set; }
        public string proxy { get; set; }
        public DateTime birthday { get; set; }
        public Gender gender { get; set; }
        public AccountStatus status { get; set; }
    }
}
