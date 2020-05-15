using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using VkBot.Core.Entities;
using VkBot.Core.Exceptions;
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
        private const int CountThreeds = 5;

        private readonly Thread[] _threads;
        private readonly ApiService _api;

        private Settings _settings;
        private IEnumerator<Account> _accounts;
        
        public VkBot(string bindingKey)
        {
            _api = new ApiServiceImpl(bindingKey);
            _threads = new Thread[CountThreeds];
        }

        public void Run()
        {
            if (_api.CheckAuth())
            {
                _settings = _api.GetSettings();
                _accounts = new Iterator<Account>(_api.GetAccounts()).GetItems();

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
                SocialNetworkService vkCom = new VkcomServiceImpl(account, _settings.rucaptchaKey);

                try
                {
                    if (vkCom.Auth())
                    {
                        _api.SaveAccount(vkCom.GetCurrentUser());
                        DoTasks(vkCom, account);
                    }
                }
                catch (AuthorizationException e)
                {
                    Helper.Log.Error($"IN Update - Authorisation error, info: {e.Message}");

                    account.status = AccountStatus.INVALID;
                    _api.SaveAccount(account);
                }
                catch (NeedValidationException e)
                {
                    Helper.Log.Error($"IN Update - NeedValidation error, info: {e.Message}");

                    account.status = AccountStatus.NEED_VALIDATION;
                    _api.SaveAccount(account);
                }
                catch (Exception e)
                {
                    Helper.Log.Error($"IN Update - Not expected error, info: {e.Message}");

                    account.status = AccountStatus.ERROR;
                    _api.SaveAccount(account);
                }
            }
        }

        private void DoTasks(SocialNetworkService vkCom, Account account)
        {
            List<Task> friends =
                _api.GetTasks(new FindTasksRequestResource(account.id, TaskType.FRIEND));

            if (friends.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoFriends(friends);
                _api.MarkTasksCompleted(tasksDone, account.id);
            }

            List<Task> groups =
                _api.GetTasks(new FindTasksRequestResource(account.id, TaskType.GROUP));

            if (groups.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoGroups(groups);
                _api.MarkTasksCompleted(tasksDone, account.id);
            }

            List<Task> likes =
                _api.GetTasks(new FindTasksRequestResource(account.id, TaskType.LIKE));

            if (likes.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoLikes(likes);
                _api.MarkTasksCompleted(tasksDone, account.id);
            }

            List<Task> reposts =
                _api.GetTasks(new FindTasksRequestResource(account.id, TaskType.REPOST));

            if (reposts.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoReposts(reposts);
                _api.MarkTasksCompleted(tasksDone, account.id);
            }
        }
    }
}