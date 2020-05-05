using VkBot.Core.Types;

namespace VkBot.Core.Entities
{
    public class AccountInfoModel
    {
        public string FullName { get; set; }

        public string City { get; set; }

        public string Birthday { get; set; }

        public GenderTypes Gender { get; set; }
    }
}
