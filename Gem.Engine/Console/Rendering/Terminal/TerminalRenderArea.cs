using Gem.CameraSystem;
using Gem.Console.Animations;
using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Console.Rendering.Terminal
{
    public class TerminalRenderAreaSettings
    {
        public float CellSpacing { get; set; }
        public float RowSpacing { get; set; }
        public Vector2 RowSize { get; set; }
        public int MaxRows { get; set; }
        public Vector2 position { get; set; }
        public Vector2 areaSize { get; set; }
    }

    internal class TerminalRenderArea
    {
        private readonly Camera camera;
        private readonly SpriteFont font;

        private readonly List<Behavior<IEffect>> effects = new List<Behavior<IEffect>>();
        private readonly List<IEffect> drawingEffects = new List<IEffect>();


        public TerminalRenderArea(TerminalRenderAreaSettings settings, SpriteFont font)
        {
            camera = new Camera(Vector2.Zero,
                                settings.areaSize,
                                new Vector2(settings.areaSize.X, settings.MaxRows * (settings.RowSpacing + settings.RowSize.Y)));
            this.font = font;
        }

        private void AddBehavior(Behavior<IEffect> effect)
        {
            effects.Add(effect);
        }

        public void ChangeRow()
        {

        }

        public void AddCellRange(IEnumerable<Row> rows)
        {

        }
        public void Update(GameTime gameTime)
        {
            drawingEffects.Clear();
            var context = new BehaviorContext((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var effect in effects)
            {
                drawingEffects.Add(effect.BehaviorFunc(context));
            }
        }

        public void Draw(SpriteBatch batch)
        {
        }
    }
}
