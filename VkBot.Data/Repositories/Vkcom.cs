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
        private readonly Rucaptcha _rucaptcha;
        private readonly HttpRequest _request;

        public AccountInfoModel AccountInfo { get; set; }

        public Vkcom(string token, string rucaptchaKey)
        {
            _helper = new Helper();
            _rucaptcha = new Rucaptcha(rucaptchaKey);
            _request = new HttpRequest();

            _token = token;
            _request.UserAgentRandomize();
        }

        ~Vkcom()
        {
            _request.Dispose();
        }

        public bool Auth()
        {
            var parameters = new Dictionary<string, string>
            {
                {"fields", "nickname, screen_name, sex, bdate, city, country, has_mobile, online, counters"}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("users.get", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
                dynamic user = response[0];

                string id = user.id;
                string firstName = user.first_name;
                string lastName = user.last_name;
                string bdate = user.bdate;
                string city = user.city?.title;
                GenderTypes gender = user.sex == 2 ? GenderTypes.Man : GenderTypes.Woman;

                AccountInfo = new AccountInfoModel
                {
                    UserId = id,
                    FullName = $"{firstName} {lastName}",
                    Birthday = bdate,
                    City = city,
                    Gender = gender
                };

                return true;
            }

            dynamic error = result.json[0].error;
            return false;
        }

        public void AddLike(string ownerId, string itemId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"type", "post"},
                {"owner_id", ownerId},
                {"item_id", itemId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("likes.add", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
            }

            dynamic error = result.json[0].error;
        }

        public bool IsLiked(string ownerId, string itemId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_id", AccountInfo.UserId},
                {"type", "post"},
                {"owner_id", ownerId},
                {"item_id", itemId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("likes.isLiked", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
                return response.liked == 1;
            }

            dynamic error = result.json[0].error;
            return false;
        }

        public bool AddRepost(string ownerId, string itemId)
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

        public string ParseCaptcha(string captchaUrl)
        {
            HttpResponse response = _request.Get(captchaUrl);
            byte[] bytes = response.ToBytes();

            return _rucaptcha.ImageCaptcha(bytes);
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
