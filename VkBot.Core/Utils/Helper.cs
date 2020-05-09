using System;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using VkBot.Core.Types;

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

        public string GetObjectTypeName(ObjectType objectType)
        {
            if (objectType == ObjectType.POST)
            {
                return "wall";
            }

            return Enum.GetName(typeof(ObjectType), objectType).ToLower();
        }
    }
}
