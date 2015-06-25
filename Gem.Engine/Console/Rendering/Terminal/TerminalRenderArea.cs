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

namespace Gem.Console.Rendering
{



    public class CellRenderingOptions
    {
        public int CellSpacing { get; set; }
        public int RowSpacing { get; set; }
        public Vector2 RowSize { get; set; }
        public int MaxRows { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 AreaSize { get; set; }
    }

    public class TerminalRenderArea
    {

        #region Fields

        private readonly Camera camera;
        private readonly SpriteFont font;
        private readonly List<Behavior<IEffect>> effects = new List<Behavior<IEffect>>();
        private readonly List<IEffect> drawingEffects = new List<IEffect>();

        private Vector2 appendLocation;

        #endregion

        #region Properties

        private CellRenderingOptions AreaSettings { get; set; }

        #endregion

        #region Ctor

        public TerminalRenderArea(CellRenderingOptions settings, SpriteFont font)
        {
            camera = new Camera(Vector2.Zero,
                                settings.AreaSize,
                                new Vector2(settings.AreaSize.X, settings.MaxRows * (settings.RowSpacing + settings.RowSize.Y)));
            this.font = font;
        }

        #endregion

        private void AddBehavior(Behavior<IEffect> effect)
        {
            effects.Add(effect);
        }

        public void AddCellRange(Row row, int rowIndex)
        {
            appendLocation = new Vector2(0, rowIndex * (AreaSettings.RowSize.Y + AreaSettings.RowSpacing));
            foreach (var entry in row.Entries)
            {
                AddBehavior(entry.Behavior.At(Behavior.Create(ctx => AreaSettings.Position.X + appendLocation.X - camera.Position.X),
                            Behavior.Create(ctx => AreaSettings.Position.Y + appendLocation.Y - camera.Position.Y)));
                appendLocation.X += (entry.SizeX + AreaSettings.CellSpacing);
            }
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
            foreach (var cellDrawing in drawingEffects)
            {
                cellDrawing.Draw(font, batch, Vector2.Zero);
            }
        }
    }
}
