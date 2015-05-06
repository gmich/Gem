using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Gem.Gui
{
    public class GuiHost : DrawableGameComponent
    {
        private readonly AggregationContext aggregationContext;

        private readonly List<AControl> controls = new List<AControl>();

        private readonly AGuiRenderer renderer;

        public GuiHost(Game game):base(game)
        {  }

        public override void Initialize()
        {
            base.Initialize();

        }

        public AControl this[int id]
        {
            get
            {
                return controls[id];
            }
        }

        public int ComponentCount
        {
            get { return controls.Count; }
        }

        public IEnumerable<AControl> Entries()
        {
            foreach (var control in controls)
            {
                yield return control;
            }
        }

        public override void Update(GameTime gameTime)
        {
            aggregationContext.Aggregate();

            foreach(var control in controls)
            {
                control.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var control in controls)
            {
                renderer.Render(control);
            }
        }
    }
    
}
