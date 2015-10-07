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

namespace Gem.Engine
{
    public class GameBase : Game
    {
        private readonly Dictionary<string, Func<IScreenHost>> hosts = new Dictionary<string, Func<IScreenHost>>();

        public event EventHandler<EventArgs> onInitialize;
        public event EventHandler<ContentManager> OnReady;
        public event EventHandler<EventArgs> onContentUnload;
        public event EventHandler<SpriteBatch> onDraw;
        public event EventHandler<GameTime> OnUpdate;

        public GraphicsDeviceManager GraphicsManager { get; }
        public ScreenManager ScreenManager { get; }
        public ContentContainer Container { get; private set; }
        public Settings Settings { get; }
        public InputManager Input { get; } = new InputManager();

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
            ScreenManager = new ScreenManager(this, Input, Settings, (batch) => onDraw?.Invoke(this, batch));
            Components.Add(ScreenManager);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            onInitialize?.Invoke(this, EventArgs.Empty);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Container = new ContentContainer(Content);
            OnReady?.Invoke(this, Content);
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
            OnUpdate?.Invoke(this, gameTime);
            base.Update(gameTime);
        }

        #region Public Methods

        public void AddHost<THost>(string hostTag, THost game, ITransition transition)
        {
            var hostedGame = game as IPhysicsGame;
            if (hostedGame != null)
            {
                AddPhysicsHost(hostTag, hostedGame, transition);
            }
            else
            {
                AddGameHost(hostTag, game as IGame, transition);
            }
        }
        private void AddPhysicsHost<THost>(string hostTag, THost game, ITransition transition)
            where THost : IPhysicsGame
        {

            Func<ITransition, THost, IScreenHost> hostGenerator = GenerateHost<PhysicsHost, THost>;
            AddHost(hostTag, () =>
            {
                var screenHost = hostGenerator(transition, game);
                hostGenerator = (tr, g) => screenHost;
                return screenHost;
            });
        }

        private void AddGameHost<THost>(string hostTag, THost game, ITransition transition)
             where THost : IGame
        {
            Func<ITransition, THost, IScreenHost> hostGenerator = GenerateHost<EmptyHost, THost>;
            AddHost(hostTag, () =>
            {
                var screenHost = hostGenerator(transition, game);
                hostGenerator = (tr, g) => screenHost;
                return screenHost;
            });
        }

        private IScreenHost GenerateHost<THost, TGame>(ITransition transition, TGame game)
                        where THost : IScreenHost
        {
            return Activator.CreateInstance(
                typeof(THost),
                game,
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