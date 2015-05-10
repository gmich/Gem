using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Gui.ScreenSystem
{
    internal class GuiHost : AnimatedGuiScreen
    {
        private readonly AggregationContext aggregationContext;
        private readonly IList<AControl> controls = new List<AControl>();
        private readonly IControlDrawable controlDrawable = Fluent.RenderControlBy.Frame;
        private readonly ITextDrawable textDrawable = Fluent.RenderTextBy.Position;

        public GuiHost(List<AControl> controls, AggregationContext aggregationContext)
            : base()
        {
            this.controls = controls;
            this.aggregationContext = aggregationContext;
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
                controlDrawable.Render(batch, control);

                if (control.Text != null)
                {
                    textDrawable.Render(batch, control.Text);
                }
            }
        }
    }

}
