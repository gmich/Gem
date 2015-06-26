using Gem.CameraSystem;
using Gem.Engine.Console.Cells;
using Gem.Engine.Console.Rendering.Animations;
using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Engine.Console.Rendering
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

    public class TerminalEntryRenderArea
    {

        #region Fields

        private readonly Camera camera;
        private readonly SpriteFont font;
        private readonly Dictionary<int, List<Behavior<IEffect>>> effects = new Dictionary<int, List<Behavior<IEffect>>>();
        private readonly List<IEffect> drawingEffects = new List<IEffect>();
        private readonly CellRenderingOptions areaSettings;

        private Behavior<IEffect> cursorEffect;
        private Vector2 appendLocation;

        #endregion

        #region Properties

        public CellRenderingOptions AreaSettings { get { return areaSettings; } }

        #endregion

        #region Ctor

        public TerminalEntryRenderArea(CellRenderingOptions settings, SpriteFont font)
        {
            Contract.Requires(settings != null);
            this.areaSettings = settings;
            camera = new Camera(Vector2.Zero,
                                settings.AreaSize,
                                new Vector2(settings.AreaSize.X, settings.MaxRows * (settings.RowSpacing + settings.RowSize.Y)));
            this.font = font;
        }

        #endregion

        public void Clear()
        {
            effects.Clear();
        }

        public void AddCellRange(Row row, int rowIndex)
        {
            appendLocation = new Vector2(0, (rowIndex) * (AreaSettings.RowSize.Y + AreaSettings.RowSpacing));
            effects.Add(rowIndex, new List<Behavior<IEffect>>());

            foreach (var entry in row.Entries)
            {
                float x = AreaSettings.Position.X + appendLocation.X - camera.Position.X + (entry.SizeX/2+1);
                float y = AreaSettings.Position.Y + appendLocation.Y - camera.Position.Y;
                effects[rowIndex].Add(entry.Behavior.At(Behavior.Create(ctx => x),
                            Behavior.Create(ctx => y)));
                appendLocation.X += (entry.SizeX + AreaSettings.CellSpacing);
            }
        }

        public void UpdateCursor(Behavior<IEffect> behavior, Row currentRow, int row, int position)
        {
            int rowIndex = row;
            appendLocation = new Vector2(0, (rowIndex) * (AreaSettings.RowSize.Y + AreaSettings.RowSpacing) - 1);
            int pos = position - 1;
            if (effects.ContainsKey(rowIndex) && pos > -1)
            {
                foreach (var entry in currentRow.Entries.Take(pos))
                {
                    appendLocation.X += (entry.SizeX + AreaSettings.CellSpacing);
                }
                if (pos != -1)
                {
                    var lastEntry = currentRow.Entries.Skip(pos).FirstOrDefault();
                    if (lastEntry != null)
                    {
                        appendLocation.X += lastEntry.SizeX  + 1;
                    }
                }
            }
            float x = AreaSettings.Position.X + appendLocation.X - camera.Position.X;
            float y = AreaSettings.Position.Y + appendLocation.Y - camera.Position.Y;
            cursorEffect = (behavior.At(Behavior.Create(ctx => x),
                                  Behavior.Create(ctx => y)));

            System.Console.WriteLine(row + " " + position + " " + x + " " + y);
        }

        public void Update(GameTime gameTime)
        {

            drawingEffects.Clear();
            var context = new BehaviorContext((float)gameTime.ElapsedGameTime.TotalSeconds);

            foreach (var behaviors in effects.Values)
            {
                foreach (var effect in behaviors)
                    drawingEffects.Add(effect.BehaviorFunc(context));
            }
            if (cursorEffect != null)
                drawingEffects.Add(cursorEffect.BehaviorFunc(context));
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            foreach (var cellDrawing in drawingEffects)
            {
                cellDrawing.Draw(font, batch, Vector2.Zero);
            }

            batch.End();
        }
    }
}
