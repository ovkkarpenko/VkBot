using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Data.Repositories;

namespace VkBot.Tests
{
    [TestClass]
    public class VkcomTest
    {
        //https://oauth.vk.com/token?grant_type=password&client_id=2274003&client_secret=hHbZxrka2uZ6jB1inYsH&username=380996476978&password=qweasfwqe123SD

        private const string Token = "84c25c9bea3d8c71e2c6b9000ab6747622a51294b9c5035cae375a7b2def2039a297ab62e4b6db3d9d979";
        private const string RucaptchaKey = "013dcdca04747d528e9518692380b3ad";

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void AuthTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            //when
            bool isAuth = vkcom.Auth();

            //then
            Assert.IsTrue(isAuth, "Token is invalid or expired");
        }

        [TestMethod]
        public void AddLikeTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string ownerId = "206934314",
                itemId = "44";

            //when
            vkcom.Auth();
            vkcom.AddLike(ownerId, itemId);

            //then
            bool isLiked = vkcom.IsLiked(ownerId, itemId);
            Assert.IsTrue(isLiked, "Failed to like");
        }

        [TestMethod]
        public void ParseCaptchaTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string captchaUrl = "https://vk.com/captcha.php?sid=115072837620&s=1";

            //when
            vkcom.Auth();
            string result = vkcom.ParseCaptcha(captchaUrl);

            //then
            Assert.IsNotNull(result, "Failed to parse captcha");
            Assert.IsTrue(result.Length >= 5);
        }
    }
}
