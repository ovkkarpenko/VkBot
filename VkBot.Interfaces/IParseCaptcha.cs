using System;
using System.Collections.Generic;
using Leaf.xNet;
using VkBot.Core.Utils;

namespace VkBot.Interfaces
{
    public interface IParseCaptcha
    {
        /**
         * @param captchaImage - bytes of image
         * @param parameters - will be added captcha parametr with result after solve the captcha 
         * @param requestFunc - will be called after solve the captcha
         *
         * @return json or html
         */
        dynamic Parse(byte[] captchaImage, Dictionary<string, string> parameters, Func<Dictionary<string, string>, dynamic> requestFunc);
    }
}
