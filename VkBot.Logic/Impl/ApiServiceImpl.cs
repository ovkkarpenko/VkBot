using System;
using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;
using VkBot.Core.Utils;
using VkBot.Data.Repositories;
using VkBot.Interfaces;

namespace VkBot.Logic.Impl
{
    public class ApiServiceImpl : ApiService
    {
        private readonly Api _api;

        public ApiServiceImpl(string bindingKey)
        {
            _api = new Api(bindingKey);
        }

        public bool CheckAuth()
        {
            Program program = _api.GetProgram();

            if(program == null)
            {
                Helper.Log.Info($"IN CheckAuth - no program loaded");
                return false;
            }

            Helper.Log.Info($"IN CheckAuth - {program} program loaded");
            return true;
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = _api.GetAccounts();

            if (accounts.Count == 0)
            {
                Helper.Log.Info($"IN GetAccounts - no accounts loaded");
                return new List<Account>();
            }

            Helper.Log.Info($"IN GetAccounts - {accounts.Count} account loaded");
            return accounts;
        }

        public Settings GetSettings()
        {
            Settings settings = _api.GetSettings();

            if (settings == null)
            {
                Helper.Log.Info($"IN GetSettings - no settings loaded");
                return null;
            }

            Helper.Log.Info($"IN GetSettings - settings loaded");
            return settings;
        }

        public List<Task> GetTasks(FindTasksRequestResource requestResource)
        {
            List<Task> tasks = _api.GetTasks(requestResource);

            if (tasks.Count == 0)
            {
                Helper.Log.Info($"IN GetTasks - no tasks loaded");
                return new List<Task>();
            }

            Helper.Log.Info(
                $"IN GetTasks - {tasks.Count} tasks loaded by taskType: : {Enum.GetName(typeof(TaskType), requestResource.taskType).ToLower()}");
            return tasks;
        }

        public void SaveAccount(Account account)
        {
            _api.SaveAccount(account);
            Helper.Log.Info($"IN SaveAccount - {account} account saved");
        }

        public void MarkTasksCompleted(List<Task> tasks, int accountId)
        {
            foreach (var task in tasks)
            {
                _api.MarkTaskCompleted(new MarkTaskCompletedRequestResource(task.id, accountId));
            }

            Helper.Log.Info($"IN MarkTasksCompleted - {tasks.Count} tasks marked by accountId: {accountId}");
        }
    }
}
