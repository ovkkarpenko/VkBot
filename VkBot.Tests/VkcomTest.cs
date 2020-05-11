using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Entities;
using VkBot.Core.Exceptions;
using VkBot.Core.Types;
using VkBot.Data.Repositories.Vkcom;

namespace VkBot.Tests
{
    [TestClass]
    public class VkcomTest
    {
        //https://oauth.vk.com/token?grant_type=password&client_id=2274003&client_secret=hHbZxrka2uZ6jB1inYsH&username=380996476978&password=qweasfwqe123SD

        private const string Token = "abf0120b0634484286fa020ea27fe1b5ff394467bd65ab75caabcaf1c40d8b1103076bf1341f5dcb7bbc0";
        private const string RucaptchaKey = "013dcdca04747d528e9518692380b3ad";

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void GetCurrentUserTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            //when
            Account account = vkcom.GetCurrentUser();

            //then
            Assert.IsNotNull(account, "Token is invalid or expired");
        }

        [TestMethod]
        public void AddLikeAndIsLikedTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string ownerId = "206934314",
                itemId = "44";

            //when
            vkcom.GetCurrentUser();
            vkcom.AddLike(ownerId, itemId, ObjectType.POST);

            //then
            bool isLiked = vkcom.IsLiked(ownerId, itemId, ObjectType.POST);
            Assert.IsTrue(isLiked, "Failed to like");
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_throwException_AddLikeAndIsLikedTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string ownerId = "",
                itemId = "";

            //when
            vkcom.GetCurrentUser();
            Action addLike = () => vkcom.AddLike(ownerId, itemId, ObjectType.POST);
            Action isLiked = () => vkcom.IsLiked(ownerId, itemId, ObjectType.POST);

            //then
            Assert.ThrowsException<ArgumentException>(addLike);
            Assert.ThrowsException<ArgumentException>(isLiked);
        }

        [TestMethod]
        public void AddRepostTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string objectId = "wall1_2442097";

            //when
            vkcom.GetCurrentUser();
            bool isReposted = vkcom.AddRepost(objectId);

            //then
            Assert.IsTrue(isReposted, "Failed to repost");
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_throwException_AddRepostTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string objectId = "";

            //when
            vkcom.GetCurrentUser();
            Action addRepost = () => vkcom.AddRepost(objectId);

            //then
            Assert.ThrowsException<ArgumentException>(addRepost);
        }

        [TestMethod]
        public void AddFriendAndIsFriendTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string userId = "1";

            //when
            vkcom.GetCurrentUser();
            vkcom.AddFriend(userId);

            //then
            bool isFriend = vkcom.IsFriend(userId);
            Assert.IsTrue(isFriend, "Failed to add friend");
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_throwException_AddFriendTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string userId = "";

            //when
            vkcom.GetCurrentUser();
            Action addFriend = () => vkcom.AddFriend(userId);

            //then
            Assert.ThrowsException<ArgumentException>(addFriend);
        }

        [TestMethod]
        public void JoinGroupAndIsMemberTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string groupId = "60389602";

            //when
            vkcom.GetCurrentUser();
            vkcom.JoinGroup(groupId);

            //then
            bool isMember = vkcom.IsMember(groupId);
            Assert.IsTrue(isMember, "Failed to joing to the group");
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_throwException_JoinGroupAndIsMemberTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string groupId = "123456qwertyyasd";

            //when
            vkcom.GetCurrentUser();
            Action joinGroup = () => vkcom.JoinGroup(groupId);

            //then
            Assert.ThrowsException<ArgumentException>(joinGroup);
        }

        [TestMethod]
        public void GetUserIdByUsernameTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string username = "durov";
            string expected = "1";

            //when
            vkcom.GetCurrentUser();
            string userId = vkcom.GetUserIdByUsername(username);

            //then
            Assert.AreEqual(userId, expected);
        }

        [TestMethod]
        public void GetGroupIdByUsernameTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account(Token), RucaptchaKey);

            string username = "artpodslushano";
            string expected = "60389602";

            //when
            vkcom.GetCurrentUser();
            string groupId = vkcom.GetGroupIdByUsername(username);

            //then
            Assert.AreEqual(groupId, expected);
        }

        [TestMethod]
        public void when_tokenIsInvalid_should_ThrowAuthorizationException_AllMethodTest()
        {
            //given
            Vkcom vkcom = new Vkcom(new Account("123"), RucaptchaKey);

            //when
            Action addLike = () => vkcom.AddLike("", "", ObjectType.PHOTO);
            Action addFriend = () => vkcom.AddFriend("");
            Action addRepost = () => vkcom.AddRepost("");
            Action joinGroup = () => vkcom.JoinGroup("");

            //then
            Assert.ThrowsException<AuthorizationException>(addLike);
            Assert.ThrowsException<AuthorizationException>(addFriend);
            Assert.ThrowsException<AuthorizationException>(addRepost);
            Assert.ThrowsException<AuthorizationException>(joinGroup);
        }
    }
}
