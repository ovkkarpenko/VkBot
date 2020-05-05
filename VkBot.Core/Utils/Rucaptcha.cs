using System.Threading;
using Leaf.xNet;

namespace VkBot.Core.Utils
{
    public class Rucaptcha
    {
        private readonly string _key;
        private const string Host = "http://rucaptcha.com";

        public Rucaptcha(string key)
        {
            _key = key;
        }

        public string ImageCaptcha(byte[] bytes)
        {
            using (HttpRequest request = new HttpRequest())
            {
                var multContent = new MultipartContent()
                {
                    {new StringContent(_key), "key"},
                    {new StringContent("1374"), "soft_id"},
                    {new BytesContent(bytes), "file", "captcha.png"}
                };

                string content = request.Post($"{Host}/in.php", multContent).ToString();

                if (content != null && !content.StartsWith("OK"))
                {
                    return null;
                }

                string captchaId = content?.Split('|')[1];

                while (true)
                {
                    content = request.Get($"{Host}/res.php?key={_key}&action=get&id={captchaId}").ToString();

                    if (content != null && content.StartsWith("OK"))
                    {
                        return content.Split('|')[1];
                    }

                    if (content != "CAPCHA_NOT_READY")
                    {
                        return null;
                    }

                    Thread.Sleep(5000);
                }
            }
        }
    }
}
