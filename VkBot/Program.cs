using System;

namespace VkBot
{
    class Program
    {
        static void Main(string[] args)
        {
            VkBot vkBot = new VkBot("12345");
            vkBot.Run();

            Console.ReadKey();
        }
    }
}
