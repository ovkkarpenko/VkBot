using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using log4net;
using VkBot.Core.Exceptions;
using VkBot.Interfaces;

namespace VkBot.Data.Repositories.Vkcom
{
    public class VkcomHandleExceptions: IHandleExceptions
    {
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Handle(string method, Action actionsFunc)
        {
            try
            {
                actionsFunc();
            }
            catch (CaptchaException e)
            {
                _log.Warn($"IN {method} - Captcha error, info: {e.Message}");
            }
            catch (NullReferenceException e )
            {
                _log.Warn($"IN {method} - NullReferenceException, info: {e.Message}");
            }
            catch (ArgumentNullException e)
            {
                _log.Warn($"IN {method} - ArgumentNullException, info: {e.Message}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                _log.Warn($"IN {method} - ArgumentOutOfRangeException, info: {e.Message}");
            }
            catch (ArgumentException e)
            {
                _log.Warn($"IN {method} - ArgumentException, info: {e.Message}");
            }
        }
    }
}
