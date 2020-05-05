using System.Collections.Generic;

namespace VkBot.Interfaces
{
    public interface IApi
    {
        bool Auth(string token);

        List<string> GetFreeAccount();

        List<string> GetBotSettings();

        List<string> GetLikes();

        List<string> GetReposts();

        List<string> GetFriends();

        List<string> GetGroups();
    }
}
