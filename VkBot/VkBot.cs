using System.Collections.Generic;
using System.Threading;
using VkBot.Core.Entities;
using VkBot.Data.Repositories;
using VkBot.Logic.Impl;

namespace VkBot
{
    class VkBot
    {
        private const int CountThreeds = 1;

        private readonly ApiServiceImpl _apiService = new ApiServiceImpl();

        private Thread[] _threads;
        private SettingsModel _settings;

        public VkBot()
        {
            _threads = new Thread[CountThreeds];
        }

        public void Run(string siteToken)
        {
            if (_apiService.Auth(siteToken))
            {
                _settings = _apiService.GetBotSettings();
                Start();
            }
        }

        private void Start()
        {
            for (var i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(Update);
                _threads[i].Start();
            }
        }

        private void Stop()
        {
            for (var i = 0; i < _threads.Length; i++)
            {
                if (_threads[i] != null && _threads[i].IsAlive)
                {
                    _threads[i].Abort();
                }
            }
        }

        private void Update()
        {
            VkcomService vkComService = new VkcomService(new Vkcom(""));

            AccountModel account = _apiService.GetFreeAccount();

            account = vkComService.Auth(account);
            if (account == null)
            {
                return;
            }

            while (true)
            {
                List<string> likes = _apiService.GetLikes();
                vkComService.DoLikes(likes);

                List<string> reposts = _apiService.GetReposts();
                vkComService.DoReposts(reposts);

                List<string> friends = _apiService.GetFriends();
                vkComService.DoFriends(friends);

                List<string> groups = _apiService.GetGroups();
                vkComService.DoGroups(groups);
            }
        }
    }
}