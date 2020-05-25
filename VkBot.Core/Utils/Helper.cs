using System;
using System.Collections.Generic;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using VkBot.Core.Types;
using log4net;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace VkBot.Core.Utils
{
    public class Helper
    {
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public (HttpResponse response, string content, dynamic json, HttpException httpException) SendRequest(
            Func<HttpResponse> request)
        {
            try
            {
                HttpResponse response = request();

                string content = $"{response}";
                if (string.IsNullOrEmpty(content))
                {
                    return (response, "", "", null);
                }

                dynamic json = JArray.Parse($"[{response}]")[0];
                return (response, content, json, null);
            }
            catch (HttpException e)
            {
                return (null, null, null, e);
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

        public string ParametersToString(Dictionary<string, string> parameters)
        {
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> keyValues in parameters)
            {
                dictionaryString += keyValues.Key + "=" + keyValues.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }

        public (string ownerId, string itemId) ParseUrlIntoOwnerAndItem(string url, ObjectType objectType)
        {
            string ownerId = "";
            string itemId = "";

            if (url.IndexOf("?z=") != -1)
            {
                ownerId = url.Substring($"?z={GetObjectTypeName(objectType)}", "_");
                itemId = url.SubstringLast("%2F", "_");
            }
            else
            {
                ownerId = url.Substring(GetObjectTypeName(objectType), "_");
                itemId = url.Substring(url.LastIndexOf("_") + 1);
            }

            return (ownerId, itemId);
        }

        public string ParseObjectFromUrl(string url)
        {
            return url.IndexOf("?z=") != -1
                ? url.Substring("?z=", "%2F")
                : url.Substring(url.LastIndexOf("/") + 1);
        }

        public string ParseUsernameFromUrl(string url)
        {
            return url.LastIndexOf("/") != -1 ? url.Substring(url.LastIndexOf("/") + 1) : "";
        }

        public ProxyClient ParseProxy(string proxy, ProxyType proxyType)
        {
            if (proxyType == ProxyType.HTTP)
            {
                return HttpProxyClient.Parse(proxy);
            }
            else if (proxyType == ProxyType.Socks4)
            {
                return Socks4ProxyClient.Parse(proxy);
            }
            else if (proxyType == ProxyType.Socks5)
            {
                return Socks5ProxyClient.Parse(proxy);
            }

            return null;
        }
    }
}
