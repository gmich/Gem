using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Infrastructure.Attributes;
using Gem.Console.Commands;
using System.Reflection;
using Gem.Infrastructure.Functional;

namespace Gem.Console
{

    public class Terminal : ICommandHost
    {

        #region Helper Classes

        internal class ExecutionGraphEntry
        {
            private readonly IList<string> arguments = new List<string>();
            private readonly CommandCallback cmd;
            private readonly CommandCallback rollback;

            public ExecutionGraphEntry(CommandCallback cmd, CommandCallback rollback, IList<string> arguments)
            {
                this.arguments = arguments;
                this.cmd = cmd;
                this.rollback = rollback;
            }

            public IList<string> Arguments { get { return arguments; } }
            public CommandCallback Callback { get { return cmd; } }
            public CommandCallback Rollback { get { return rollback; } }
        }

        public class CommandTable
        {
            private readonly List<CommandTable> subCommands = new List<CommandTable>();
            private readonly CommandCallback callback;
            private readonly string command;
            private readonly string description;
            private readonly bool requiresAuthorization;

            public CommandTable(CommandCallback callback, string command, string description, bool requiresAuthorization)
            {
                this.command = command;
                this.description = description;
                this.callback = callback;
                this.requiresAuthorization = requiresAuthorization;
                Rollback = null;
            }

            public bool AddSubCommand(CommandTable subCommand)
            {
                if (!subCommands.Any(arg => arg.command == subCommand.Command))
                {
                    subCommands.Add(subCommand);
                    return true;
                }
                return false;
            }

            public IEnumerable<CommandTable> SubCommand { get { return subCommands; } }
            public CommandCallback Callback { get { return callback; } }
            public CommandCallback Rollback { get; set; }
            public string Command { get { return command; } }
            public string Description { get { return description; } }
            public bool RequiresAuthorization { get { return requiresAuthorization; } }

        }

        internal class CommandCacheEntry<TEntry>
        {
            private readonly string command;
            private readonly List<TEntry> cachedCommands = new List<TEntry>();

            public CommandCacheEntry(string command, TEntry cachedCommand)
            {
                this.command = command;
                cachedCommands.Add(cachedCommand);
            }

            public void AddEntry(TEntry cachedCommand)
            {
                cachedCommands.Add(cachedCommand);
            }

            public string Command { get { return command; } }
            public IEnumerable<TEntry> Entries { get { return cachedCommands; } }
        }

        #endregion

        #region Fields

        private readonly List<CommandTable> commandTable = new List<CommandTable>();
        private readonly List<CommandCacheEntry<CommandTable>> subCommandCache = new List<CommandCacheEntry<CommandTable>>();
        private readonly List<CommandCacheEntry<CommandCallback>> rollbackCache = new List<CommandCacheEntry<CommandCallback>>();
        #endregion

        #region To Be Removed

        public List<CommandTable> Commands { get { return commandTable; } }

        #endregion

        #region Commands

        public void RegisterCommand<TObject>(TObject objectWithCommand)
            where TObject : class
        {
            ResolveAttributeToCommand<CommandAttribute>(objectWithCommand, (callback, attribute) =>
                                                                         RegisterCommand(callback,
                                                                                         attribute.Command,
                                                                                         attribute.Description,
                                                                                         attribute.RequiresAuthorization));

            ResolveAttributeToCommand<SubcommandAttribute>(objectWithCommand, (callback, attribute) =>
                                                                         RegisterSubCommand(callback,
                                                                                         attribute.ParentCommand,
                                                                                         attribute.SubCommand,
                                                                                         attribute.Description,
                                                                                         attribute.RequiresAuthorization));

            ResolveAttributeToCommand<RollbackCommandAttribute>(objectWithCommand, (callback, attribute) =>
                                                                       RegisterRollback(callback,
                                                                                        attribute.Command));


        }

