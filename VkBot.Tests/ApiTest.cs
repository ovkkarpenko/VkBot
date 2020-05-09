using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;
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
            Settings settings = api.GetSettings();

            //then
            Assert.IsNotNull(settings);
        }

        [TestMethod]
        public void GetAccountsTest()
        {
            //given
            Api api = new Api(BindingKey);

            //when
            List<Account> accounts = api.GetAccounts();

            //then
            Assert.AreNotEqual(0, accounts.Count);
        }

        [TestMethod]
        public void GetTasksTest()
        {
            //given
            Api api = new Api(BindingKey);

            FindTasksRequestResource requestResource = new FindTasksRequestResource();
            requestResource.accountId = 1;
            requestResource.bindingKey = "12345";
            requestResource.taskType = TaskType.LIKE;

            //when
            List<Task> tasks = api.GetTasks(requestResource);

            //then
            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod]
        public void SaveAccountTest()
        {
            //given
            Api api = new Api(BindingKey);

            Account account = new Account();
            account.id = 1;
            account.proxy = "123";
            account.gender = Gender.WOMAN;
            account.status = AccountStatus.ERROR;

            //when
            bool result = api.SaveAccount(account);

            //then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void MarkTaskCompletedTest()
        {
            //given
            Api api = new Api(BindingKey);

            MarkTaskCompletedRequestResource requestResource = new MarkTaskCompletedRequestResource();
            requestResource.accountId = 1;
            requestResource.taskId = 1;
            
            //when
            bool result = api.MarkTaskCompleted(requestResource);

            //then
            Assert.IsTrue(result);
        }
    }
}
