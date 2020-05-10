using System;
using System.Collections.Generic;
using System.Text;
using Leaf.xNet;
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

        public VkcomHandleErrors(Helper helper, HttpRequest request, IGenerateUrl generateUrl, IParseCaptcha parseCaptcha)
        {
            _helper = helper;
            _request = request;
            _generateUrl = generateUrl;
            _parseCaptcha = parseCaptcha;
        }

        ~VkcomHandleErrors()
        {
            _request.Dispose();
        }

        public dynamic HandleErrors(string urlMethod, dynamic error, Dictionary<string, string> parameters,
            Func<dynamic, dynamic> successAction)
        {
            string errorCode = error.error_code;

            //Captcha
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
                    return successAction(json.response);
                }
            }

            return null;
        }
    }
}
