using System.Collections.Generic;
using VkBot.Core.Entities;
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
            throw new System.NotImplementedException();
        }

        public List<AccountModel> GetAccounts()
        {
            throw new System.NotImplementedException();
        }

        public SettingsModel GetSettings()
        {
            throw new System.NotImplementedException();
        }

        public List<Task> GetTasks(TaskType taskType)
        {
            throw new System.NotImplementedException();
        }

        public void SaveAccount(AccountModel account)
        {
            throw new System.NotImplementedException();
        }

        public void MarkTaskCompleted()
        {
            throw new System.NotImplementedException();
        }
    }
}
