using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;

namespace Gem.Network.Commands
{
    public class Commander : ICommandHost
    {
        #region Private Fields

        /// Maximum command history entries
        const int MaxCommandHistory = 32;

        // Registered command executioners
        private Stack<ICommandExecutioner> executioners;

        // Registered commands
        private Dictionary<string, CommandInfo> commandTable;

        //Registered appender        
        private List<IAppender> appenders;
             
        private readonly IDebugHost echoer;

        #endregion

        #region Constructor

        public Commander(IDebugHost debugHost)
        {
            appenders = new List<IAppender>();
            executioners = new Stack<ICommandExecutioner>();
            commandTable = new Dictionary<string, CommandInfo>();
            echoer = debugHost;

            RegisterCommand("help", "Show command help",
            (host, command, args) =>
            {
                int maxLen = 0;
                foreach (CommandInfo cmd in commandTable.Values)
                    maxLen = Math.Max(maxLen, cmd.command.Length);

                string fmt = String.Format("{{0,-{0}}}    {{1}}", maxLen);

                foreach (CommandInfo cmd in commandTable.Values)
                {
                    echoer.Write(String.Format(fmt, cmd.command, cmd.description));
                }
            });
        }

        #endregion

        #region Commands

        public void RegisterCommand(string command, string description, CommandExecute callback)
        {
            string lowerCommand = command.ToLower();
            if (commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is already registered.", command));
            }

            commandTable.Add(lowerCommand, new CommandInfo(command, description, callback));
        }

        public void DeregisterCommand(string command)
        {
            string lowerCommand = command.ToLower();
            if (!commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(String.Format("Command \"{0}\" is not registered.", command));
            }

            commandTable.Remove(command);
        }

        public void ExecuteCommand(string command)
        {          
            if (executioners.Count != 0)
            {
                executioners.Peek().ExecuteCommand(command);
                return;
            }

            char[] spaceChars = new char[] { ' ' };

            echoer.Write(command);

            command = command.TrimStart(spaceChars);

            List<string> args = new List<string>(command.Split(spaceChars));
            string cmdText = args[0];
            args.RemoveAt(0);

            CommandInfo cmd;
            if (commandTable.TryGetValue(cmdText.ToLower(), out cmd))
            {
                try
                {
                    // Call registered command delegate.
                    cmd.callback(this, command, args);
                }
                catch (Exception e)
                {
                    echoer.Error("Unhandled Exception occurred");

                    string[] lines = e.Message.Split(new char[] { '\n' });
                    foreach (string line in lines)
                        echoer.Error(line);
                }
            }
            else
            {
                echoer.Write("Unknown Command");
            }
        }

        #endregion

        #region Register / Deregister

        public void PushExecutioner(ICommandExecutioner executioner)
        {
            executioners.Push(executioner);
        }

        public void PopExecutioner()
        {
            executioners.Pop();
        }

        public void RegisterAppender(IAppender appender)
        {
            echoer.RegisterAppender(appender);
        }

        public void DeregisterAppender(IAppender appender)
        {
            echoer.DeregisterAppender(appender);
        }

        #endregion
    }
}
