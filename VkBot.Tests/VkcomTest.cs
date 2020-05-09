using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Types;
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
            vkcom.AddLike(ownerId, itemId, ObjectType.POST);

            //then
            bool isLiked = vkcom.IsLiked(ownerId, itemId);
            Assert.IsTrue(isLiked, "Failed to like");
        }

        [TestMethod]
        public void AddRepostTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string objectId = "wall1_2442097";

            //when
            vkcom.Auth();
            bool isReposted = vkcom.AddRepost(objectId);

            //then
            Assert.IsTrue(isReposted, "Failed to repost");
        }

        [TestMethod]
        public void AddFriendTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string userId = "1";

            //when
            vkcom.Auth();
            vkcom.AddFriend(userId);

            //then
            bool isMember = vkcom.IsFriend(userId);
            Assert.IsTrue(isMember, "Failed to add friend");
        }

        [TestMethod]
        public void JoinGroupTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string groupdId = "60389602";

            //when
            vkcom.Auth();
            vkcom.JoinGroup(groupdId);

            //then
            bool isMember = vkcom.IsMember(groupdId);
            Assert.IsTrue(isMember, "Failed to joing to the group");
        }

        [TestMethod]
        public void GetUserIdByUsernameTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string username = "durov";
            string expected = "1";

            //when
            vkcom.Auth();
            string userId = vkcom.GetUserIdByUsername(username);

            //then
            Assert.AreEqual(userId, expected);
        }

        [TestMethod]
        public void GetGroupIdByUsernameTest()
        {
            //given
            Vkcom vkcom = new Vkcom(Token, RucaptchaKey);

            string username = "artpodslushano";
            string expected = "60389602";

            //when
            vkcom.Auth();
            string groupId = vkcom.GetGroupIdByUsername(username);

            //then
            Assert.AreEqual(groupId, expected);
        }
    }
}
