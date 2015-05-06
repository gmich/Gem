using Gem.Gui.Controls;
using Gem.Gui.Factories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Gem.Gui
{
    public class GemGui 
    {

        public GemGui(Game game)
        {
            //add gui hosts
            //resolve control factory
            hosts.ForEach(host => game.Components.Add(host as GameComponent));
        }
        private List<GuiHost> hosts = new List<GuiHost>();
              
        private static IControlFactory controlFactory;

        public static Button Button
        {
            get { return controlFactory.CreateButton(); }
        }
        
        //TODO: add the rest controls

    }
}