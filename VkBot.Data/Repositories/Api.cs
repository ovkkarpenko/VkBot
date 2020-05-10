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
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"program")));
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
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"program/account")));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            List<Account> accounts = new List<Account>();

            foreach (dynamic item in response)
            {
                Account account = new Account();
                account.id = item.id;
                account.userId = item.userId;
                account.token = item.token;
                account.fullName = item.fullName;
                account.country = item.country;
                account.userAgent = item.userAgent;
                account.proxy = item.proxy;
                //account.Birthday = item.birthday;
                if (item.gender != null) account.gender = item.gender;
                account.status = item.status;

                accounts.Add(account);
            }

            return accounts;
        }

        public Settings GetSettings()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"program/settings")));
            if (result.httpException != null)
            {
                return null;
            }

            dynamic response = result.json;

            Settings settings = new Settings();
            settings.proxies = Regex.Split($"{response.proxies}", "\r\n").ToList();
            settings.userAgents = Regex.Split($"{response.userAgents}", "\r\n").ToList();
            settings.rucaptchaKey = response.rucaptchaKey;
            settings.timeoutLikes = response.timeoutLikes;
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
            var result = _helper.SendRequest(() => _request.Post(GenerateUrl($"program/task"), json, "application/json"));
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
                task.url = item.url;
                task.count = item.count;
                task.status = item.status;
                task.objectType = item.objectType;

                tasks.Add(task);
            }

            return tasks;
        }

        public bool SaveAccount(Account account)
        {
            dynamic json = JsonConvert.SerializeObject(account);
            var result = _helper.SendRequest(() => _request.Put(GenerateUrl($"program/account"), json, "application/json"));
            if (result.httpException != null)
            {
                return false;
            }

            return true;
        }

        public bool MarkTaskCompleted(MarkTaskCompletedRequestResource requestResource)
        {
            dynamic json = JsonConvert.SerializeObject(requestResource);
            var result = _helper.SendRequest(() => _request.Put(GenerateUrl($"program/task/completed"), json, "application/json"));
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
