using Gem.Gui.Controls;
using Gem.Gui.Elements.Areas;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui
{
    public class GuiHost : GameComponent
    {
        private readonly IGuiArea area;
        private readonly AggregationToken token;
        private readonly RenderTarget2D renderTarget;
        private readonly DrawManager drawManager;
        
        public GuiHost(Game game, IGuiArea area, DrawManager drawManager)
            : base(game)
        {
            this.area = area;
            this.token = new AggregationToken();
            this.drawManager = drawManager;
            //renderTarget = new RenderTarget2D(drawManager.Device, area.Region.Frame.Width, area.Region.Frame.Height);
        }

    }

}
