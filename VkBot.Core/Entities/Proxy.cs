using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Core.Entities
{
    public class Proxy
    {
        public string Ip { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool EnableAuth { get; set; }
    }
}
