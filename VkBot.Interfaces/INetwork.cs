using VkBot.Core.Entities;

namespace VkBot.Interfaces
{
    public interface INetwork
    {
        bool Auth();

        void AddLike(string ownerId, string itemId);

        bool IsLiked(string ownerId, string itemId);

        bool AddRepost(string ownerId, string itemId);

        bool AddFriend(string userId);

        bool AddGroup(string groupId);
    }
}
