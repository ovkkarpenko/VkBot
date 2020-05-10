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

        private static readonly log4net.ILog _log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ApiServiceImpl(string bindingKey)
        {
            _api = new Api(bindingKey);
        }

        public bool CheckAuth()
        {
            Program program = _api.GetProgram();
            _log.Info($"IN CheckAuth - {program} program loaded");
            return program != null;
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = _api.GetAccounts();
            _log.Info($"IN GetAccounts - {accounts.Count} account loaded");
            return accounts;
        }

        public Settings GetSettings()
        {
            Settings settings = _api.GetSettings();
            _log.Info($"IN GetSettings - settings loaded");
            return settings;
        }

        public List<Task> GetTasks(FindTasksRequestResource requestResource)
        {
            List<Task> tasks = _api.GetTasks(requestResource);
            _log.Info($"IN GetTasks - {tasks.Count} tasks loaded");
            return tasks;
        }

        public void SaveAccount(Account account)
        {
            _api.SaveAccount(account);
            _log.Info($"IN SaveAccount - {account} account saved");
        }

        public void MarkTasksCompleted(List<Task> tasks, int accountId)
        {
            foreach (var task in tasks)
            {
                _api.MarkTaskCompleted(new MarkTaskCompletedRequestResource(task.id, accountId));
            }

            _log.Info($"IN MarkTasksCompleted - {tasks.Count} tasks marked by accountId: {accountId}");
        }
    }
}
