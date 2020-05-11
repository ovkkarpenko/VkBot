using System.Collections.Generic;
using System.Linq;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomGenerateUrl : IGenerateUrl
    {
        private const string Host = "https://api.vk.com/method";
        private readonly string _token;

        public VkcomGenerateUrl(string token)
        {
            _token = token;
        }

        public string Generate(string method, Dictionary<string, string> parameters = null)
        {
            string url = $"{Host}/{method}?" +
                         $"access_token={_token}" +
                         $"&v=5.103" +
                         $"{(parameters != null ? "&" + string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}")) : "")}";


            return url;
        }
    }
}
