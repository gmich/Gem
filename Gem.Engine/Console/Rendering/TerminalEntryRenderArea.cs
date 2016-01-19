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

namespace Gem.Engine.Console.Rendering
{

    public class CellRenderingOptions
    {
        public int CellSpacing { get; set; }
        public int RowSpacing { get; set; }
        public int RowHeight { get; set; }
        public int RowWidth { get; set; }
        public int MaxRows { get; set; }
    }

    public class TerminalEntryRenderArea
    {

        #region Fields

        private readonly SpriteFont font;
        private readonly Dictionary<int, List<Tuple<Vector2, ICell>>> cellEffects = new Dictionary<int, List<Tuple<Vector2, ICell>>>();
        private readonly CellRenderingOptions areaSettings;

        private Behavior<IEffect> cursorEffect;
        private IEffect cursorRenderEffect;

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
            cellEffects.Clear();
        }

        public void AddCellRange(Row row, int rowIndex)
        {
            appendLocation = new Vector2(0, (rowIndex) * (rowSize + AreaSettings.RowSpacing));
            cellEffects.Add(rowIndex, new List<Tuple<Vector2, ICell>>());

            foreach (var entry in row.Entries)
            {
                var entryWidth = font.MeasureString(entry.Content.ToString()).X;
                float x = screenPosition.X + appendLocation.X - camera.Position.X + (entryWidth / 2 + 1);
                float y = screenPosition.Y + appendLocation.Y - camera.Position.Y;
                cellEffects[rowIndex].Add(new Tuple<Vector2, ICell>(new Vector2(x, y), entry));
                appendLocation.X += (entryWidth + AreaSettings.CellSpacing);
            }
        }

        public void UpdateCursor(Behavior<IEffect> behavior, Row currentRow, int row, int position)
        {
            int rowIndex = row;
            appendLocation = new Vector2(0, (rowIndex) * (rowSize + AreaSettings.RowSpacing) - 1);
            int pos = position - 1;
            if (cellEffects.ContainsKey(rowIndex) && pos > -1)
            {
                foreach (var entry in currentRow.Entries.Take(pos))
                {
                    var entryWidth = font.MeasureString(entry.Content.ToString()).X;
                    appendLocation.X += (entryWidth + AreaSettings.CellSpacing);
                }
                if (pos != -1)
                {
                    var lastEntry = currentRow.Entries.Skip(pos).FirstOrDefault();
                    if (lastEntry != null)
                    {
                        var entryWidth = font.MeasureString(lastEntry.Content.ToString()).X;
                        appendLocation.X += entryWidth + 1;
                    }
                }
            }
            float x = screenPosition.X + appendLocation.X - camera.Position.X;
            float y = screenPosition.Y + appendLocation.Y - camera.Position.Y;
            cursorEffect = (behavior.At(Behavior.Create(ctx => x),
                                Behavior.Create(ctx => y)));
        }

        public void Update(GameTime gameTime)
        {
            if (cursorEffect != null)
            {
                cursorRenderEffect = cursorEffect.BehaviorFunc(new BehaviorContext((float)gameTime.ElapsedGameTime.TotalSeconds));
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            cursorRenderEffect?.Draw(font, batch, Vector2.Zero);

            foreach (var row in cellEffects)
            {
                foreach (var cell in row.Value)
                {
                    cell.Item2.Render(batch, cell.Item1);
                }
            }
            batch.End();
        }
    }
}
