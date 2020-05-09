using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Core.Resources
{
    public class MarkTaskCompletedRequestResource
    {
        public int taskId { get; set; }
        public int accountId { get; set; }

        public MarkTaskCompletedRequestResource()
        {
        }

        public MarkTaskCompletedRequestResource(int taskId, int accountId)
        {
            this.taskId = taskId;
            this.accountId = accountId;
        }
    }
}
