using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;

namespace VkBot.Core.Entities
{
    public class Settings
    {
        public List<string> proxies { get; set; }
        public List<string> userAgents { get; set; }
        public string rucaptchaKey { get; set; }
        public int timeoutLikes { get; set; }
        public int timeoutFriend { get; set; }
        public int timeoutRepost { get; set; }
        public int timeoutGroup { get; set; }
        public int timeoutAfterTask { get; set; }
        public ProxyType proxyType { get; set; }
    }
}
