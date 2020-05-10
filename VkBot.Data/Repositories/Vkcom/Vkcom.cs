using System;
using System.Collections.Generic;
using System.Linq;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class Vkcom
    {
        private readonly Helper _helper;
        private readonly HttpRequest _request;

        private readonly IGenerateUrl _generateUrl;
        private readonly IHandleErrors _handleErrors;

        public Account Account { get; private set; }

        public Vkcom(Account account, string rucaptchaKey)
        {
            _helper = new Helper();
            _request = new HttpRequest();
            _generateUrl = new VkcomGenerateUrl("https://api.vk.com/method", account.token);

            Account = account;
            _request.UserAgentRandomize();

            _handleErrors = new VkcomHandleErrors(_helper, _request, _generateUrl, new VkcomParseCaptcha(rucaptchaKey));
        }

        ~Vkcom()
        {
            _request.Dispose();
        }

        public Account GetCurrentUser()
        {
            var parameters = new Dictionary<string, string>
            {
                {"fields", "nickname, screen_name, sex, bdate, city, country, has_mobile, online, counters"}
            };

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("users.get", parameters)));
            if (result.json.response != null)
            {
                dynamic response = result.json.response;
                dynamic user = response[0];

                string userId = user.id;
                string firstName = user.first_name;
                string lastName = user.last_name;
                string[] bdate = $"{user.bdate}".Split('.');
                string country = user.country?.title;
                Gender gender = user.sex == 2 ? Gender.MAN : Gender.WOMAN;

                if (bdate?.Length == 3)
                {
                    Account.birthday = new DateTime(Convert.ToInt32(bdate[2]), Convert.ToInt32(bdate[1]),
                        Convert.ToInt32(bdate[0]));
                }

                Account.userId = userId;
                Account.fullName = $"{firstName} {lastName}";
                Account.country = country;
                Account.gender = gender;
                Account.status = AccountStatus.VALID;

                return Account;
            }

            dynamic error = result.json.error;
            _handleErrors.HandleErrors("users.get", error, parameters, null);
            return null;
        }

        public bool AddLike(string ownerId, string itemId, ObjectType objectType)
        {
            var parameters = new Dictionary<string, string>
            {
                {"type", Enum.GetName(typeof(ObjectType), objectType).ToLower()},
                {"owner_id", ownerId},
                {"item_id", itemId}
            };

            dynamic SuccessAction(dynamic response) => response.likes != null;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("likes.add", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("likes.add", error, parameters, (Func<dynamic, dynamic>) SuccessAction);
        }

        public bool AddRepost(string @object)
        {
            var parameters = new Dictionary<string, string>
            {
                {"object", @object}
            };

            dynamic SuccessAction(dynamic response) => response.success == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("wall.repost", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("wall.repost", error, parameters, (Func<dynamic, dynamic>) SuccessAction);
        }

        public bool AddFriend(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_id", GetUserIdByUsername(username)}
            };

            dynamic SuccessAction(dynamic response) => response == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("friends.add", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("friends.add", error, parameters, (Func<dynamic, dynamic>) SuccessAction);
        }

        public bool JoinGroup(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_id", GetGroupIdByUsername(username)}
            };

            dynamic SuccessAction(dynamic response) => response == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("groups.join", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("groups.join", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
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

            dynamic SuccessAction(dynamic response) => response.liked == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("likes.isLiked", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("likes.isLiked", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
        }

        public bool IsMember(string groupdId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_id", groupdId},
                {"user_id", Account.userId},
                {"extended", "1"}
            };

            dynamic SuccessAction(dynamic response) => response.member == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("groups.isMember", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("groups.isMember", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
        }

        public bool IsFriend(string userId)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_ids", userId}
            };

            dynamic SuccessAction(dynamic response) => response.friend_status == 1;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("friends.areFriends", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response[0]);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("friends.areFriends", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
        }

        public string GetUserIdByUsername(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"user_ids", username}
            };

            dynamic SuccessAction(dynamic response) => response.id;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("users.get", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response[0]);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("users.get", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
        }

        public string GetGroupIdByUsername(string username)
        {
            var parameters = new Dictionary<string, string>
            {
                {"group_ids", username}
            };

            dynamic SuccessAction(dynamic response) => response.id;

            var result = _helper.SendRequest(() => _request.Get(_generateUrl.Generate("groups.getById", parameters)));
            if (result.json.response != null)
            {
                return SuccessAction(result.json.response[0]);
            }

            dynamic error = result.json.error;
            return _handleErrors.HandleErrors("groups.getById", error, parameters, (Func<dynamic, dynamic>)SuccessAction);
        }
    }
}