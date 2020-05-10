using System;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using VkBot.Core.Types;

namespace VkBot.Core.Utils
{
    public class Helper
    {
        public (HttpResponse response, string content, dynamic json, HttpException httpException) SendRequest(
            Func<HttpResponse> request)
        {
            try
            {
                HttpResponse response = request();

                string content = $"{response}";
                dynamic json = JArray.Parse($"[{response}]")[0];

                return (response, content, json, null);
            }
            catch (HttpException e)
            {
                return (null, null, null, e);
                ;
            }
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
