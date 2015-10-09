using System;

namespace Gem.Engine.Logging
{
    /// <summary>
    /// Options that are invoked prior IAppender's members 
    /// </summary>
    /// <typeparam name="TAppender"></typeparam>
    public class AppenderOptions<TAppender>
         where TAppender : IAppender
    {

        #region Ctor

        public AppenderOptions()
        {
            Info = x => { };
            Debug = x => { };
            Warn = x => { };
            Error = x => { };
            Fatal = x => { };
        }

        #endregion

        #region Options

        public Action<TAppender> Message { get; set; }
        public Action<TAppender> Info { get; set; }
        public Action<TAppender> Debug { get; set; }
        public Action<TAppender> Warn { get; set; }
        public Action<TAppender> Error { get; set; }
        public Action<TAppender> Fatal { get; set; }

        #endregion

        #region Predefined Options

        private static Lazy<AppenderOptions<TAppender>> defaultOptions = new Lazy<AppenderOptions<TAppender>>();
        public static AppenderOptions<TAppender> None
        {
            get
            {
                return defaultOptions.Value;
            }
        }

        #endregion

    }
}