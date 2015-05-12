using Gem.Gui.Aggregation;
using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Gem.Gui.ScreenSystem
{
    public class GuiHost : IGuiHost
    {
        private readonly AggregationContext aggregationContext;
        private readonly IList<AControl> controls = new List<AControl>();
        private readonly RenderTemplate renderTemplate;

        public GuiHost(List<AControl> controls, RenderTemplate renderTemplate, AggregationContext aggregationContext, ITransition transition)
            : base()
        {
            this.renderTemplate = renderTemplate;
            this.controls = controls;
            this.aggregationContext = aggregationContext;
            this.Transition = transition;
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
            }
        }


        public ScreenState ScreenState { get; set; }

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
        public IEnumerable<AControl> Entries()
        {
            foreach (var control in controls)
            {
                yield return control;
            }
        }

        public void EnterScreen()
        {
            Transition.Start(TransitionDirection.Enter);
        }


        public void ExitScreen()
        {
            Transition.Start(TransitionDirection.Leave);
        }

        public void HandleInput()
        {
            aggregationContext.Aggregate();
        }

        public void Update(GameTime gameTime)
        {
            System.Console.WriteLine(this.ScreenState);
            foreach (var control in controls)
            {
                control.Update(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(ScreenState == ScreenState.TransitionOff 
                || ScreenState== ScreenState.TransitionOn)
            {
                Transition.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var control in controls)
            {
                control.Render(batch, renderTemplate);
            }
        }
    }
}


