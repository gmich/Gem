using System;
using Gem.Gui.Input;
using Microsoft.Xna.Framework.Input;

namespace Gem.Gui.Aggregation
{
    public class ScriptAggregator<TInputHelper> : IAggregator    
        where TInputHelper : Input.IInputHelper
    {
        #region Fields

        private readonly Func<TInputHelper,bool> next;
        private readonly Func<TInputHelper,bool> previous;
        private readonly Func<TInputHelper,bool> trigger;
        private readonly TInputHelper inputHelper;

        #endregion

        #region Ctor

        public ScriptAggregator(TInputHelper inputHelper,
                                Func<TInputHelper,bool> next, 
                                Func<TInputHelper,bool> previous, 
                                Func<TInputHelper,bool> trigger)
        {
            this.inputHelper = inputHelper;
            this.next = next;
            this.previous = previous;
            this.trigger = trigger;
            this.IsEnabled = true;
        }

        #endregion

        #region IAggregator Members

        public bool IsEnabled
        {
            get;
            set;
        }

        public void Aggregate(GuiEntry entry, AggregationContext context)
        {
            //TODO: implement repeat

            if (!context.FirstEntry) return;

            if (next(inputHelper))
            {
                context.Reset();
                context.FocusControlAt(context.Indexer.Next);
            }
            if (previous(inputHelper))
            {
                context.Reset();
                context.FocusControlAt(context.Indexer.Previous);
            }
            if (trigger(inputHelper))
            {
                context[context.Indexer.Current].Events.OnClicked();
            }

            context.FirstEntry = false;
        }

        #endregion

    }

    public static class Script
    {
        public static ScriptAggregator<KeyboardInputHelper> ForKeyboard(KeyboardMenuScript keyboardScript)
        {
            return new ScriptAggregator<KeyboardInputHelper>(InputManager.Keyboard,
                                                             input => input.IsKeyClicked(keyboardScript.Next),
                                                             input => input.IsKeyClicked(keyboardScript.Previous),
                                                             input => input.IsKeyClicked(keyboardScript.Trigger));
        }
    }
}
