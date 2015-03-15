#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Gem.Network.Shooter.Client
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class ShooterClient
    {
        
        [STAThread]
        static void Main()
        {

            Console.WriteLine("Your nickname : ");
            var name = Console.ReadLine();

            using (var game = new ShooterGame(name))
                game.Run();
        }
    }
#endif
}
