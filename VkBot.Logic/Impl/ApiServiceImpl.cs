using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;
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
            return program != null;
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = _api.GetAccounts();
            return accounts;
        }

        public Settings GetSettings()
        {
            Settings settings = _api.GetSettings();
            return settings;
        }

        public List<Task> GetTasks(FindTasksRequestResource requestResource)
        {
            List<Task> tasks = _api.GetTasks(requestResource);
            return tasks;
        }

        public void SaveAccount(Account account)
        {
            _api.SaveAccount(account);
        }

        public void MarkTasksCompleted(List<Task> tasks, int accountId)
        {
            foreach (var task in tasks)
            {
                _api.MarkTaskCompleted(new MarkTaskCompletedRequestResource(task.id, accountId));
            }
        }
    }
}
