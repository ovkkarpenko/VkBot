using System;
using System.Collections.Generic;
using System.Linq;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories
{
    public class Vkcom
    {
        private const string Host = "https://api.vk.com/method";
        private readonly string _token;

        private readonly Helper _helper;
        private readonly Rucaptcha _rucaptcha;
        private readonly HttpRequest _request;

        private Account Account { get; set; }

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
                string country = user.country?.title;
                Gender gender = user.sex == 2 ? Gender.MAN : Gender.WOMAN;

                Account = new Account
                {
                    userId = id,
                    fullName = $"{firstName} {lastName}",
                    //Birthday = bdate,
                    country = country,
                    gender = gender
                };

                return true;
            }

            dynamic error = result.json[0].error;
            return false;
        }

        public void AddLike(string ownerId, string itemId, ObjectType objectType)
        {
            var parameters = new Dictionary<string, string>
            {
                {"type", Enum.GetName(typeof(ObjectType), objectType).ToLower()},
                {"owner_id", ownerId},
                {"item_id", itemId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("likes.add", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
            }

            dynamic error = result.json[0].error;
            string errorCode = error?.error_code;

            //Captcha
            if (errorCode == "14")
            {
                string captchaSid = error.captcha_sid;
                dynamic json = ParseCaptcha("likes.add", parameters, captchaSid);

                if (json != null)
                {
                    dynamic response = json[0].response;
                }
            }
        }

        public bool AddRepost(string @object)
        {
            var parameters = new Dictionary<string, string>
            {
                {"object", @object}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("wall.repost", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
                return response.success == 1;
            }

            dynamic error = result.json[0].error;
            string errorCode = error.error_code;

            //Captcha
            if (errorCode == "14")
            {
                string captchaSid = error.captcha_sid;
                dynamic json = ParseCaptcha("wall.repost", parameters, captchaSid);

                if (json != null)
                {
                    dynamic response = json[0].response;
                    return response.success == 1;
                }
            }

            return false;
        }

        public void AddFriend(string userId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_id", userId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("friends.add", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
                return;
            }

            dynamic error = result.json[0].error;
            string errorCode = error.error_code;

            //Captcha
            if (errorCode == "14")
            {
                string captchaSid = error.captcha_sid;
                ParseCaptcha("friends.add", parameters, captchaSid);
            }
        }

        public void JoinGroup(string groupId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_id", groupId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("groups.join", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
            }

            dynamic error = result.json[0].error;
            string errorCode = error?.error_code;

            //Captcha
            if (errorCode == "14")
            {
                string captchaSid = error.captcha_sid;
                dynamic json = ParseCaptcha("groups.join", parameters, captchaSid);

                if (json != null)
                {
                    dynamic response = json[0].response;
                }
            }
        }

        public bool IsLiked(string ownerId, string itemId, ObjectType objectType)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_id", Account.userId},
                {"type", Enum.GetName(typeof(ObjectType), objectType).ToLower()},
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

        public bool IsMember(string groupdId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_id", groupdId},
                {"user_id", Account.userId},
                {"extended", "1"}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("groups.isMember", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response;
                return response.member == 1;
            }

            dynamic error = result.json[0].error;
            return false;
        }

        public bool IsFriend(string userId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_ids", userId}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("friends.areFriends", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response[0];
                return response.friend_status == 1;
            }

            dynamic error = result.json[0].error;
            return false;
        }

        public string GetUserIdByUsername(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_ids", username}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("users.get", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response[0];
                return response.id;
            }

            dynamic error = result.json[0].error;
            return null;
        }

        public string GetGroupIdByUsername(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_ids", username}
            };

            var result = _helper.SendRequest(() => _request.Get(GenerateUrl("groups.getById", parameters)));

            if (result.json[0].response != null)
            {
                dynamic response = result.json[0].response[0];
                return response.id;
            }

            dynamic error = result.json[0].error;
            return null;
        }

        private dynamic ParseCaptcha(string apiMethod, Dictionary<string, string> parameters, string captchaSid)
        {
            int triesRecognize = 0;

            while (triesRecognize++ < 5)
            {
                string captchaImageUrl = $"https://vk.com/captcha.php?sid={captchaSid}&s=1";

                HttpResponse file = _request.Get(captchaImageUrl);
                byte[] bytes = file.ToBytes();

                string capthaKey = _rucaptcha.ImageCaptcha(bytes);

                if (capthaKey != null)
                {
                    var copyParameters = new Dictionary<string, string>(parameters)
                    {
                        {"captcha_sid", captchaSid},
                        {"captcha_key", capthaKey}
                    };

                    var result = _helper.SendRequest(() => _request.Get(GenerateUrl(apiMethod, copyParameters)));

                    if (result.json[0].response != null)
                    {
                        return result.json[0].response;
                    }

                    dynamic error = result.json[0].error;
                    string errorCode = error.error_code;

                    if (errorCode != "14")
                    {
                        return error;
                    }
                }
            }

            return null;
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
