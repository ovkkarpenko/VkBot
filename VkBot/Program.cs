using System;
using VkBot.Core.Entities;

namespace VkBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter program BindingKey: ");
            string bindingKey = Console.ReadLine();
            
            VkBot vkBot = new VkBot(bindingKey);
            vkBot.Run();

            Console.ReadKey();
        }
    }
}