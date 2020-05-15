using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Types;
using VkBot.Data.Repositories;
using VkBot.Data.Repositories.Vkcom;

namespace VkBot.Tests
{
    [TestClass]
    public class ApiTest
    {
        private const string BindingKey = "JX659UlS3ZfjB1GaCauFUJJz5Vz2W8mthIQ0WUmABRG1j5esEaGQfUzLE4rfv8VTbnLzO9AadeJu31rFuQXm8wd8AAqutznvHDwA";

        [TestMethod]
        public void GetProgramTest()
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
            requestResource.taskType = TaskType.LIKE;

            //when
            List<Task> tasks = api.GetTasks(requestResource);

            //then
            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_returnNull_GetTasksTest()
        {
            //given
            Api api = new Api(BindingKey);

            FindTasksRequestResource requestResource = new FindTasksRequestResource();
            requestResource.accountId = 111111;
            requestResource.taskType = TaskType.LIKE;

            //when
            List<Task> tasks = api.GetTasks(requestResource);

            //then
            Assert.IsNull(tasks);
        }

        [TestMethod]
        public void SaveAccountTest()
        {
            //given
            Api api = new Api(BindingKey);
            Account loadedAccount = api.GetAccounts()[0];
            Vkcom vkcom = new Vkcom(loadedAccount, "");

            //when
            Account account = vkcom.GetCurrentUser();
            bool result = api.SaveAccount(account);

            //then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void when_paramsAreInvalid_should_returnNull_SaveAccountTest()
        {
            //given
            Api api = new Api(BindingKey);

            Account account = new Account();

            //when
            bool result = api.SaveAccount(account);

            //then
            Assert.IsFalse(result);
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

        [TestMethod]
        public void when_paramsAreInvalid_should_returnNull_MarkTaskCompletedTest()
        {
            //given
            Api api = new Api(BindingKey);

            MarkTaskCompletedRequestResource requestResource1 = new MarkTaskCompletedRequestResource(1, 1111111);
            MarkTaskCompletedRequestResource requestResource2 = new MarkTaskCompletedRequestResource(1111111, 1);

            //when
            bool result1 = api.MarkTaskCompleted(requestResource1);
            bool result2 = api.MarkTaskCompleted(requestResource2);

            //then
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void when_bindingKeyIsInvalid_should_returnNullOrFalse()
        {
            //given
            Api api = new Api("123");

            //when
            Program program = api.GetProgram();
            Settings settings = api.GetSettings();
            List<Account> accounts = api.GetAccounts();
            List<Task> tasks = api.GetTasks(new FindTasksRequestResource(1, TaskType.LIKE));
            bool isSaved = api.SaveAccount(new Account(1, ""));
            bool isMarked = api.MarkTaskCompleted(new MarkTaskCompletedRequestResource(1, 1));

            //then
            Assert.IsNull(program);
            Assert.IsNull(settings);
            Assert.IsNull(accounts);
            Assert.IsNull(tasks);
            Assert.IsFalse(isSaved);
            Assert.IsFalse(isMarked);
        }
    }
}
