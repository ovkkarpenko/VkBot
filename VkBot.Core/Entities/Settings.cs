using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;

namespace VkBot.Core.Entities
{
    public class SettingsModel
    {
        public List<string> Proxies { get; set; }
        public List<string> UserAgents { get; set; }
        public string RucaptchaKey { get; set; }
        public int TimeoutLikes { get; set; }
        public int TimeoutFriend { get; set; }
        public int TimeoutRepost { get; set; }
        public int TimeoutGroup { get; set; }
        public int TimeoutAfterTask { get; set; }
        public ProxyType ProxyType { get; set; }
    }
}
