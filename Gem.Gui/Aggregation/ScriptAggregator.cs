using System;
using Gem.Gui.Input;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// Aggregates using delegates that take an instance of IInputHelper for next, previous and triggering events
    /// </summary>
    /// <typeparam name="TInputHelper">The IInputHelper that is used to determine the enumeration</typeparam>
    public class ScriptAggregator<TInputHelper> : IAggregator
        where TInputHelper : Input.IInputHelper
    {
        #region Fields

        private readonly Func<TInputHelper, bool> next;
        private readonly Func<TInputHelper, bool> previous;
        private readonly Func<TInputHelper, bool> trigger;
        private readonly Predicate<TInputHelper> disableWhen;
        private readonly KeyRepetition KeyRepetition;
        private readonly TInputHelper inputHelper;

        private double keyRepeatTimer;
        private AggregationAction aggrAction;

        /// True when at least one script has evaluated upon aggregation
        private bool scriptHasEvaluated;

        #endregion

        private enum AggregationAction
        {
            None,
            Next,
            Previous,
            Trigger
        }

        #region Ctor

        public ScriptAggregator(TInputHelper inputHelper,
                                Predicate<TInputHelper> disableWhen,
                                Func<TInputHelper, bool> next,
                                Func<TInputHelper, bool> previous,
                                Func<TInputHelper, bool> trigger,
                                KeyRepetition KeyRepetition)
        {
            this.inputHelper = inputHelper;
            this.next = next;
            this.previous = previous;
            this.trigger = trigger;
            this.IsEnabled = true;
            this.KeyRepetition = KeyRepetition;
            this.disableWhen = disableWhen;
        }

        #endregion

        #region IAggregator Members

        public bool IsEnabled
        {
            get;
            private set;
        }

        public void Aggregate(AggregationEntry entry, AggregationContext context)
        {
            if (disableWhen(inputHelper)) return;
            if (!context.FirstEntry) return;

            double timeDelta = context.Time(span => span.TotalSeconds);

            if (ShouldHandle(next, AggregationAction.Next, timeDelta))
            {
                context.Reset();
                context.FocusControlAt(context.Indexer.Next);
            }
            if (ShouldHandle(previous, AggregationAction.Previous, timeDelta))
            {
                context.Reset();
                context.FocusControlAt(context.Indexer.Previous);
            }
            if (ShouldHandle(trigger, AggregationAction.Trigger, timeDelta))
            {
                context[context.Indexer.Current].Events.OnClicked();
            }

            aggrAction = scriptHasEvaluated ? aggrAction : AggregationAction.None;

            scriptHasEvaluated = false;
            context.FirstEntry = false;
        }

        /// <summary>
        /// Handles key repetition
        /// </summary>
        private bool ShouldHandle(Func<TInputHelper, bool> script, AggregationAction aggrAction, double timeDelta)
        {
            if (script(inputHelper))
            {
                scriptHasEvaluated = true;
                if (this.aggrAction != aggrAction)
                {
                    keyRepeatTimer = KeyRepetition.KeyRepeatStartDuration;
                    this.aggrAction = aggrAction;
                    return true;
                }
                if (this.aggrAction == aggrAction)
                {
                    keyRepeatTimer -= timeDelta;
                    if (keyRepeatTimer <= 0.0f)
                    {
                        keyRepeatTimer += KeyRepetition.KeyRepeatDuration;
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        #endregion

    }

}
