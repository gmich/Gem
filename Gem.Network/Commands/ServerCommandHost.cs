using Gem.Network.Messages;
using Gem.Network.Server;
using Gem.Network.Utilities.Loggers;
using Lidgren.Network;
using Seterlund.CodeGuard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
                var serverNotification = new Notification(msg, NotificationType.Message);
                MessageSerializer.Encode(serverNotification, ref om);
                GemNetworkDebugger.Echo(msg);
                commandHost.SendOnlyTo(om, connection);
            };

            RegisterHelpComand();
            RegisterSetPasswordCommand();
            RegisterEchoCommand();
            RegisterKickCommand();
            RegisterViewClientsCommand();

            password = "gem";

        }

        #endregion


        #region Commands

        public void SetPassword(string newPassword)
        {
            this.password = newPassword;
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
                executioners.Peek().ExecuteCommand(sender, command);
                return;
            }
            if (sender != null)
            {
                GemNetworkDebugger.Echo(sender + " sent: " + command);
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
                    if (cmd.requiresAuthentication && sender!=null)
                    {
                        if (args.Count > 0)
                        {
                            if (args[0] == password)
                            {
                                args.RemoveAt(0);
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
                        Echo(line,sender);
                }
            }
            else
            {
                Echo("Unknown Command", sender);
            }
        }

        #endregion


        #region Helpers

        private string GetConnectionInfo(NetConnection connection)
        {
            if (connection == null)
            {
                return "Server";
            }
            else
            {
                return connection.ToString();
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


        #region Common Commands

        private void RegisterEchoCommand()
        {
            RegisterCommand("echo", false, "Echo message",
                (Server, connection, command, arguments)
                => Server.NotifyAll(String.Format("[Server] {0}", command.Substring(4))));
        }

        private void RegisterViewClientsCommand()
        {
            RegisterCommand("showall", true, "Show all the connected clients",
                (Server, connection, command, arguments) =>
                {
                    var listOfIps = String.Join(Environment.NewLine, Server.ClientsIP.Select(x => x.ToString()));
                    Server.NotifyOnly(listOfIps, connection);
                });
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

                Echo(String.Format("{0} Commands marked with -pwd require password {0}", Environment.NewLine), netConnection);
                Echo(String.Format(fmt, "Commmand", "Description"), netConnection);
                Echo("---------------------------------", netConnection);
                foreach (CommandInfo cmd in commandTable.Values)
                {
                    var commmandMessage = cmd.command;

                    if (cmd.requiresAuthentication)
                    {
                        commmandMessage += " -pwd";
                    }
                    Echo(String.Format(fmt, commmandMessage, cmd.description), netConnection);
                }
                Echo(Environment.NewLine, netConnection);
            });
        }

        private void RegisterKickCommand()
        {
            RegisterCommand("kick", true, "Kick player -[IP:Port] reason",
            (host, netConnection, command, args) =>
            {
                string reason = string.Empty;
                if (args.Count > 1)
                {
                    reason = args[1];
                }
                if (args.Count > 0)
                {
                    var ipAndPort = args[0].Split(':');
                    int port = Int32.Parse(ipAndPort[1]);
                    if (ipAndPort.Length == 2)
                    {

                        if (host.Kick(new IPEndPoint(NetUtility.Resolve(ipAndPort[0]), port), reason))
                        {
                            host.NotifyOnly("Successfully kicked: " + args[1], netConnection);
                            GemNetworkDebugger.Echo(String.Format(" {0} was kicked by {1}", args[1], GetConnectionInfo(netConnection)));
                            return;
                        }
                    }
                }
                host.NotifyOnly("Unable to execute command", netConnection);

            });
        }

        private void RegisterSetPasswordCommand()
        {
            RegisterCommand("setpwd", true, "Set new password",
            (host, netConnection, command, args) =>
            {
                if (args.Count > 0)
                {
                    SetPassword(args[0]);
                    host.NotifyOnly("New password: " + password, netConnection);
                    GemNetworkDebugger.Echo(String.Format("New password: [ {0} ] by {1}", password, GetConnectionInfo(netConnection)));
                }
                else
                {
                    host.NotifyOnly("Problem occured", netConnection);
                }
            });
        }

        #endregion
    }
}
