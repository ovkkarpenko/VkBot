using VkBot.Core.Types;

namespace VkBot.Core.Resources
{
    public class FindTasksRequestResource
    {
        public int accountId { get; set; }
        public string bindingKey { get; set; }
        public TaskType taskType { get; set; }

        public FindTasksRequestResource()
        {
        }

        public FindTasksRequestResource(int accountId, string bindingKey, TaskType taskType)
        {
            this.accountId = accountId;
            this.bindingKey = bindingKey;
            this.taskType = taskType;
        }
    }
}
