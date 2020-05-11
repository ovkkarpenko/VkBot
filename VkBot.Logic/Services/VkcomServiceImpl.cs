using System.Collections.Generic;
using System.Reflection;
using log4net;
using VkBot.Core.Entities;
using VkBot.Core.Utils;
using VkBot.Data.Repositories.Vkcom;
using VkBot.Interfaces;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace VkBot.Logic.Services
{
    public class VkcomServiceImpl : SocialNetworkService
    {
        private readonly Vkcom _vkcom;
        private readonly Helper _helper;

        private readonly IHandleExceptions _handleExceptions;

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public VkcomServiceImpl(Account account, string rucaptchaKey)
        {
            _helper = new Helper();
            _vkcom = new Vkcom(account, rucaptchaKey);
            _handleExceptions = new VkcomHandleExceptions();
        }

        public bool Auth()
        {
            return _vkcom.GetCurrentUser() != null;
        }

        public List<Task> DoLikes(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                _handleExceptions.Handle("DoLikes", () =>
                {
                    var url = _helper.ParseUrlIntoOwnerAndItem(task.url, task.objectType);

                    _vkcom.AddLike(url.ownerId, url.itemId, task.objectType);
                    if (_vkcom.IsLiked(url.ownerId, url.itemId, task.objectType))
                    {
                        tasksDone.Add(task);
                    }
                });
            }

            _log.Info($"IN DoLikes - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoReposts(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                _handleExceptions.Handle("DoReposts", () =>
                {
                    string @object = _helper.ParseObjectFromUrl(task.url);

                    bool isReposted = _vkcom.AddRepost(@object);
                    if (isReposted)
                    {
                        tasksDone.Add(task);
                    }
                });
            }

            _log.Info($"IN DoReposts - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoFriends(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                _handleExceptions.Handle("DoFriends", () =>
                {
                    string username = _helper.ParseUsernameFromUrl(task.url);
                    string userId = _vkcom.GetUserIdByUsername(username);

                    _vkcom.AddFriend(userId);
                    if (_vkcom.IsFriend(userId))
                    {
                        tasksDone.Add(task);
                    }
                });
            }

            _log.Info($"IN DoFriends - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoGroups(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                _handleExceptions.Handle("DoFriends", () =>
                {
                    string username = _helper.ParseUsernameFromUrl(task.url);

                    _vkcom.JoinGroup(username);
                    if (_vkcom.IsMember(username))
                    {
                        tasksDone.Add(task);
                    }
                });
            }

            _log.Info($"IN DoGroups - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public Account GetCurrentUser()
        {
            return _vkcom.Account;
        }
    }
}
