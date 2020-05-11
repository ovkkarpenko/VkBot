using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace VkBot.Interfaces
{
    public interface IHandleExceptions
    {
        /**
         * @param urlMethod - method for generate url
         * @param parameters - parameters for added to the url
         * @param successAction - will be called after handle the error
         */
        void Handle(string method, Action actionsFunc);
    }
}
