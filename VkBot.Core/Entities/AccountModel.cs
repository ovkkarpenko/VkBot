namespace VkBot.Core.Entities
{
    public class AccountModel
    {
        public string Token { get; set; }
        public string Useragent { get; set; }
        public ProxyModel Proxy { get; set; }
        public AccountInfoModel AccountInfo { get; set; }
    }
}
