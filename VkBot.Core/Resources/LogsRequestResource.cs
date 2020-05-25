using VkBot.Core.Types;

namespace VkBot.Core.Resources
{
    public class LogsRequestResource
    {
        public string message;
        public int accountId;

        public LogsRequestResource()
        {
        }

        public LogsRequestResource(string message, int accountId)
        {
            this.message = message;
            this.accountId = accountId;
        }

        public override string ToString()
        {
            return $"Logs(" +
                   $"message: '{message}', " +
                   $"accountId: {accountId}" +
                   $")";
        }
    }
}
