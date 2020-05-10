using System;
using System.Collections.Generic;
using Leaf.xNet;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomParseCaptcha : IParseCaptcha
    {
        private readonly Rucaptcha _rucaptcha;

        public VkcomParseCaptcha(string rucaptchaKey)
        {
            _rucaptcha = new Rucaptcha(rucaptchaKey);
        }

        public dynamic Parse(byte[] captchaImage, Dictionary<string, string> parameters, Func<Dictionary<string, string>, dynamic> requestFunc)
        {
            string capthaKey = _rucaptcha.ImageCaptcha(captchaImage);

            if (capthaKey != null)
            {
                parameters.Add("captcha_key", capthaKey);
                return requestFunc(parameters);
            }

            return null;
        }
    }
}
