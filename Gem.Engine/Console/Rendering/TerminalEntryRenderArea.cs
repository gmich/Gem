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
        public int MaxRows { get; set; }
    }

    public class TerminalEntryRenderArea
    {

        #region Fields

        private readonly SpriteFont font;
        private readonly Dictionary<int, List<Behavior<IEffect>>> effects = new Dictionary<int, List<Behavior<IEffect>>>();
        private readonly List<IEffect> drawingEffects = new List<IEffect>();
        private readonly CellRenderingOptions areaSettings;

        private Behavior<IEffect> cursorEffect;
        private Vector2 appendLocation;
        private Camera camera;

        private Vector2 screenPosition;
        private readonly float rowSize;

        #endregion

        #region Properties

        public CellRenderingOptions AreaSettings { get { return areaSettings; } }

        #endregion

        #region Ctor

        public TerminalEntryRenderArea(CellRenderingOptions settings, Rectangle viewArea, SpriteFont font)
        {
            Contract.Requires(settings != null);
            this.areaSettings = settings;
            this.font = font;
            this.rowSize = font.MeasureString("|").Y;
            SetViewingArea(viewArea);
        }

        #endregion

        public void SetViewingArea(Rectangle viewArea)
        {
            screenPosition = new Vector2(viewArea.Left, viewArea.Top);
            var cameraViewportSize = new Vector2(viewArea.Width, viewArea.Height);

            camera = new Camera(Vector2.Zero,
                               cameraViewportSize);
        }

        public void Clear()
        {
            effects.Clear();
        }

        public void AddCellRange(Row row, int rowIndex)
        {
            appendLocation = new Vector2(0, (rowIndex) * (rowSize + AreaSettings.RowSpacing));
            effects.Add(rowIndex, new List<Behavior<IEffect>>());

            foreach (var entry in row.Entries)
            {
                float x = screenPosition.X + appendLocation.X - camera.Position.X + (entry.SizeX / 2 + 1);
                float y = screenPosition.Y + appendLocation.Y - camera.Position.Y;
                effects[rowIndex].Add(entry.Behavior.At(Behavior.Create(ctx => x),
                            Behavior.Create(ctx => y)));
                appendLocation.X += (entry.SizeX + AreaSettings.CellSpacing);
            }
        }

        public void UpdateCursor(Behavior<IEffect> behavior, Row currentRow, int row, int position)
        {
            int rowIndex = row;
            appendLocation = new Vector2(0, (rowIndex) * (rowSize+ AreaSettings.RowSpacing) - 1);
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
                        appendLocation.X += lastEntry.SizeX + 1;
                    }
                }
            }
            float x = screenPosition.X + appendLocation.X - camera.Position.X;
            float y = screenPosition.Y + appendLocation.Y - camera.Position.Y;
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
