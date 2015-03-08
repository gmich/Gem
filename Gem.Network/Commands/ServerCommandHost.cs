using Gem.Network.Messages;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;

namespace Gem.Network.Commands
{
    public class ServerCommandHost : ICommandHost
    {    

        #region Private Fields

        /// Maximum command history entries
        const int MaxCommandHistory = 32;

        // Registered command executioners
        private Stack<ICommandExecutioner> executioners;

        // Registered commands
        private Dictionary<string, CommandInfo> commandTable;
        
        public Action<string, NetConnection> Echo;

        private readonly IDebugHost EchoToAll;

        private readonly IServer commandHost;

        private string password;

        #endregion


        #region Constructor

        public ServerCommandHost(IServer commandHost)
        {
            Guard.That(commandHost, "Command host must not be null").IsNotNull();

            this.commandHost = commandHost;
            executioners = new Stack<ICommandExecutioner>();
            commandTable = new Dictionary<string, CommandInfo>();

            EchoToAll = new DebugListener();
            EchoToAll.RegisterAppender(new ServerCommandAppender(commandHost));
            
            Echo = (msg, connection) =>
            {
                var om = commandHost.CreateMessage();
                var package = new ServerNotification(GemNetwork.NotificationByte,msg);
                var serverNotification = new ServerNotification(GemNetwork.NotificationByte, msg);
                MessageSerializer.Encode(serverNotification, ref om);
                GemNetworkDebugger.Echo(msg);
                commandHost.SendOnlyTo(om, connection);
            };

            RegisterHelpComand();

            RegisterEchoCommand();
            password = string.Empty;

        }

        #endregion


        #region Commands

        public void SetPassword(string newPassword)
        {
            this.password = newPassword;
        }

        private void RegisterEchoCommand()
        {
            GemServer.RegisterCommand("echo", "Echo message", false,
                (Server, connection, command, arguments)
                => Server.NotifyAll(command.Substring(4)));
        }

        private void RegisterHelpComand()
        {
            RegisterCommand("help", false, "Show command help",
            (host, netConnection, command, args) =>
            {
                int maxLen = 0;
                foreach (CommandInfo cmd in commandTable.Values)
                    maxLen = Math.Max(maxLen, cmd.command.Length);

                string fmt = String.Format("{{0,-{0}}}    {{1}}", maxLen);

                foreach (CommandInfo cmd in commandTable.Values)
                {
                    Echo(String.Format(fmt, cmd.command, cmd.description), netConnection);
                }
            });

            RegisterCommand("setpwd", true, "Set new password",
            (host, netConnection, command, args) =>
            {
                SetPassword(password);
            });
        }

        public void RegisterCommand(string command, bool requiresAuthorization, string description, CommandExecute callback)
        {
            string lowerCommand = command.ToLower();
            if (commandTable.ContainsKey(lowerCommand))
            {
                throw new InvalidOperationException(
                    String.Format("Command \"{0}\" is already registered.", command));
            }

            commandTable.Add(lowerCommand, new CommandInfo(command,requiresAuthorization, description, callback));
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

        public void ExecuteCommand(NetConnection sender, string command)
        {          
            if (executioners.Count != 0)
            {
                executioners.Peek().ExecuteCommand(sender,command);
                return;
            }

            char[] spaceChars = new char[] { ' ' };

            //EchoToAll.Write(command);

            command = command.TrimStart(spaceChars);

            List<string> args = new List<string>(command.Split(spaceChars));
            string cmdText = args[0];
            args.RemoveAt(0);

            CommandInfo cmd;
            if (commandTable.TryGetValue(cmdText.ToLower(), out cmd))
            {
                try
                {
                    if (cmd.requiresAuthentication)
                    {
                        if (args.Count > 0)
                        {
                            if (args[0] == password)
                            {
                                cmd.callback(commandHost, sender, command, args);
                            }
                            else
                            {
                                Echo("Wrong password", sender);
                            }
                        }
                    }
                    else
                    {
                        cmd.callback(commandHost, sender, command, args);
                    }
                }
                catch (Exception e)
                {
                    Echo("Unhandled Exception occurred", sender);

                    string[] lines = e.Message.Split(new char[] { '\n' });
                    foreach (string line in lines)
                        EchoToAll.Error(line);
                }
            }
            else
            {
                Echo("Unknown Command",sender);
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
            EchoToAll.RegisterAppender(appender);
        }

        public void DeregisterAppender(IAppender appender)
        {
            EchoToAll.DeregisterAppender(appender);
        }

        #endregion

    }
}
