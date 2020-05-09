using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Core.Resources;

namespace VkBot.Interfaces
{
    public interface ApiService
    {
        bool CheckAuth();

        List<Account> GetAccounts();

        Settings GetSettings();

        List<Task> GetTasks(FindTasksRequestResource requestResource);

        void SaveAccount(Account account);

        void MarkTasksCompleted(List<Task> tasks, int accountId);
    }
}
