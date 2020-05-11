using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;
using VkBot.Core.Entities;
using VkBot.Core.Exceptions;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomHandleErrors : IHandleErrors
    {
        private readonly Helper _helper;
        private readonly HttpRequest _request;

        private readonly IGenerateUrl _generateUrl;
        private readonly IParseCaptcha _parseCaptcha;

        private readonly int _accountId;

        public VkcomHandleErrors(Helper helper, HttpRequest request, IGenerateUrl generateUrl, IParseCaptcha parseCaptcha, int accountId)
        {
            _helper = helper;
            _request = request;
            _generateUrl = generateUrl;
            _parseCaptcha = parseCaptcha;
            _accountId = accountId;
        }

        ~VkcomHandleErrors()
        {
            _request.Dispose();
        }

        public dynamic Handle(string urlMethod, dynamic error, Dictionary<string, string> parameters,
            Func<dynamic, dynamic> successAction)
        {
            string errorCode = error.error_code;

            if (errorCode == "5")
            {
                throw new AuthorizationException($"accountId: {_accountId}, error_message: {error.error_msg}");
            }
            if (errorCode == "100")
            {
                throw new ArgumentException(_helper.ParametersToString(parameters));
            }
            if (errorCode == "14")
            {
                string captchaSid = error.captcha_sid;
                string captchaImageUrl = $"https://vk.com/captcha.php?sid={captchaSid}&s=1";
                HttpResponse captchaImage = _request.Get(captchaImageUrl);

                parameters.Add("captcha_sid", captchaSid);

                Func<Dictionary<string, string>, dynamic> requestFunc = (@params) =>
                {
                    return _helper.SendRequest(() => _request.Get(_generateUrl.Generate(urlMethod, @params))).json;
                };

                dynamic json = _parseCaptcha.Parse(captchaImage.ToBytes(), parameters, requestFunc);
                if (json != null && json.response != null)
                {
                    return successAction != null ? successAction(json.response) : null;
                }

                throw new CaptchaException($"{json?.error.error_msg}");
            }

            return null;
        }
    }
}
