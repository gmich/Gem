using System;

namespace Gem.Infrastructure.Logging
{

    public class ConfigurableAppender
    {
        /// <summary>
        /// Creates a new instance of <see cref=">ConfigurableAppender"/> with the appender that's customised and the options
        /// </summary>
        /// <example>
        /// ConfigurableAppender
        /// .Create(new ActionAppender(
        ///         echo: Console.WriteLine,
        ///         formatTemplate: "[%Date{0:G}] %Verbosity - %Message"),
        ///         options =>
        ///         {
        ///             options.Error = appender =>
        ///             {
        ///                Console.BackgroundColor = ConsoleColor.White;
        ///                Console.ForegroundColor = ConsoleColor.Red;
        ///             };
        ///         });
        /// </example>
        /// <typeparam name="TAppend">The IAppender's type</typeparam>
        /// <param name="appender">The IAppender's instance</param>
        /// <param name="appenderOptions">The prior invocation options</param>
        /// <returns>An instance of <see cref="ConfigurableAppender<TAppend>"/></returns>
        public static ConfigurableAppender<TAppend> Create<TAppend>(TAppend appender, Action<AppenderOptions<TAppend>> appenderOptions)
            where TAppend: IAppender
        {
            var options = new AppenderOptions<TAppend>();
            appenderOptions(options);

            return new ConfigurableAppender<TAppend>(appender, options );
        }
    }

    /// <summary>
    /// A class that invokes an option delegate to customize the <see cref="IAppender"/> message 
    /// </summary>
    /// <typeparam name="TAppender">The appender that's customised</typeparam>
    public class ConfigurableAppender<TAppender> : IConfigurableAppender<TAppender> 
        where TAppender : IAppender
    {

        #region Readonly Fields

        private readonly TAppender appender;

        #endregion

        #region Ctor

        internal ConfigurableAppender(TAppender appender,
                                    AppenderOptions<TAppender> formattingOptions)
        {
            this.appender = appender;
            this.options = formattingOptions;
        }

        #endregion

        #region IConfigurableAppender Members

        public void Info(string message)
        {
            options.Info(appender);
            appender.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            options.Info(appender);
            appender.Info(message, args);
        }

        public void Debug(string message)
        {
            options.Debug(appender);
            appender.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            options.Debug(appender);
            appender.Debug(message, args);
        }

        public void Warn(string message)
        {
            options.Warn(appender);
            appender.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            options.Warn(appender);
            appender.Warn(message, args);
        }

        public void Error(string message)
        {
            options.Error(appender);
            appender.Error(message);
        }

        public void Error(string message, params object[] args)
        {
            options.Error(appender);
            appender.Error(message, args);
        }

        public void Fatal(string message)
        {
            options.Fatal(appender);
            appender.Fatal(message);
        }

        public void Fatal(string message, params object[] args)
        {
            options.Fatal(appender);
            appender.Fatal(message, args);
        }

        private AppenderOptions<TAppender> options;
        public AppenderOptions<TAppender> Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }

        #endregion
    }

}
