using Gem.CameraSystem;
using Gem.Engine.Console.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Engine.Console.Cells;

namespace Gem.Engine.Console.Rendering
{
    public class TerminalWindowRenderArea
    {

        #region Fields

        private readonly SpriteFont font;
        private Camera camera;
        private Vector2 screenPosition;

        public CellRenderingOptions AreaSettings { get; }
        private readonly List<List<ICell>> flushedEntries= new List<List<ICell>>();

        private readonly List<Tuple<ICell, Vector2>> renderedEntries = new List<Tuple<ICell, Vector2>>();

        #endregion


        #region Ctor

        public TerminalWindowRenderArea(CellRenderingOptions settings, Rectangle viewArea, SpriteFont font)
        {
            AreaSettings = settings;
            this.font = font;
            screenPosition = new Vector2(viewArea.X, viewArea.Y);
        }

        public void AddCellRange(IEnumerable<ICell> cellRange)
        {
            flushedEntries.Add(new List<ICell>());
            flushedEntries[flushedEntries.Count - 1].AddRange(cellRange);
            ArrangeEntries();
        }

        private void ArrangeEntries()
        {
            renderedEntries.Clear();

            for (int row = 0; row < flushedEntries.Count; row++)
            {
                ConvertToRenderedEntry(row, flushedEntries[row]);
            }
            //Arrange rows
        }

        private void ConvertToRenderedEntry(int row, IList<ICell> entry)
        {
            float currentRowWidth = 0.0f;
            float y = row * (AreaSettings.RowSpacing + AreaSettings.RowHeight);
            var reconstructedEntry = new List<Tuple<ICell, Vector2>>();
            foreach (var cell in entry)
            {
                var charWidth = font.MeasureString(cell.Content.ToString()).X;
                currentRowWidth += charWidth;
                if (currentRowWidth > AreaSettings.RowWidth)
                {
                    renderedEntries.AddRange(reconstructedEntry);
                    reconstructedEntry.Clear();
                    currentRowWidth = charWidth;
                }
                reconstructedEntry.Add(new Tuple<ICell, Vector2>(
                    cell,
                    new Vector2(screenPosition.X + currentRowWidth - charWidth, y)));
            }
            renderedEntries.AddRange(reconstructedEntry);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (var entry in renderedEntries)
            {
                entry.Item1.Render(batch, entry.Item2);
            }
            batch.End();
        }


        #endregion

    }
}
