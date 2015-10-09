using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System;
using Gem.Engine.Configuration;
using Gem.Engine.Containers;
using Gem.Engine.ScreenSystem;
using System.Collections.Generic;
using Gem.Engine.Input;
using Gem.Engine.Physics;
using Gem.Engine.Console;
using Microsoft.Xna.Framework.Input;
using Gem.Engine.Console.Commands;
using Gem.Engine.Logging;

namespace Gem.Engine
{
    public class GameBase : Game
    {
        private readonly Dictionary<string, Func<IScreenHost>> hosts = new Dictionary<string, Func<IScreenHost>>();

        public event EventHandler<EventArgs> onInitialize;
        public event EventHandler<ContentManager> WhenReady;
        public event EventHandler<EventArgs> onContentUnload;
        public event EventHandler<SpriteBatch> onDraw;
        public event EventHandler<GameTime> OnUpdate;

        public GraphicsDeviceManager GraphicsManager { get; }
        public ScreenManager ScreenManager { get; private set; }
        public ContentContainer Container { get; private set; }
        public Settings Settings { get; }
        public InputManager Input { get; } = new InputManager();

        public static IDebugHost Reporter { get; } = new DebugHost();

        public bool IsConsoleEnabled { get; set; } = true;

        private GemConsole console;

        public IScreenHost this[string guiHostId]
        {
            get
            {
                if (hosts.ContainsKey(guiHostId))
                {
                    return hosts[guiHostId]();
                }
                else
                {
                    throw new ArgumentException("Host was not found");
                }
            }
        }

        public GameBase(int windowWidth, int windowHeight)
        {
            Settings = new Settings(this, new Vector2(windowWidth, windowHeight));
            GraphicsManager = new GraphicsDeviceManager(this);       
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Container = new ContentContainer(Content);
            console = AddHost(GemConsole.HostTag, host => new GemConsole(Input.Keyboard, host), new TimedTransition(TimeSpan.FromSeconds(0.5),
                                (state, progress, target, batch) =>
                                 batch.Draw(target,
                                Vector2.Zero,
                                 Color.White * progress))) as GemConsole;
            ScreenManager = new ScreenManager(this, console.Terminal, Input, Settings, (batch) => onDraw?.Invoke(this, batch));
            Reporter.RegisterAppender(new ActionAppender(console.EntryPoint.AddString));
            Components.Add(ScreenManager);
            onInitialize?.Invoke(this, EventArgs.Empty);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            WhenReady?.Invoke(this, Content);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            onContentUnload?.Invoke(this, EventArgs.Empty);
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Flush();

            if (IsConsoleEnabled)
            {
                if (Input.Keyboard.IsKeyClicked(Keys.Tab))
                {
                    if (IsShowing(GemConsole.HostTag))
                    {
                        Hide(GemConsole.HostTag);
                    }
                    else
                    {
                        Show(GemConsole.HostTag);
                    }
                }
            }
            OnUpdate?.Invoke(this, gameTime);
            base.Update(gameTime);
        }

        #region Public Methods

        public IPhysicsGame AddHost(string hostTag, Func<PhysicsHost, IPhysicsGame> gameGenerator, ITransition transition)
        {
            var host = new PhysicsHost(transition, GraphicsDevice, Container);
            host.Game = gameGenerator(host);
            host.Initialize();
            AddHost(hostTag, host);
            return host.Game;
        }

        public IGame AddHost(string hostTag, Func<Host, IGame> gameGenerator, ITransition transition)
        {
            var host = new EmptyHost(transition, GraphicsDevice, Container);
            host.Game = gameGenerator(host) as IGame;
            host.Initialize();
            AddHost(hostTag, host);
            return host.Game;
        }

        private IScreenHost GenerateHost<THost, TGame>(ITransition transition, TGame game)
                        where THost : IScreenHost
        {
            return Activator.CreateInstance(
                typeof(THost),
                transition,
                GraphicsDevice,
                Container)
                as IScreenHost;
        }

        public void AddHost(string hostTag, IScreenHost host)
        {
            hosts.Add(hostTag, () => host);
        }

        public void AddHost(string hostTag, Func<IScreenHost> host)
        {
            hosts.Add(hostTag, host);
        }

        public void Disable()
        {
            ScreenManager.Visible = false;
            ScreenManager.Enabled = false;
        }

        public void Enable()
        {
            ScreenManager.Visible = true;
            ScreenManager.Enabled = true;
        }

        public bool Show(string hostTag)
        {
            return ScreenManager.AddScreen(hosts[hostTag]());
        }

        public bool IsShowing(string hostTag)
        {
            return ScreenManager.IsShowing(hosts[hostTag]());
        }

        public bool ShowOnly(string hostTag)
        {
            ScreenManager.HideAll();
            return Show(hostTag);
        }

        public bool Hide(string hostTag)
        {
            return ScreenManager.RemoveScreen(hosts[hostTag]());
        }

        public bool Swap(string previousHost, string newHost)
        {
            return Hide(previousHost) && Show(newHost);
        }

        #endregion

    }
}