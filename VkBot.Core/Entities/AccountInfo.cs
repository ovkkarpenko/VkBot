using VkBot.Core.Types;

namespace VkBot.Core.Entities
{
    public class AccountInfo
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string City { get; set; }

        public string Birthday { get; set; }

        public GenderTypes Gender { get; set; }
    }
}
