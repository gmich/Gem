using System;

namespace Gem.Network.Utilities.Loggers
{
    /// <summary>
    /// Appends information using an Action<string> method
    /// </summary>
    public class ActionAppender : IAppender
    {

        #region Readonly Fields

        private readonly Action<string> Echo;

        #endregion

        #region Appending Info

        private const char Separator = '-';
        private const string EchoFormat = "{0} {1} {2}";

        private static class Verbosity
        {
            internal const string Info = "Info";
            internal const string Debug = "Debug";
            internal const string Warn = "Warn";
            internal const string Error = "Error";
            internal const string Fatal = "Fatal";
        }

        #endregion

        #region Ctor

        public ActionAppender(Action<string> echo)
        {
            Echo = echo;
        }

        #endregion

        #region Append

        public void Write(string message)
        {
            Echo(message);
        }

        public void Info(string message)
        {
            Echo(FormatMessage(Verbosity.Info, message));
        }

        public void Info(string message, params object[] args)
        {
            Echo(FormatMessage(Verbosity.Info, message, args));
        }

        public void Debug(string message)
        {
            Echo(FormatMessage(Verbosity.Debug, message));
        }

        public void Debug(string message, params object[] args)
        {
            Echo(FormatMessage(Verbosity.Debug, message, args));
        }

        public void Warn(string message)
        {
            Echo(FormatMessage(Verbosity.Warn, message));
        }

        public void Warn(string message, params object[] args)
        {
            Echo(FormatMessage(Verbosity.Warn, message, args));
        }

        public void Error(string message)
        {
            Echo(FormatMessage(Verbosity.Error, message));
        }

        public void Error(string message, params object[] args)
        {
            Echo(FormatMessage(Verbosity.Error, message, args));
        }

        public void Fatal(string message)
        {
            Echo(FormatMessage(Verbosity.Fatal, message));
        }

        public void Fatal(string message, params object[] args)
        {
            Echo(FormatMessage(Verbosity.Fatal, message, args));
        }

        #endregion

        #region Formatting Helpers

        private string FormatMessage(string verbosity, string message, params object[] args)
        {
            return String.Format(FormatMessage(verbosity, message), args);
        }

        private string FormatMessage(string severity, string message)
        {
            return String.Format(EchoFormat, severity, Separator, message);
        }

        #endregion

    }
}
