using VkBot.Core.Entities;

namespace VkBot.Interfaces
{
    public interface SocialNetworkService
    {
        bool Auth();

        void AddLike(string ownerId, string itemId);

        bool AddRepost(string objectId);

        void AddFriend(string userId);

        void JoinGroup(string groupId);

        bool IsLiked(string ownerId, string itemId);

        bool IsMember(string groupId);

        bool IsFriend(string groupId);
    }
}