        private void ResolveAttributeToCommand<TAttribute>(object objectWithCommand, Action<CommandCallback, TAttribute> action)
            where TAttribute : Attribute
        {
            var methods = objectWithCommand.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                      .Where(m => m.GetCustomAttributes(typeof(TAttribute), false).Length > 0);
            foreach (var methodInfo in methods)
            {
                CommandCallback callback;
                try
                {
                    callback = (CommandCallback)Delegate.CreateDelegate(typeof(CommandCallback), objectWithCommand, methodInfo, true);
                    AttributeResolver.Find<TAttribute>(methodInfo, attribute => action(callback, attribute));
                }
                catch (Exception ex)
                {
                    this.Error("Failed to much delegate with CommandAttribute in {0} with the CommandCallback delegate. {1}", objectWithCommand, ex.Message);
                }

            }

        }

        public void RegisterCommand(CommandCallback callback, string command, string description, bool requiresAuthorization)
        {
            if (!commandTable.Any(entry => entry.Command == command))
            {
                commandTable.Add(new CommandTable(callback, command, description, requiresAuthorization));
            }
            this.Info("registration result");
            CheckSubcommandCache(command);
            CheckRollbackCache(command);
        }

        //A subcommand might be registered before its parent. To avoid losing the subcommand , cache them 
        //and every time a new command is registered, check the cache.
        private void CheckSubcommandCache(string parent)
        {
            for (int i = 0; i < subCommandCache.Count; i++)
            {
                if (subCommandCache[i].Command == parent)
                {
                    var cachedEntry = subCommandCache[i];
                    subCommandCache.RemoveAt(i);

                    foreach (var entry in cachedEntry.Entries)
                    {
                        RegisterSubCommand(entry.Callback, parent, entry.Command, entry.Description, entry.RequiresAuthorization);
                    }
                }
            }
        }

        private void CheckRollbackCache(string parent)
        {
            for (int i = 0; i < rollbackCache.Count; i++)
            {
                if (rollbackCache[i].Command == parent)
                {
                    var cachedEntry = rollbackCache[i];
                    subCommandCache.RemoveAt(i);

                    CommandCallback callback = null;
                    foreach (var entry in cachedEntry.Entries)
                    {
                        callback += entry;
                    }

                    RegisterRollback(callback, parent);
                }
            }
        }

        public void RegisterSubCommand(CommandCallback callback, string parentCommand, string subCommand, string description, bool requiresAuthorization)
        {
            var command = commandTable.FirstOrDefault(entry => entry.Command == parentCommand);

            if (command != null)
            {
                if (!command.SubCommand.Any(entry => entry.Command == subCommand))
                {
                    command.AddSubCommand(new CommandTable(callback, subCommand, description, requiresAuthorization));
                }
            }
            else
            {
                subCommandCache.Add(new CommandCacheEntry<CommandTable>(parentCommand, new CommandTable(callback, subCommand, description, requiresAuthorization)));
            }

            this.Info("registration result");
        }

        public void RegisterRollback(CommandCallback callback, string parentCommand)
        {
            var command = commandTable.FirstOrDefault(entry => entry.Command == parentCommand);

            if (command != null)
            {
                command.Rollback += callback;
            }
            else
            {
                rollbackCache.Add(new CommandCacheEntry<CommandCallback>(parentCommand, callback));
            }

            this.Info("registration result");
        }

        public void UnregisterCommand(string command)
        {
            var entry = commandTable.Where(x => x.Command == command).FirstOrDefault();
            if (entry != null)
            {
                commandTable.Remove(entry);
            }
        }

        #endregion

        public void PushExecutioner(ICommandExecutioner executioner)
        {
            throw new NotImplementedException();
        }

        public void PopExecutioner()
        {
            throw new NotImplementedException();
        }

        #region Message Appending

        public void RemoveAppenders()
        {
            throw new NotImplementedException();
        }

        public void Message(string message)
        {
            throw new NotImplementedException();
        }

