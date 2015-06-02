using Microsoft.Xna.Framework;

namespace Gem.Diagnostics.Console
{
    /// <summary>
    /// DebugSystem is a helper class that streamlines the creation of the various GameDebug
    /// pieces. While games are free to add only the pieces they care about, DebugSystem allows
    /// games to quickly create and add all the components by calling the Initialize method.
    /// </summary>
    public class DebugSystem
    {
        private static DebugSystem singletonInstance;

        /// <summary>
        /// Gets the singleton instance of the debug system. You must call Initialize
        /// to create the instance.
        /// </summary>
        public static DebugSystem Instance
        {
            get { return singletonInstance; }
        }

        /// <summary>
        /// Gets the DebugManager for the system.
        /// </summary>
        public DebugManager DebugManager { get; private set; }

        /// <summary>
        /// Gets the DebugCommandUI for the system.
        /// </summary>
       // public DebugCommandUI DebugCommandUI { get; private set; }

        /// <summary>
        /// Gets the FpsCounter for the system.
        /// </summary>
        public FpsCounter FpsCounter { get; private set; }

        /// <summary>
        /// Gets the TimeRuler for the system.
        /// </summary>
        public TimeRuler TimeRuler { get; private set; }

        /// <summary>
        /// Initializes the DebugSystem and adds all components to the game's Components collection.
        /// </summary>
        /// <param name="game">The game using the DebugSystem.</param>
        /// <param name="debugFont">The font to use by the DebugSystem.</param>
        /// <returns>The DebugSystem for the game to use.</returns>
        public static DebugSystem Initialize(Game game, string debugFont)
        {
            // if the singleton exists, return that; we don't want two systems being created for a game
            if (singletonInstance != null)
                return singletonInstance;

            // Create the system
            singletonInstance = new DebugSystem();

            // Create all of the system components
            singletonInstance.DebugManager = new DebugManager(game, debugFont);
            game.Components.Add(singletonInstance.DebugManager);

            //singletonInstance.DebugCommandUI = new DebugCommandUI(game);
            //game.Components.Add(singletonInstance.DebugCommandUI);

            singletonInstance.FpsCounter = new FpsCounter(game);
            game.Components.Add(singletonInstance.FpsCounter);

            singletonInstance.TimeRuler = new TimeRuler(game);
            game.Components.Add(singletonInstance.TimeRuler);

            return singletonInstance;
        }

        // Private constructor; games should use Initialize
        private DebugSystem() { }
    }
}
