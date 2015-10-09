using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Microsoft.Xna.Framework;
using Gem.Engine.Console.Rendering;
using Gem.Engine.Console.EntryPoint;
using Gem.Engine.Console.Cells;
using Gem.Engine.Console.Commands;
using Gem.Engine.ScreenSystem;
using Gem.Engine.Input;
using System;
using NullGuard;
using Gem.Engine.Logging;

namespace Gem.Engine.Console
{
    public class GemConsole : IGame
    {

        #region Fields

        public const string HostTag = "Gem_Console";

        private CellRowAligner aligner;
        private CellAppender appender;
        private TerminalEntryRenderArea cellEntryRenderArea;
        public TerminalEntry EntryPoint { get; private set; }
        private KeyProcessor keyProcessor;
        private CellRenderingOptions renderingOptions;
        private readonly Terminal terminal;
        private readonly KeyboardInput input;
        private readonly TerminalWindowRenderArea window;

        public Action<SpriteBatch, SpriteFont, Vector2, char> CellRenderAction;

        #endregion
        public Host Host
        {
            get;
        }

        public GemConsole(KeyboardInput input, Host host)
        {
            CellRenderAction = (batch, rfont, position, ch) =>
                batch.DrawString(rfont, ch.ToString(), position, Color.Black);

            Host = host;
            terminal = new Terminal(TerminalSettings.Default);
            this.input = input;
            var font = Host.Container.Fonts.Content.Load<SpriteFont>("Fonts/consoleFont");

            renderingOptions = new CellRenderingOptions
            {
                CellSpacing = 2,
                MaxRows = 5,
                RowHeight = (int)font.MeasureString("|").Y,
                RowSpacing = 2,
                RowWidth = 300
            };

            keyProcessor = new KeyProcessor(new TextAppenderHelper(input));
            appender = new CellAppender((ch) =>
            {
                string content = ch.ToString();
                var size = font.MeasureString(content);
                return new Cell(ch, (batch, location) => CellRenderAction(batch,font, location, ch));
            });

            aligner = new CellRowAligner(ch=> font.MeasureString(ch.ToString()));
            EntryPoint = new TerminalEntry(appender, aligner, () => renderingOptions.CellSpacing, () => Host.Device.Viewport.Width);
            aligner.RowAdded += (sender, args) => cellEntryRenderArea.AddCellRange(args.Row, args.RowIndex);
            aligner.Cleared += (sender, args) => cellEntryRenderArea.Clear();

            cellEntryRenderArea = new TerminalEntryRenderArea(renderingOptions, new Rectangle(10, 10, 100, 100), font);


            terminal.RegisterAppender(new ActionAppender(EntryPoint.AppendString));
            window = new TerminalWindowRenderArea(renderingOptions, new Rectangle(5, 5, 300, 300), font);
     
            SubscribeEntryToKeyprocessor();
        }

        public void SetConsoleViewArea(Rectangle viewArea)
        {

        }

        public void SetEntryPointToTerminalRatio(float ratio)
        {

        }

        private Rectangle ConsoleViewArea()
        {
            return new Rectangle();
        }

        private void SubscribeEntryToKeyprocessor()
        {
            keyProcessor.KeyPressed += (sender, ch) => EntryPoint.AddEntry(ch);
            keyProcessor.BackSpace += (sender, args) => EntryPoint.DeleteEntry();
            keyProcessor.Delete += (sender, args) => EntryPoint.DeleteEntryAfterCursor();
            keyProcessor.Left += (sender, args) => EntryPoint.Cursor.Left();
            keyProcessor.Right += (sender, args) => EntryPoint.Cursor.Right();
            keyProcessor.Up += (sender, args) => EntryPoint.PeekNext();
            keyProcessor.Down += (sender, args) => EntryPoint.PeekPrevious();
            keyProcessor.Insert += (sender, args) =>
            {             
                var entry = EntryPoint.Flush();
            };
            EntryPoint.OnFlushedEntry += (sender, command) =>
            {
                window.AddCellRange(appender.Cells());
                terminal.ExecuteCommand(command);
            };
        }
        public CellRenderingOptions RenderingOptions { get { return renderingOptions; } }
        public CellAppender Appender { get { return appender; } }
        public Terminal Terminal { get { return terminal; } }


        public void FixedUpdate(GameTime gameTime)
        {
            cellEntryRenderArea.UpdateCursor(EntryPoint.Cursor.Effect,
                                          aligner.Rows().Skip(EntryPoint.Cursor.Row).FirstOrDefault(),
                                          EntryPoint.Cursor.Row,
                                          EntryPoint.Cursor.HeadInRow);

            cellEntryRenderArea.Update(gameTime);
        }


        public void Draw(SpriteBatch batch)
        {
            cellEntryRenderArea.Draw(batch);
            window.Draw(batch);
        }

        public void HandleInput(InputManager inputManager, GameTime gameTime)
        {
            keyProcessor.ProcessKeyInput(gameTime.ElapsedGameTime.TotalSeconds);
        }

    }
}
