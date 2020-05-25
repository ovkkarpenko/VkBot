using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VkBot.Core.Entities;
using VkBot.Core.Exceptions;
using VkBot.Core.Resources;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Interfaces;
using VkBot.Logic.Impl;
using Task = VkBot.Core.Entities.Task;

namespace VkBot
{
    public class VkBot
    {
        private int _activeThreads;
        private ProgramStatus _programStatus;

        private readonly string _bindingKey;

        private Thread[] _threads;

        private Settings _settings;
        private List<Account> _accounts;
        
        public VkBot(string bindingKey)
        {
            _bindingKey = bindingKey;
        }

        public void Run()
        {
            ApiService api = new ApiServiceImpl(_bindingKey);

            if (api.CheckAuth())
            {
                _settings = api.GetSettings();
                _accounts = api.GetAccounts();

                new Thread(() =>
                {
                    while (true)
                    {
                        _programStatus = api.GetProgramStatus();
                        Thread.Sleep(10000);
                    }
                }).Start();

                while (true)
                {
                    Start();

                    while (_activeThreads > 1)
                    {
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private void Start()
        {
            Random rand = new Random();
            _threads = new Thread[_accounts.Count];

            for (int i = 0; i < _accounts.Count; i++)
            {
                if (_programStatus.Equals(ProgramStatus.DELETED) || _programStatus.Equals(ProgramStatus.ERROR))
                {
                    Environment.Exit(-1);
                }

                _threads[i] = new Thread(Update);
                _threads[i].Start(_accounts[i]);

                _activeThreads++;
                Thread.Sleep(rand.Next(100, 1000));

                while (_activeThreads >= 50 || _programStatus.Equals(ProgramStatus.STOPPED))
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void Stop()
        {
            
        }

        private void Update(object o)
        {
            Random rand = new Random();
            Account account = (Account) o;

            if (string.IsNullOrEmpty(account.userAgent))
            {
                account.userAgent = _settings.useragents[rand.Next(0, _settings.useragents.Count)];
            }

            if (_settings.proxies.Count != 0)
            {
                account.proxy = _settings.proxies[rand.Next(0, _settings.proxies.Count)];
            }

            ApiService api = new ApiServiceImpl(_bindingKey);
            SocialNetworkService vkCom = new VkcomServiceImpl(account, _settings.proxyType, _settings.rucaptchaKey);

            api.AddLogs(new LogsRequestResource("Start", account.id));

            try
            {
                if (vkCom.Auth())
                {
                    api.AddLogs(new LogsRequestResource("Authorization was successful", account.id));

                    api.SaveAccount(vkCom.GetCurrentUser());
                    DoTasks(api, vkCom, account);
                }
                else
                {
                    api.AddLogs(new LogsRequestResource("Authorization failed", account.id));
                }
            }
            catch (AuthorizationException e)
            {
                Helper.Log.Error($"IN Update - Authorization error, info: {e.Message}");

                account.status = AccountStatus.INVALID;
                api.SaveAccount(account);
            }
            catch (NeedValidationException e)
            {
                Helper.Log.Error($"IN Update - NeedValidation error, info: {e.Message}");

                account.status = AccountStatus.NEED_VALIDATION;
                api.SaveAccount(account);
            }
            catch (Exception e)
            {
                Helper.Log.Error($"IN Update - Not expected error, info: {e.Message}");

                account.status = AccountStatus.ERROR;
                api.SaveAccount(account);
            }

            api.AddLogs(new LogsRequestResource("Stop", account.id));
            _activeThreads--;
        }

        private void DoTasks(ApiService api, SocialNetworkService vkCom, Account account)
        {
            List<Task> friends =
                api.GetTasks(new FindTasksRequestResource(account.id, TaskType.FRIEND));

            if (friends.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoFriends(friends);
                api.MarkTasksCompleted(tasksDone, account.id);
                api.AddLogs(new LogsRequestResource($"Completed {tasksDone.Count} tasks", account.id));
            }

            List<Task> groups =
                api.GetTasks(new FindTasksRequestResource(account.id, TaskType.GROUP));

            if (groups.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoGroups(groups);
                api.MarkTasksCompleted(tasksDone, account.id);
                api.AddLogs(new LogsRequestResource($"Completed {tasksDone.Count} tasks", account.id));
            }

            List<Task> likes =
                api.GetTasks(new FindTasksRequestResource(account.id, TaskType.LIKE));

            if (likes.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoLikes(likes);
                api.MarkTasksCompleted(tasksDone, account.id);
                api.AddLogs(new LogsRequestResource($"Completed {tasksDone.Count} tasks", account.id));
            }

            List<Task> reposts =
                api.GetTasks(new FindTasksRequestResource(account.id, TaskType.REPOST));

            if (reposts.Count != 0)
            {
                List<Task> tasksDone = vkCom.DoReposts(reposts);
                api.MarkTasksCompleted(tasksDone, account.id);
                api.AddLogs(new LogsRequestResource($"Completed {tasksDone.Count} tasks", account.id));
            }
        }
    }
}