using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Utils;

namespace VkBot.Tests
{
    [TestClass]
    public class RucaptchaTest
    {
        private const string Key = "013dcdca04747d528e9518692380b3ad";
        private const string ImagePath = @"C:\Users\sanya\source\repos\VkBot\VkBot.Tests\resources\capcha.png";

        [TestMethod]
        [Timeout(30000)]
        public void ImageCaptchaTest()
        {
            if (!File.Exists(ImagePath))
            {
                Assert.Fail("Captcha is not found.");
            }

            //given
            string expected = "шшеша";

            Rucaptcha rucaptcha = new Rucaptcha(Key);

            //when
            string result = rucaptcha.ImageCaptcha(File.ReadAllBytes(ImagePath));

            //then
            Assert.IsNotNull(result, "Unknown error.");
            Assert.AreEqual(expected, result);
        }
    }
}
