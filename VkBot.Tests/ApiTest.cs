using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Entities;
using VkBot.Data.Repositories;

namespace VkBot.Tests
{
    [TestClass]
    public class ApiTest
    {
        private const string BindingKey = "12345";

        [TestMethod]
        public void CheckAuthTest()
        {
            //given
            Api api = new Api(BindingKey);

            //when
            Program program = api.GetProgram();

            //then
            Assert.IsNotNull(program);
        }

        [TestMethod]
        public void GetSettingsTest()
        {
            //given
            Api api = new Api(BindingKey);

            //when
            SettingsModel settings = api.GetSettings();

            //then
            Assert.IsNotNull(settings);
        }
    }
}
