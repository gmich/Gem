using Gem.Gui.Aggregation;
using Gem.Gui.Animations;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Gui.ScreenSystem
{
    public class GuiHost : IGuiHost
    {

        #region Fields

        private readonly AggregationContext aggregationContext;
        private readonly IList<AControl> controls = new List<AControl>();
        private AnimationContext animationContext;

        #endregion

        public GuiHost(List<AControl> controls,
                       AggregationContext aggregationContext,
                       ITransition transition)
            : base()
        {
            this.controls = controls;
            this.aggregationContext = aggregationContext;
            this.Transition = transition;
            this.ScreenState = ScreenState.Exit;

        }

        #region Properties

        public AControl this[int id]
        {
            get
            {
                return controls[id];
            }
        }

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
                    this.ScreenState = enterState;
                    break;
                case TransitionDirection.Leave:
                    this.ScreenState = leaveState;
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

        public IEnumerable<AControl> Entries()
        {
            foreach (var control in controls)
            {
                yield return control;
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
            //foreach (var control in controls)
            //{
            //    control.HasFocus=false;
            //}
            Transition.Start(TransitionDirection.Leave);
        }

        public void HandleInput(GameTime gameTime)
        {
            aggregationContext.Aggregate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            this.animationContext = animationContext ?? new AnimationContext(gameTime);

            foreach (var control in controls)
            {
                control.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (ScreenState == ScreenState.TransitionOff
                || ScreenState == ScreenState.TransitionOn)
            {
                Transition.Update(animationContext);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var control in controls)
            {
                control.Render(batch);
            }
        }
    }
}


