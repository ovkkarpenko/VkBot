using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Interfaces
{
    public interface IHandleErrors
    {
        /**
         * @param urlMethod - method for generate url
         * @param parameters - parameters for added to the url
         * @param successAction - will be called after handle the error
         */
        dynamic HandleErrors(string urlMethod, dynamic error, Dictionary<string, string> parameters,
            Func<dynamic, dynamic> successAction);
    }
}
