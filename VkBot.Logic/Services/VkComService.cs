using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Data.Repositories;
using VkBot.Interfaces;

namespace VkBot.Logic.Services
{
    public class VkComService
    {
        private readonly IVkComRepository _vkComRepository = new VkComRepository();

        public AccountModel Auth(AccountModel account)
        {
            return null;
        }

        public AccountModel ReAuth(string token)
        {
            _vkComRepository.Auth(token);
            return null;
        }

        public void DoLikes(List<string> urls)
        {

        }

        public void DoReposts(List<string> urls)
        {

        }

        public void DoFriends(List<string> urls)
        {

        }

        public void DoGroups(List<string> urls)
        {

        }
    }
}
