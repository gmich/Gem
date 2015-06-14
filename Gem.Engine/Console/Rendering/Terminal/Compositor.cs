using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Infrastructure.Input;

namespace Gem.Console.Rendering.Terminal
{
    class Compositor
    {
        public void Compose()
        {
           SpriteFont font = null;
            var appender = new CellAppender((ch) =>
            {
                string content = ch.ToString();
                var size = font.MeasureString(content);
                return new Cell(content,(int)size.X,(int)size.Y);
            });

            var cellEntry = new TerminalEntry(appender, EntryWidth,maxRows: 3, historyEntries:20);
            
            var renderArea = new TerminalRenderArea(new TerminalRenderAreaSettings(), font);

        }

        private float EntryWidth()
        {
            return 60.0f;
        }
    }
}
