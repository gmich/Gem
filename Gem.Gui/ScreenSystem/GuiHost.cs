using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Gui.ScreenSystem
{
    public class GuiHost : AnimatedGuiScreen
    {
        private readonly AggregationContext aggregationContext; 

        private readonly List<AControl> controls = new List<AControl>();

        private readonly IControlDrawable controlRenderer;
        private readonly ITextDrawable textRenderer; 

        public GuiHost(List<AControl> controls):base()
        {
            this.controls = controls;

            //TODO: provide from ioc container
            this.aggregationContext = null;
            this.controlRenderer = null;
            this.textRenderer = null;
        }

        public static GuiHost Create(AControl[] controls)
        {
            return new GuiHost(controls.ToList());
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

        public override void HandleInput()
        {
            aggregationContext.Aggregate();
        }

         public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            foreach (var control in controls)
            {
                control.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public override void Draw(SpriteBatch batch)
        {
            foreach (var control in controls)
            {
                controlRenderer.Render(batch,control);
                textRenderer.Render(batch,control.Text);
            }
        }
    }

}
