using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gem.Engine.Logging
{
    /// <summary>
    /// Appends strings using an Action<string> delegate.
    /// FormatTemplate options:
    /// %Verbosity - The output verbosity
    /// %Date{pattern} - The DateTime.Now %Date{0} for default
    /// %Message - The output message
    /// %NewLine - Environment.NewLine
    /// </summary>
    public class ActionAppender : IAppender
    {

        #region Readonly Fields

        private readonly Action<string> Echo;
        private readonly string FormatTemplate;

        #endregion

        #region Appending Info

        private static class Verbosity
        {
            internal const string Info = "Info";
            internal const string Debug = "Debug";
            internal const string Warn = "Warn";
            internal const string Error = "Error";
            internal const string Fatal = "Fatal";
        }

        private string ApplyFormatTemplate(string message, string verbosity)
        {
            var sb = new StringBuilder();

            sb.Append(Formatter.Parse(FormatTemplate));
            sb.Replace("%Verbosity", verbosity);
            sb.Replace("%Message", message);
            sb.Replace("%NewLine", Environment.NewLine);

            return sb.ToString();
        }

        #endregion

        #region Formatting

        /// <summary>
        /// Immutable Token
        /// </summary>
        internal class FormattingRule
        {
            private readonly Regex pattern;
            /// <summary>
            /// Provide the new format by the pattern's match
            /// </summary>
            private readonly Func<string, string> replacementRule;

            internal FormattingRule(Regex pattern, Func<string, string> replacementRule)
            {
                this.pattern = pattern;
                this.replacementRule = replacementRule;
            }

            public string ApplyPattern(string message)
            {
                return pattern.Replace(message, match => replacementRule(match.Value));
            }
        }

        /// <summary>
        /// Applies the formatting rules
        /// </summary>
        internal class Formatter
        {
            internal static List<FormattingRule> rules
                  = new List<FormattingRule>
            {                
                 new FormattingRule(pattern: new Regex("%Date{.*}"),
                                    replacementRule: match => 
                                    String.Format(match.Replace("%Date", ""), DateTime.Now))                                               
            };

            internal static string Parse(string msg)
            {
                string result = msg;
                rules.ForEach(rule => result = rule.ApplyPattern(msg));

                return result;
            }
        }

        #endregion

        #region Ctor

        public ActionAppender(Action<string> echo, string formatTemplate = "[%Date{0:G}] %Verbosity - %Message")
        {
            if (formatTemplate == null)
            {
                throw new ArgumentNullException("formatTemplate");
            }
            FormatTemplate = formatTemplate;
            Echo = echo;
        }

        #endregion

        #region Append

        public void Message(string message)
        {
            Echo(message);
        }

        public void Message(string message, params object[] args)
        {
            Echo(string.Format(message, args));
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

        private string FormatMessage(string verbosity, string message)
        {
            return ApplyFormatTemplate(message, verbosity);
        }

        #endregion

    }

    internal static class ActionAppenderFormattingExtensions
    {
        /// <summary>
        /// Helper extension for adding formatting rules to the appender
        /// </summary>
        /// <param name="appender"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        internal static ActionAppender AddFormattingRules(this ActionAppender appender,
                                                        ActionAppender.FormattingRule[] tokens)
        {
            ActionAppender.Formatter.rules.AddRange(tokens);
            return appender;
        }
    }
}
