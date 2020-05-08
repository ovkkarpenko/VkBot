using VkBot.Interfaces;

namespace VkBot.Logic.Impl
{
    public class VkcomServiceImpl : SocialNetworkService
    {
        private readonly SocialNetworkService _vkcom;

        public VkcomServiceImpl(SocialNetworkService vkcom)
        {
            _vkcom = vkcom;
        }

        public bool Auth()
        {
            throw new System.NotImplementedException();
        }

        public void AddLike(string ownerId, string itemId)
        {
            throw new System.NotImplementedException();
        }

        public bool AddRepost(string objectId)
        {
            throw new System.NotImplementedException();
        }

        public void AddFriend(string userId)
        {
            throw new System.NotImplementedException();
        }

        public void JoinGroup(string groupId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsLiked(string ownerId, string itemId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsMember(string groupId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsFriend(string groupId)
        {
            throw new System.NotImplementedException();
        }
    }
}
