using System.Collections.Generic;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;

namespace VkBot.Interfaces
{
    public interface ApiService
    {
        bool CheckAuth();

        List<Account> GetAccounts();

        Settings GetSettings();

        ProgramStatus GetProgramStatus();

        List<Task> GetTasks(FindTasksRequestResource requestResource);

        void SaveAccount(Account account);

        void AddLogs(LogsRequestResource requestResource);

        void MarkTasksCompleted(List<Task> tasks, int accountId);
    }
}
