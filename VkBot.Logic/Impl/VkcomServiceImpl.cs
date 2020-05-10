using System;
using System.Collections.Generic;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Exceptions;
using VkBot.Core.Utils;
using VkBot.Data.Repositories.Vkcom;
using VkBot.Interfaces;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace VkBot.Logic.Impl
{
    public class VkcomServiceImpl : SocialNetworkService
    {
        private readonly Vkcom _vkcom;
        private readonly Helper _helper;

        private static readonly log4net.ILog _log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public VkcomServiceImpl(Account account, string rucaptchaKey)
        {
            _helper = new Helper();
            _vkcom = new Vkcom(account, rucaptchaKey);
        }

        public bool Auth()
        {
            try
            {
                Account account = _vkcom.GetCurrentUser();
                return account != null;
            }
            catch (AuthorizationException e)
            {

            }
            catch (Exception e)
            {

            }

            return false;
        }

        public List<Task> DoLikes(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                try
                {
                    var url = _helper.ParseUrlIntoOwnerAndItem(task.url, task.objectType);

                    _vkcom.AddLike(url.ownerId, url.itemId, task.objectType);
                    if (_vkcom.IsLiked(url.ownerId, url.itemId, task.objectType))
                    {
                        tasksDone.Add(task);
                    }
                }
                catch (AuthorizationException e)
                {

                }
                catch (ArgumentException e)
                {

                }
                catch (CaptchaException e)
                {

                }
                catch (Exception e)
                {

                }
            }

            _log.Info($"IN DoLikes - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoReposts(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string @object = _helper.ParseObjectFromUrl(task.url);

                bool isReposted = _vkcom.AddRepost(@object);
                if (isReposted)
                {
                    tasksDone.Add(task);
                }
            }

            _log.Info($"IN DoReposts - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoFriends(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string username = _helper.ParseUsernameFromUrl(task.url);

                _vkcom.AddFriend(username);
                if (_vkcom.IsFriend(username))
                {
                    tasksDone.Add(task);
                }
            }

            _log.Info($"IN DoFriends - {tasksDone.Count} tasks completed from {tasks.Count}");
            return tasksDone;
        }

        public List<Task> DoGroups(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string username = _helper.ParseUsernameFromUrl(task.url);

                _vkcom.JoinGroup(username);
                if (_vkcom.IsMember(username))
                {
                    tasksDone.Add(task);
                }
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
