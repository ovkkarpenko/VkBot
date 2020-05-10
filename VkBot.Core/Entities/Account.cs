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

        public Account()
        {
        }

        public Account(int id, string token)
        {
            this.id = id;
            this.token = token;
        }

        public Account(int id, string token, string userAgent, string proxy)
        {
            this.id = id;
            this.token = token;
            this.userAgent = userAgent;
            this.proxy = proxy;
        }

        public override string ToString()
        {
            return $"Account(" +
                   $"token='{token}', " +
                   $"userId='{userId}')";
        }
    }
}
