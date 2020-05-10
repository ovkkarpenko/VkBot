using VkBot.Core.Types;

namespace VkBot.Core.Resources
{
    public class FindTasksRequestResource
    {
        public int accountId { get; set; }
        public TaskType taskType { get; set; }

        public FindTasksRequestResource()
        {
        }

        public FindTasksRequestResource(int accountId, TaskType taskType)
        {
            this.accountId = accountId;
            this.taskType = taskType;
        }
    }
}
