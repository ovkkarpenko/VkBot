using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Core.Types;

namespace VkBot.Interfaces
{
    public interface ApiService
    {
        bool CheckAuth();

        List<AccountModel> GetAccounts();

        SettingsModel GetSettings();

        List<Task> GetTasks(TaskType taskType);

        void SaveAccount(AccountModel account);

        void MarkTaskCompleted();
    }
}
