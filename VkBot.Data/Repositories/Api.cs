using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Types;
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
            _request.AddHeader("Content-Type", "application/json");

            _bindingKey = bindingKey;
        }

        ~Api()
        {
            _request.Dispose();
        }

        public Program GetProgram()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"program/{_bindingKey}")));
            dynamic response = result.json[0];

            Program program = new Program();
            program.Name = response.name;
            program.BindingKey = response.bindingKey;

            return program;
        }

        public List<AccountModel> GetAccounts()
        {
            throw new System.NotImplementedException();
        }

        public SettingsModel GetSettings()
        {
            var result = _helper.SendRequest(() => _request.Get(GenerateUrl($"program/settings/{_bindingKey}")));
            dynamic response = result.json[0];

            SettingsModel settings = new SettingsModel();
            settings.Proxies = Regex.Split($"{response.proxies}", "\r\n").ToList();
            settings.UserAgents = Regex.Split($"{response.userAgents}", "\r\n").ToList();
            settings.RucaptchaKey = response.rucaptchaKey;
            settings.TimeoutLikes = response.timeoutLikes;
            settings.TimeoutFriend = response.timeoutFriend;
            settings.TimeoutRepost = response.timeoutRepost;
            settings.TimeoutGroup = response.timeoutGroup;
            settings.TimeoutAfterTask = response.timeoutAfterTask;
            settings.ProxyType = response.proxyType;

            return settings;
        }

        public List<Task> GetTasks(TaskType taskType)
        {
            throw new System.NotImplementedException();
        }

        public void SaveAccount(AccountModel account)
        {
            throw new System.NotImplementedException();
        }

        public void MarkTaskCompleted()
        {
            throw new System.NotImplementedException();
        }

        private string GenerateUrl(string method, Dictionary<string, string> parameters = null)
        {
            string url = $"{Host}/{method}?" +
                         $"{(parameters != null ? "&" + string.Join("&", parameters.Select(pair => $"{pair.Key}={pair.Value}")) : "")}";

            return url;
        }
    }
}
