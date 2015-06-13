#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Gem.Infrastructure.Attributes;
using Gem.Console.Commands;
using System.Reflection;
using Gem.Infrastructure.Functional;
using Gem.Infrastructure.Logging;
using System.Threading.Tasks;

#endregion

namespace Gem.Console
{

    public class Terminal : ICommandHost
    {

        #region Fields

        private readonly char commandSeparator;
        private readonly char subCommandSeparator;
        private readonly char argumentSeparator;

        private readonly List<CommandTable> commandTable = new List<CommandTable>();
        private readonly List<CommandCacheEntry<CommandTable>> subCommandCache = new List<CommandCacheEntry<CommandTable>>();
        private readonly List<CommandCacheEntry<CommandCallback>> rollbackCache = new List<CommandCacheEntry<CommandCallback>>();
        private readonly Stack<ICommandExecutioner> executioners = new Stack<ICommandExecutioner>();
        private readonly List<IAppender> appenders = new List<IAppender>();
        private readonly CommandHistory history;

        #endregion

        #region Ctor

        public Terminal(TerminalSettings settings)
        {
            history = new CommandHistory(settings.HistoryEntries);
            commandSeparator = settings.CommandSeparator;
            subCommandSeparator = settings.SubCommandSeparator;
            argumentSeparator = settings.ArgumentSeparator;
        }

        #endregion

        #region Public Properties

        public IEnumerable<CommandTable> Commands { get { return commandTable; } }

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

        #region Executioners

        public void PushExecutioner(ICommandExecutioner executioner)
        {
            executioners.Push(executioner);
        }

        public void PopExecutioner()
        {
            executioners.Pop();
        }

        private Task<Result<object>> CallExecutioner(string command)
        {
            return executioners.Peek().ExecuteCommand(command);
        }

        #endregion

        #region Message Appending

        public void RemoveAppenders()
        {
            appenders.Clear();
        }

        public void RegisterAppender(IAppender appender)
        {
            appenders.Add(appender);
        }

        public void DeregisterAppender(IAppender appender)
        {
            appenders.Remove(appender);
        }

        public void Message(string message)
        {
            appenders.ForEach(x => x.Message(message));
        }

        public void Message(string message, params object[] args)
        {
            appenders.ForEach(x => x.Message(message, args));
        }

        public void Info(string message)
        {
            appenders.ForEach(x => x.Info(message));
        }

        public void Info(string message, params object[] args)
        {
            appenders.ForEach(x => x.Info(message, args));
        }

        public void Debug(string message)
        {
            appenders.ForEach(x => x.Debug(message));
        }

        public void Debug(string message, params object[] args)
        {
            appenders.ForEach(x => x.Debug(message, args));
        }

        public void Warn(string message)
        {
            appenders.ForEach(x => x.Warn(message));
        }

        public void Warn(string message, params object[] args)
        {
            appenders.ForEach(x => x.Warn(message, args));
        }

        public void Error(string message)
        {
            appenders.ForEach(x => x.Error(message));
        }

        public void Error(string message, params object[] args)
        {
            appenders.ForEach(x => x.Error(message, args));
        }

        public void Fatal(string message)
        {
            appenders.ForEach(x => x.Fatal(message));
        }

        public void Fatal(string message, params object[] args)
        {
            appenders.ForEach(x => x.Fatal(message, args));
        }

        #endregion

        #region Command Execution

        private Task<Result<object>> ExecutionFailed(string error)
        {
            return new Task<Result<object>>(() => Result.Failed("Invalid Command"));
        }

        public Task<Result<object>> ExecuteCommand(string command)
        {
            if (executioners.Count != 0)
            {
                return CallExecutioner(command);
            }

            var result = Result.Successful(null);
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
                //split the command to its0 subcommands
                for (int subCommandIteration = 0; subCommandIteration < subCommands.Count(); subCommandIteration++)
                {
                    var trimmedSub = subCommands[subCommandIteration].TrimStart(argumentSeparator).TrimEnd(argumentSeparator);
                    var subCommandWithArguments = trimmedSub.Split(argumentSeparator);

                    if (subCommandWithArguments.Count() == 0) return ExecutionFailed("Invalid Command");
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
                            return ExecutionFailed("Invalid Command");
                        }
                        continue;
                    }
                    //if the subcommand string is not long enough, fail
                    if (subCommandWithArguments.Count() == 0) return ExecutionFailed("Invalid Command");

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
                        return ExecutionFailed("Invalid Command");
                    }
                }
            }
            
            //execute the graph
            return Task<Result<object>>.Factory.StartNew(() =>
                {
                    Stack<ExecutionGraphEntry> callstack = new Stack<ExecutionGraphEntry>();
                    Result<object> executionResult = Result.Ok<object>(null);
                    foreach (var entry in executionGraph)
                    {
                        if (!executionResult.Failure)
                        {
                            callstack.Push(entry);
                            executionResult = entry.Callback(this, entry.Arguments, executionResult.Value);
                        }
                        else
                        {
                            //fallback
                            foreach (var executedCommand in callstack)
                            {
                                if (executedCommand.Rollback != null)
                                    executionResult = executedCommand.Rollback(this, entry.Arguments, executionResult.Value);
                            }
                            return Result.Failed("Command execution failed " + executionResult.Error, executionResult.Value);
                        }
                    }
                    return executionResult;
                });
        }

        #endregion

    }
}
