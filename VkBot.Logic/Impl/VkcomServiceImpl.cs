using System.Collections.Generic;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Data.Repositories;
using VkBot.Interfaces;

namespace VkBot.Logic.Impl
{
    public class VkcomServiceImpl : SocialNetworkService
    {
        private readonly Vkcom _vkcom;

        public VkcomServiceImpl(string token, string rucaptchaKey)
        {
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
                _vkcom.AddLike();

                if (_vkcom.IsLiked())
                {
                    tasksDone.Add(task);
                }
            }

            return tasksDone;
        }

        public List<Task> DoReposts(List<Task> tasks)
        {
            //_vkcom.AddRepost(objectId);
            return null;
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
