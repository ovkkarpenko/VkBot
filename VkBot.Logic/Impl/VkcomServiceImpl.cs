using System;
using System.Collections.Generic;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Data.Repositories;
using VkBot.Interfaces;

namespace VkBot.Logic.Impl
{
    public class VkcomServiceImpl : SocialNetworkService
    {
        private readonly Vkcom _vkcom;
        private readonly Helper _helper;

        public VkcomServiceImpl(string token, string rucaptchaKey)
        {
            _helper = new Helper();
            _vkcom = new Vkcom(token, rucaptchaKey);
        }

        public bool Auth()
        {
            bool isAuth = _vkcom.Auth();
            return isAuth;
        }

        public List<Task> DoLikes(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string ownerId = "";
                string itemId = "";

                if (task.url.IndexOf("?z=") != -1)
                {
                    ownerId = task.url.Substring($"?z={_helper.GetObjectTypeName(task.objectType)}", "_");
                    itemId = task.url.SubstringLast("%2F", "_");
                }
                else
                {
                    ownerId = task.url.Substring(_helper.GetObjectTypeName(task.objectType), "_");
                    itemId = task.url.Substring(task.url.LastIndexOf("_") + 1);
                }

                _vkcom.AddLike(ownerId, itemId, task.objectType);

                if (_vkcom.IsLiked(ownerId, itemId, task.objectType))
                {
                    tasksDone.Add(task);
                }
            }

            return tasksDone;
        }

        public List<Task> DoReposts(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string @object = task.url.IndexOf("?z=") != -1
                    ? task.url.Substring("?z=", "%2F")
                    : task.url.Substring(task.url.LastIndexOf("/") + 1);

                bool isReposted = _vkcom.AddRepost(@object);

                if (isReposted)
                {
                    tasksDone.Add(task);
                }
            }

            return tasksDone;
        }

        public List<Task> DoFriends(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string username = task.url.Substring(task.url.LastIndexOf("/") + 1);
                string userId = _vkcom.GetUserIdByUsername(username);

                _vkcom.AddFriend(userId);

                if (_vkcom.IsFriend(userId))
                {
                    tasksDone.Add(task);
                }
            }

            return tasksDone;
        }

        public List<Task> DoGroups(List<Task> tasks)
        {
            List<Task> tasksDone = new List<Task>();

            foreach (Task task in tasks)
            {
                string username = task.url.Substring(task.url.LastIndexOf("/") + 1);
                string groupId = _vkcom.GetGroupIdByUsername(username);

                _vkcom.JoinGroup(groupId);

                if (_vkcom.IsMember(groupId))
                {
                    tasksDone.Add(task);
                }
            }

            return tasksDone;
        }
    }
}
