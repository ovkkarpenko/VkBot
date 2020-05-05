using System.Collections.Generic;
using System.Linq;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories
{
    public class Vkcom : INetwork
    {
        private const string Host = "https://api.vk.com/method";
        private readonly string _token;

        private readonly Helper _helper;
        private readonly HttpRequest _request;

        public Vkcom(string token)
        {
            _helper = new Helper();
            _request = new HttpRequest();

            _token = token;
            _request.UserAgentRandomize();
        }

        public AccountInfoModel Auth()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("account.getProfileInfo")));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;

                string firstName = response.first_name;
                string lastName = response.last_name;
                string bdate = response.bdate;
                string city = response.city?.title;
                GenderTypes gender = response.sex == 2 ? GenderTypes.Man : GenderTypes.Woman;

                return new AccountInfoModel
                {
                    FullName = $"{firstName} {lastName}",
                    Birthday = bdate,
                    City = city,
                    Gender = gender
                };
            }

            dynamic error = result.json[0].error;

            return null;
        }

        public bool AddLike(string postId)
        {
            throw new System.NotImplementedException();
        }

        public bool AddRepost(string postId)
        {
            throw new System.NotImplementedException();
        }

        public bool AddFriend(string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool AddGroup(string groupId)
        {
            throw new System.NotImplementedException();
        }

        private string GenerateUrl(string method, Dictionary<string, string> parameters = null)
        {
            string url = $"{Host}/{method}?" +
                         $"access_token={_token}" +
                         $"&v=5.103" +
                         $"{(parameters != null ? "&" + string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}")) : "")}";


            return url;
        }
    }
}
