using System.Collections.Generic;
using System.Linq;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomGenerateUrl : IGenerateUrl
    {
        private readonly string _host;
        private readonly string _token;

        public VkcomGenerateUrl(string host, string token)
        {
            _host = host;
            _token = token;
        }

        public string Generate(string method, Dictionary<string, string> parameters = null)
        {
            string url = $"{_host}/{method}?" +
                         $"access_token={_token}" +
                         $"&v=5.103" +
                         $"{(parameters != null ? "&" + string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}")) : "")}";


            return url;
        }
    }
}
