using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gem.Infrastructure.Input;
using Microsoft.Xna.Framework;

namespace Gem.Console.Rendering
{
    public class GemConsole : DrawableGameComponent
    {
        private readonly CellAligner aligner;
        private readonly CellAppender appender;
        private SpriteBatch batch;

        public GemConsole(Game game,SpriteFont font):base(game)
        {
            appender = new CellAppender((ch) =>
            {
                string content = ch.ToString();
                var size = font.MeasureString(content);
                return new Cell(content, (int)size.X, (int)size.Y, cell => appender.CellBehavior.CreateEffect(cell));
            }, new CellBehavior(Color.Black, 0.0f, 1.0f));

            aligner = new CellAligner();
            var cellEntry = new TerminalEntry(appender, aligner, () => 1, () => 3.0f);

            var historyRenderArea = new TerminalRenderArea(new CellRenderingOptions(), font);
            Terminal terminal = new Terminal(TerminalSettings.Default);
        }


        #region DrawableGameComponent Members

        public override void Initialize()
        {
            batch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
        

    }
}
