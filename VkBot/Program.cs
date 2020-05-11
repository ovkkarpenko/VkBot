using System;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace VkBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            VkBot vkBot = new VkBot("12345");
            vkBot.Run();

            Console.ReadKey();
        }
    }
}