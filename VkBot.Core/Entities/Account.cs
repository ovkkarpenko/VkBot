namespace VkBot.Core.Entities
{
    public class AccountModel
    {
        public string Token { get; set; }
        public string Useragent { get; set; }
        public Proxy Proxy { get; set; }
        public AccountInfo AccountInfo { get; set; }
    }
}
