using VkBot.Core.Entities;

namespace VkBot.Interfaces
{
    public interface INetwork
    {
        AccountInfoModel Auth();

        bool AddLike(string postId);

        bool AddRepost(string postId);

        bool AddFriend(string userId);

        bool AddGroup(string groupId);
    }
}
