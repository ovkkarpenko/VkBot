using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;
using VkBot.Core.Exceptions;
using VkBot.Core.Utils;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomHandleExceptions: IHandleExceptions
    {
        public void Handle(string method, Action actionsFunc)
        {
            try
            {
                actionsFunc();
            }
            catch (CaptchaException e)
            {
                Helper.Log.Warn($"IN {method} - Captcha error, info: {e.Message}");
            }
            catch (NullReferenceException e )
            {
                Helper.Log.Error($"IN {method} - NullReferenceException, info: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                Helper.Log.Error($"IN {method} - ArgumentNullException, info: {e.Message}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Helper.Log.Error($"IN {method} - ArgumentOutOfRangeException, info: {e.Message}");
            }
            catch (ArgumentException e)
            {
                Helper.Log.Error($"IN {method} - ArgumentException, info: {e.Message}");
            }
        }
    }
}
