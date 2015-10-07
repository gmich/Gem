using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Engine.Containers;
using Gem.Engine.Input;
using Gem.Engine.Configuration;

namespace Gem.Engine.ScreenSystem
{
    public abstract class Host : IScreenHost
    {

        public ContentContainer Container {  get; }
        public GraphicsDevice Device { get; }        
        public Settings Settings {  get { return ScreenManager.Settings; } }

        public Host(ITransition transition,GraphicsDevice device, ContentContainer container)
        {
            Device = device;
            Container = container;
            this.Transition = transition;
            this.ScreenState = ScreenState.Exit;
        }

        #region Properties

        private ITransition transition;
        public ITransition Transition
        {
            get
            {
                return transition;
            }
            set
            {
                transition = value;
                transition.TransitionStarted += (sender, direction) => AssignState(direction, ScreenState.TransitionOn, ScreenState.TransitionOff);
                transition.TransitionFinished += (sender, direction) => AssignState(direction, ScreenState.Active, ScreenState.Exit);
                transition.TransitionFinished += (sender, direction) => RaiseEndEvent( direction);
            }
        }


        public ScreenState ScreenState { get; set; }

        #endregion

        #region Events

        public event EventHandler<EventArgs> OnEntering;
        public event EventHandler<EventArgs> OnExiting;

        public event EventHandler<EventArgs> OnEnter;
        public event EventHandler<EventArgs> OnExit;

        private void OnEnteringScreen()
        {
            var handler = OnEntering;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        
        private void OnExitingScreen()
        {
            var handler = OnExiting;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnEnteredScreen()
        {
            var handler = OnEnter;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnExitedScreen()
        {
            var handler = OnExit;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        #endregion

        private void AssignState(TransitionDirection direction, ScreenState enterState, ScreenState leaveState)
        {
            switch (direction)
            {
                case TransitionDirection.Enter:
                    ScreenState = enterState;
                    break;
                case TransitionDirection.Leave:
                    ScreenState = leaveState;
                    break;
                default:
                    break;
            }
        }

        private void RaiseEndEvent(TransitionDirection direction)
        {
            switch (direction)
            {
                case TransitionDirection.Enter:
                    OnEnteredScreen();
                    break;
                case TransitionDirection.Leave:
                    OnExitedScreen();
                    break;
                default:
                    break;
            }
        }

        public void EnterScreen()
        {
            OnEnteringScreen();
            Transition.Start(TransitionDirection.Enter);
        }

        public void ExitScreen()
        {
            OnExitingScreen();
            Transition.Start(TransitionDirection.Leave);
        }

        public abstract void HandleInput(InputManager inputManager, GameTime gameTime);

        public abstract void Initialize();

        public void Update(GameTime gameTime)
        {
            if (ScreenState == ScreenState.TransitionOff
                || ScreenState == ScreenState.TransitionOn)
            {
                Transition.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            FixedUpdate(gameTime);
        }

        public abstract void FixedUpdate(GameTime gameTime);

        public abstract void Draw(SpriteBatch batch);

    }
}


