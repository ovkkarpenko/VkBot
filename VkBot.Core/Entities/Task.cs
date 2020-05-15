using VkBot.Core.Types;

namespace VkBot.Core.Entities
{
    public class Task
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int count { get; set; }
        public int statsCompleted { get; set; }
        public TaskStatus status { get; set; }
        public TaskType taskType { get; set; }
        public ObjectType objectType { get; set; }
    }
}
