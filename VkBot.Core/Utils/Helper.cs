using System;
using Leaf.xNet;
using Newtonsoft.Json.Linq;

namespace VkBot.Core.Utils
{
    public class Helper
    {
        public (HttpResponse response, string content, dynamic json) SendRequest(Func<HttpResponse> request)
        {
            HttpResponse response = request();

            string content = $"{response}";
            dynamic json = JArray.Parse($"[{response}]");

            return (response, content, json);
        }
    }
}
