using Gem.Network.Client;
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
    class Chat
    {

        #region Fields

        private static Peer peer;
        private static GemClient client;
        private static string name;

        private static Dictionary<string, Action<string>> CommandTable;

        #endregion

        #region Private Helpers

        private static void PrintIntroMessage()
        {
            Console.WriteLine(String.Format(
            @" 
 Command               Description 
-------------------------------------
-help                  Show commands
-lock                  Stop appending
-unlock                Continue appending
-gem <command>         Gem console       
-cls                   Clear screen 
-clsx                  Clear screen and lock appending
-setname <newname>     Change nickname  
-quit                  Quit  {0}", Environment.NewLine));

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
                        var cmdAndArgs = input.Split(' ');
                        if (CommandTable.ContainsKey(cmdAndArgs[0]))
                        {
                            CommandTable[cmdAndArgs[0]].Invoke(input);
                        }
                        else
                        {
                            Console.WriteLine("Unknown Command");
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
                    Console.Clear();
            });
            CommandTable.Add("-clsx", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.Clear();
                    peer.CanAppend = false;
                }
            });
            CommandTable.Add("-help", x =>
            {
                if (HasOnlyOneArgument(x))
                    PrintIntroMessage();
            });
            CommandTable.Add("-lock", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.WriteLine("[Locked]");
                    peer.CanAppend = false;
                }
            });
            CommandTable.Add("-unlock", x =>
            {
                if (HasOnlyOneArgument(x))
                {
                    Console.WriteLine("[Unlocked]");
                    peer.CanAppend = true;
                }
            });
            CommandTable.Add("-gem", x =>
            {
                var cmd = x.Substring(5);
                if (cmd.Length > 0)
                {
                    client.SendCommand(cmd);
                }
            });
            CommandTable.Add("-setname", x =>
            {
                var args = x.Split(' ');
                if (args.Count() >= 2)
                {
                    peer.ChangeName(args[1]);
                }
                else
                {
                    Console.WriteLine("Unknown Command");
                }
            });
        }

        #endregion

        static void Main(string[] args)
        {
            CommandTable = new Dictionary<string, Action<string>>();
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
                    peer.SayGoodBye();
                    client.Dispose();
                    return true;
                case CtrlType.CTRL_CLOSE_EVENT:
                    peer.SayGoodBye();
                    client.Dispose();
                    return true;
                default:
                    return false;
            }
        }

        #endregion

    }
}
