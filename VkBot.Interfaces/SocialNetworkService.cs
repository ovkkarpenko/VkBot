using System.Collections.Generic;
using VkBot.Core.Entities;

namespace VkBot.Interfaces
{
    public interface SocialNetworkService
    {
        bool Auth();

        List<Task> DoLikes(List<Task> tasks);

        List<Task> DoReposts(List<Task> tasks);

        List<Task> DoFriends(List<Task> tasks);

        List<Task> DoGroups(List<Task> tasks);

        Account GetCurrentUser();
    }
}
