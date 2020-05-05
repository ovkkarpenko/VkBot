using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Data.Repositories;
using VkBot.Interfaces;

namespace VkBot.Logic.Services
{
    public class ApiService
    {
        private readonly IApi _siteApi = new SiteApi();

        public bool Auth(string token)
        {
            return false;
        }

        public AccountModel GetFreeAccount()
        {
            return null;
        }

        public SettingsModel GetBotSettings()
        {
            return null;
        }

        public List<string> GetLikes()
        {
            return null;
        }

        public List<string> GetReposts()
        {
            return null;
        }

        public List<string> GetFriends()
        {
            return null;
        }

        public List<string> GetGroups()
        {
            return null;
        }
    }
}
