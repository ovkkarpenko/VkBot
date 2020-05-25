using System;
using System.Collections.Generic;
using System.Text;
using VkBot.Core.Types;

namespace VkBot.Core.Entities
{
    public class Program
    {
        public string name { get; set; }
        public string bindingKey { get; set; }
        public ProgramStatus status { get; set; }

        public override string ToString()
        {
            return $"Program(" +
                   $"name='{name}', " +
                   $"bindingKey='{bindingKey}', " +
                   $"status='{Enum.GetName(typeof(ProgramStatus), status)}')";
        }
    }
}