        public void Message(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Info(string message)
        {
            //throw new NotImplementedException();
        }

        public void Info(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message)
        {
            throw new NotImplementedException();
        }

        public void Debug(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Error(string message)
        {
            throw new NotImplementedException();
        }

        public void Error(string message, params object[] args)
        {
            // throw new NotImplementedException();
        }

        public void Fatal(string message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string message, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void RegisterAppender(Infrastructure.Logging.IAppender appender)
        {
            throw new NotImplementedException();
        }

        public void DeregisterAppender(Infrastructure.Logging.IAppender appender)
        {
            throw new NotImplementedException();
        }

        #endregion

        private readonly char commandSeparator = '|';
        private readonly char subCommandSeparator = '>';
        private readonly char argumentSeparator = ' ';

        public Result<object> ExecuteCommand(string command)
        {
            Result<object> result = Result.Successful(null);
            List<ExecutionGraphEntry> executionGraph = new List<ExecutionGraphEntry>();
            var commands = command.Split(commandSeparator);

            //loop through all the commands
            for (int commandIteration = 0; commandIteration < commands.Count(); commandIteration++)
            {
                var trimmed = commands[commandIteration].TrimStart(argumentSeparator).TrimEnd(argumentSeparator);
                var subCommands = commands[commandIteration].Split(subCommandSeparator);
                string currentCommand = string.Empty;
                CommandTable commandEntry = null;

                //var commandWithArguments = trimmed.Split(argumentSeparator);
                //if the command string is not long enough, fail
                //split the command to its subcommands

                for (int subCommandIteration = 0; subCommandIteration < subCommands.Count(); subCommandIteration++)
                {
                    var trimmedSub = subCommands[subCommandIteration].TrimStart(argumentSeparator).TrimEnd(argumentSeparator);
                    var subCommandWithArguments = trimmedSub.Split(argumentSeparator);

                    if (subCommandWithArguments.Count() == 0) return Result.Failed("Invalid Command");
                    if (subCommandIteration == 0)
                    {
                        currentCommand = subCommandWithArguments[0];
                        commandEntry = commandTable.Where(entry => entry.Command == currentCommand).FirstOrDefault();
                        if (commandEntry != null)
                        {
                            executionGraph.Add(new ExecutionGraphEntry(commandEntry.Callback, commandEntry.Rollback, subCommandWithArguments.Skip(1).ToList()));
                        }
                        //if the commandtable doesn't have a command match, fail
                        else
                        {
                            return Result.Failed("Invalid Command");
                        }
                        continue;
                    }
                    //if the subcommand string is not long enough, fail
                    if (subCommandWithArguments.Count() == 0) return Result.Failed("Invalid Command");

                    commandEntry = commandTable.Where(entry => entry.Command == currentCommand)
                                                     .SelectMany(x => x.SubCommand)
                                                     .Where(cmd => cmd.Command == subCommandWithArguments[0])
                                                     .FirstOrDefault();
                    if (commandEntry != null)
                    {
                        executionGraph.Add(new ExecutionGraphEntry(commandEntry.Callback, null, subCommandWithArguments.Skip(1).ToList()));
                    }
                    //if no entries were found, fail
                    else
                    {
                        return Result.Failed("Invalid Command");
                    }
                }
            }

            //execute the graph
            Stack<ExecutionGraphEntry> callstack = new Stack<ExecutionGraphEntry>();
            foreach (var entry in executionGraph)
            {
                if (!result.Failure)
                {
                    callstack.Push(entry);
                    result = entry.Callback(this, command, entry.Arguments, result.Value);
                }
                else
                {
                    //fallback
                    foreach (var executedCommand in callstack)
                    {
                        if (executedCommand.Rollback != null)
                            result = executedCommand.Rollback(this, command, entry.Arguments, result.Value);
                    }
                    return Result.Failed("Command execution failed " + result.Error, result.Value);
                }
            }
            return result;
        }

    }
}
