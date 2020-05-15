using System;

namespace VkBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            VkBot vkBot = new VkBot("JX659UlS3ZfjB1GaCauFUJJz5Vz2W8mthIQ0WUmABRG1j5esEaGQfUzLE4rfv8VTbnLzO9AadeJu31rFuQXm8wd8AAqutznvHDwA");
            vkBot.Run();

            Console.ReadKey();
        }
    }
}