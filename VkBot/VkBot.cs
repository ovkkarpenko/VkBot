using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Data.Repositories;
using VkBot.Interfaces;
using VkBot.Logic.Impl;

namespace VkBot
{
    public class VkBot
    {
        private const int CountThreeds = 10;

        private readonly Thread[] _threads;
        private readonly ApiServiceImpl _apiService;

        private Settings _settings;
        private IEnumerator<Account> _accounts;

        public VkBot(string bindingKey)
        {
            _apiService = new ApiServiceImpl(bindingKey);
            _threads = new Thread[CountThreeds];
        }

        public void Run()
        {
            if (_apiService.CheckAuth())
            {
                _settings = _apiService.GetSettings();
                _accounts = new Iterator<Account>(_apiService.GetAccounts()).GetItems();

                Start();
            }
        }

        private void Start()
        {
            object mLock = new object();

            for (var i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(() =>
                {
                    lock (mLock)
                    {
                        Update();
                    }
                });

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
            while (_accounts.MoveNext() && _accounts.Current != null)
            {
                Account account = _accounts.Current;

                SocialNetworkService vkCom = new VkcomServiceImpl(account.token, _settings.rucaptchaKey);

                bool isAuth = vkCom.Auth();
                if (!isAuth)
                {
                    continue;
                }

                List<Task> friends =
                    _apiService.GetTasks(new FindTasksRequestResource(account.id, TaskType.FRIEND));

                if (friends.Count != 0)
                {
                    List<Task> tasksDone = vkCom.DoFriends(friends);
                    _apiService.MarkTasksCompleted(tasksDone, account.id);
                }

                List<Task> groups =
                    _apiService.GetTasks(new FindTasksRequestResource(account.id, TaskType.GROUP));

                if (groups.Count != 0)
                {
                    List<Task> tasksDone = vkCom.DoGroups(groups);
                    _apiService.MarkTasksCompleted(tasksDone, account.id);
                }

                List<Task> likes =
                    _apiService.GetTasks(new FindTasksRequestResource(account.id, TaskType.LIKE));

                if (likes.Count != 0)
                {
                    List<Task> tasksDone = vkCom.DoLikes(likes);
                    _apiService.MarkTasksCompleted(tasksDone, account.id);
                }

                List<Task> reposts =
                    _apiService.GetTasks(new FindTasksRequestResource(account.id, TaskType.REPOST));

                if (reposts.Count != 0)
                {
                    List<Task> tasksDone = vkCom.DoReposts(reposts);
                    _apiService.MarkTasksCompleted(tasksDone, account.id);
                }
            }
        }
    }
}