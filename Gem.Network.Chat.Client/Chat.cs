using Gem.Network.Client;
using Gem.Network.Events;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Network.Chat.Client
{
    /// <summary>
    /// This is just an example to test Gem.Network
    /// </summary>
    public static class Chat
    {

        #region Fields

        private static Peer peer;
        private static GemClient client;
        private static string name;
        private static YoutubeSearch search;
        private static Dictionary<string, Func<string,bool>> CommandTable;

        #endregion

        #region Private Helpers

        private static void PrintIntroMessage()
        {
            Console.WriteLine(String.Format(
            @" 
 Command                       Description 
----------------------------------------------------
-help                          Show commands
-lock                          Stop appending
-unlock                        Continue appending
-gem <command>                 Gem console       
-cls                           Clear screen 
-clsx                          Clear screen and lock appending
-yt <keyword>                  Search youtube and retrieve video indexed
-yt <keyword> | <index>        Search youtube and retrieve video the number of videos specified
-play <index>                  Plays video by index
-setname <newname>             Change nickname  
-quit                          Quit  {0}", Environment.NewLine));

        }

        private static void ClientSetup()
        {
            // GemNetwork.ActiveProfile = "GemChat";

            GemNetworkDebugger.Echo = Console.WriteLine;
            //client = new GemClient("GemChat", "GemChat", "127.0.0.1", 14242, name);
            client = new GemClient("GemChat", "GemChat", "83.212.103.13", 14242, name);
        }


        private static void ProcessInput()
        {
            string input = string.Empty;
            while (input != "-quit")
            {
                input = Console.ReadLine();

                if (input.Length >= 1)
                {
                    if (input.StartsWith("-"))
                    {
                        if (Executecommand(input))
                        {
                            //if (input.StartsWith("-yt"))
                            //{
                            //    peer.SendCommand(input);
                            //}
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 1);
                        peer.Say(input);
                    }
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }
        }

        public static bool Executecommand(string cmd)
        {
            var cmdAndArgs = cmd.Split(' ');
            if (CommandTable.ContainsKey(cmdAndArgs[0]))
            {
                return CommandTable[cmdAndArgs[0]].Invoke(cmd);
            }
            else
            {
                Console.WriteLine("Unknown Command");
                return false;
            }
        }

        private static bool HasOnlyOneArgument(string input)
        {
            if (input.Split(' ').Count() == 1)
                return true;
            else
            {
                Console.WriteLine("Unknown command");
                return false;
            }
        }

        private static void RegisterCommands()
        {
            CommandTable.Add("-cls", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.Clear();
                    return true;
                }
                return false;
            });
            CommandTable.Add("-clsx", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.Clear();
                    peer.CanAppend = false;
                    return true;
                }
                return false;
            });
            CommandTable.Add("-help", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    PrintIntroMessage();
                    return true;
                }
                return false;
            });
            CommandTable.Add("-lock", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.WriteLine("[Locked]");
                    peer.CanAppend = false;
                    return true;
                }
                return false;
            });
            CommandTable.Add("-unlock", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.WriteLine("[Unlocked]");
                    peer.CanAppend = true;
                    return true;
                }
                return false;
            });
            CommandTable.Add("-gem", x =>
            {
                var cmd = x.Substring(5);
                if (cmd.Length > 0)
                {
                    client.SendCommand(cmd);
                    return true;
                }
                return false;
            });
            CommandTable.Add("-setname", x =>
            {
                var args = x.Split(' ');
                if (args.Count() >= 2)
                {
                    peer.ChangeName(args[1]);
                    return true;
                }
                else
                {
                    Console.WriteLine("Unknown Command");
                    return false;
                }
            });
            CommandTable.Add("-yt", x =>
                {
                    try
                    {
                        if (x.Split(' ').Count() < 2)
                        {
                            Console.WriteLine("Unknown Command");
                            return false;
                        }
                        var args = x.Substring(4);
                        if (x.Contains("|"))
                        {
                            var index = x.Substring(x.IndexOf('|') + 1);
                            int indexPos;
                            if (Int32.TryParse(index,out indexPos))
                            {
                                var breakIndex = args.IndexOf("|");
                                if (breakIndex > 0)
                                {
                                    args = args.Substring(0, breakIndex);
                                }
                                peer.Send(String.Format("[ {0} searched for {1} ]", peer.Name, args));
                                search.Run(args, indexPos).Wait();
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid index");
                                return true;
                            }
                        }
                        else
                        {
                            peer.Send(String.Format("[ {0} searched for {1} ]", peer.Name,args));
                            search.Run(args, 5).Wait();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        return false;
                    }
                });

            CommandTable.Add("-play", x =>
            {
                var args = x.Split(' ');
                if (args.Count() == 2)
                {
                    int index;
                    if (Int32.TryParse(args[1], out index))
                    {
                        string title;
                        if(search.Play(index,out title))
                        {
                            peer.Send(String.Format("[ {0} is watching {1} ] ",peer.Name,title));
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid index");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Unknown Command");
                    return false;
                }
            });
        }

        #endregion

        static void Main(string[] args)
        {
            CommandTable = new Dictionary<string, Func<string, bool>>();
            RegisterCommands();
              
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            //Pick a name
            Console.WriteLine("Your nickname : ");
            name = Console.ReadLine();

            Console.WriteLine("Password : ");
            string pwd = Console.ReadLine();

            ClientSetup();

            PrintIntroMessage();

            client.RunAsync(() => new ConnectionApprovalMessage { Message = "Incoming client", Sender = name, Password = pwd });

            //Initialize a chat peer
            peer = new Peer(name);
            search = new YoutubeSearch(peer.QueueMessage);

            GemNetworkDebugger.Echo = peer.QueueMessage;

            ProcessInput();

            peer.SayGoodBye();

            client.Dispose();

            Console.ReadLine();
        }

        #region Override closing event

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        private static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                    return false;
                case CtrlType.CTRL_LOGOFF_EVENT:
                    return false;
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                    if (peer != null)
                    {
                        peer.SayGoodBye();
                        client.Dispose();
                    }
                    return true;
                case CtrlType.CTRL_CLOSE_EVENT:
                    if (peer != null)
                    {
                        peer.SayGoodBye();
                        client.Dispose();
                    }
                    return true;
                default:
                    return false;
            }
        }

        #endregion

    }
}
