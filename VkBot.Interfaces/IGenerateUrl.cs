using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Interfaces
{
    public interface IGenerateUrl
    {
        string Generate(string method, Dictionary<string, string> parameters = null);
    }
}
