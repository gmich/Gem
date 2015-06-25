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

        #region Fields

        private readonly CellAligner aligner;
        private readonly CellAppender appender;
        private readonly TerminalRenderArea cellEntryRenderArea;
        private readonly TerminalEntry entryPoint;
        private readonly Terminal terminal;
        private readonly KeyProcessor keyProcessor;
        private readonly CellRenderingOptions renderingOptions;

        private SpriteBatch batch;

        #endregion

        public GemConsole(Game game, SpriteFont font)
            : base(game)
        {
            renderingOptions = new CellRenderingOptions
            {
                AreaSize = new Vector2(100, 100),
                CellSpacing = 2,
                MaxRows = 5,
                Position = new Vector2(10, 10),
                RowSize = new Vector2(200, font.MeasureString("|").Y),
                RowSpacing = 2
            };

            keyProcessor = new KeyProcessor(TextAppenderHelper.Default);
            appender = new CellAppender((ch) =>
            {
                string content = ch.ToString();
                var size = font.MeasureString(content);
                return new Cell(content, (int)size.X, (int)size.Y, cell => appender.CellBehavior.CreateEffect(cell));
            }, new CellBehavior(Color.Black, 0.0f, 1.0f));

            aligner = new CellAligner();
            entryPoint = new TerminalEntry(appender, aligner, ()=>renderingOptions.CellSpacing,()=> renderingOptions.RowSize.X);
            aligner.RowAdded += (sender, args) => cellEntryRenderArea.AddCellRange(args.Row, args.RowIndex);
            aligner.Cleared += (sender, args) => cellEntryRenderArea.Clear();

            cellEntryRenderArea = new TerminalRenderArea(renderingOptions, font);

            terminal = new Terminal(TerminalSettings.Default);
            entryPoint.OnFlushedEntry += (sender, command) => terminal.ExecuteCommand(command);
            SubscribeEntryToKeyprocessor();
        }

        private void SubscribeEntryToKeyprocessor()
        {
            keyProcessor.KeyPressed += (sender, ch) => entryPoint.AddEntry(ch);
            keyProcessor.BackSpace += (sender, args) => entryPoint.DeleteEntry();
            keyProcessor.Delete += (sender, args) => entryPoint.DeleteEntryAfterCursor();
            keyProcessor.Left += (sender, args) => entryPoint.Cursor.Left();
            keyProcessor.Right += (sender, args) => entryPoint.Cursor.Right();
            keyProcessor.Up += (sender, args) => entryPoint.PeekNext();
            keyProcessor.Down += (sender, args) => entryPoint.PeekPrevious();
        }

        public CellRenderingOptions RenderingOptions { get { return renderingOptions; } }
        public CellAppender Appender { get { return appender; } }
        public Terminal Terminal { get { return terminal; } }

        #region DrawableGameComponent Members

        public override void Initialize()
        {
            batch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            keyProcessor.ProcessKeyInput(gameTime.ElapsedGameTime.TotalSeconds);
            cellEntryRenderArea.UpdateCursor(entryPoint.Cursor.Effect,
                                          aligner.Rows().Skip(entryPoint.Cursor.Row).FirstOrDefault(),
                                          entryPoint.Cursor.Row,
                                          entryPoint.Cursor.HeadInRow);

            cellEntryRenderArea.Update(gameTime);

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            cellEntryRenderArea.Draw(batch);
            base.Draw(gameTime);
        }

        #endregion


    }
}
