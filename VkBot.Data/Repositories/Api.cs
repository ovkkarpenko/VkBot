using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Leaf.xNet;
using Newtonsoft.Json;
using VkBot.Core.Entities;
using VkBot.Core.Resources;
using VkBot.Core.Utils;

namespace VkBot.Data.Repositories
{
    public class Api
    {
        private readonly string _bindingKey;
        private const string Host = "http://localhost:8075/api/v1";

        private readonly Helper _helper;
        private readonly HttpRequest _request;

        public Api(string bindingKey)
        {
            _helper = new Helper();
            _request = new HttpRequest();

            _request.UserAgentRandomize();
            _bindingKey = bindingKey;
        }

        ~Api()
        {
            _request.Dispose();
        }

        public Program GetProgram()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"client_program")));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            Program program = new Program();
            program.name = response.name;
            program.bindingKey = response.bindingKey;

            return program;
        }

        public List<Account> GetAccounts()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"client_program/account")));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            List<Account> accounts = new List<Account>();

            foreach (dynamic item in response)
            {
                var account = new Account
                {
                    id = item.id,
                    userId = item.userId,
                    token = item.token,
                    fullName = item.fullName,
                    //birthday = item.birthday;
                    country = item.country,
                    userAgent = item.userAgent,
                    proxy = item.proxy
                };

                if (item.gender != null) account.gender = item.gender;
                account.status = item?.status;

                accounts.Add(account);
            }

            return accounts;
        }

        public Settings GetSettings()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"client_program/settings")));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            Settings settings = new Settings();
            settings.proxies = Regex.Split($"{response.proxies}", "\r\n").ToList();
            settings.useragents = Regex.Split($"{response.useragents}", "\r\n").ToList();
            settings.rucaptchaKey = response.rucaptchaKey;
            settings.timeoutLike = response.timeoutLike;
            settings.timeoutFriend = response.timeoutFriend;
            settings.timeoutRepost = response.timeoutRepost;
            settings.timeoutGroup = response.timeoutGroup;
            settings.timeoutAfterTask = response.timeoutAfterTask;
            settings.proxyType = response.proxyType;

            return settings;
        }

        public List<Task> GetTasks(FindTasksRequestResource requestResource)
        {
            dynamic json = JsonConvert.SerializeObject(requestResource);
            var result = _helper.SendRequest(() => _request.Post(GenerateUrl($"client_program/task"), json, "application/json"));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            List<Task> tasks = new List<Task>();

            foreach (dynamic item in response)
            {
                Task task = new Task();
                task.id = item.id;
                task.name = item.name;
                task.url = item.url;
                task.count = item.count;
                task.statsCompleted = item.statsCompleted;
                task.status = item.status;
                task.taskType = item.taskType;
                task.objectType = item.objectType;

                tasks.Add(task);
            }

            return tasks;
        }

        public bool SaveAccount(Account account)
        {
            dynamic json = JsonConvert.SerializeObject(account);
            var result = _helper.SendRequest(() => _request.Put(GenerateUrl($"client_program/account"), json, "application/json"));
            if (result.httpException != null)
            {
                return false;
            }

            return true;
        }

        public bool MarkTaskCompleted(MarkTaskCompletedRequestResource requestResource)
        {
            dynamic json = JsonConvert.SerializeObject(requestResource);
            var result = _helper.SendRequest(() => _request.Put(GenerateUrl($"client_program/task/completed"), json, "application/json"));
            if (result.httpException != null)
            {
                return false;
            }

            return true;
        }

        private string GenerateUrl(string method, Dictionary<string, string> parameters = null)
        {
            string url = $"{Host}/{method}/{_bindingKey}?" +
                         $"{(parameters != null ? "&" + string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}")) : "")}";

            return url;
        }
    }
}
