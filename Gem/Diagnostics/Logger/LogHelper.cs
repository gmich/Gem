using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Diagnostics.Logger
{
    /// <summary>
    /// Helper class for logging
    /// </summary>
    public class LogHelper
    {

        #region Fields

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion


        #region Append

        public static void LogError(string error,params object[] args)
        {
            log.Error(string.Format(error, args));
        }

        #endregion

    }
}
