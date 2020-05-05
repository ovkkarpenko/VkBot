using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Data.Repositories;

namespace VkBot.Tests
{
    [TestClass]
    public class VkcomTest
    {
        private string _token;
        private Vkcom _vkcom;

        [TestInitialize]
        public void Initialize()
        {
            _token = "59bee568c620da6b1904aa2b8d143fcb26003c15266eb0dbe8c4a240fdc0fda49f10438b3910c73c886f5";
            _vkcom = new Vkcom(_token);
        }

        [TestMethod]
        public void CheckAuthTest()
        {
            var accountInfo = _vkcom.Auth();
            Assert.IsNotNull(accountInfo, "Token is invalid or expired");
        }
    }
}
