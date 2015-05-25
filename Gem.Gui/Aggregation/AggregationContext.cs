using Gem.Gui.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Aggregation
{
    /// <summary>
    /// The aggregation context is responsible for the GuiHost's Control's aggregation
    /// The context knows which controls are being aggregated and which control has focus.
    /// </summary>
    public class AggregationContext
    {

        #region Fields

        /// <summary>
        /// Aggregates the entries
        /// </summary>
        private readonly IList<IAggregator> aggregators;

        /// <summary>
        /// The control with its aggregation token
        /// </summary>
        private readonly List<AggregationEntry> entries = new List<AggregationEntry>();

        /// <summary>
        /// Enumerates the entries
        /// </summary>
        private readonly Indexer indexer;

        /// <summary>
        /// Keeps track of the elapsed gametime
        /// </summary>
        private GameTime gameTime;

        #endregion

        public AggregationContext(IList<IAggregator> aggregators, IEnumerable<AControl> controls)
        {
            this.aggregators = aggregators;
            int controlCount = 0;
            foreach (var control in controls)
            {
                //aggregate only the controls that allow focus
                if (control.Options.IsFocusEnabled)
                {
                    entries.Add(new AggregationEntry(control, controlCount++));
                }
            }
            indexer = new Indexer(0, entries.Count - 1);

            if (entries.Count > 0)
            {
                FocusControlAt(0);
            }
        }

        /// <summary>
        /// Enumerates the entries
        /// </summary>
        public Indexer Indexer { get { return indexer; } }

        /// <summary>
        /// True on the first aggregation cycle
        /// </summary>
        public bool FirstEntry { get; set; }

        /// <summary>
        /// Adds new aggregator to the aggregation list and returns the entry's disposable 
        /// </summary>
        /// <param name="aggregator">The aggregator entry</param>
        /// <returns>The entry's disposable</returns>
        public IDisposable AddAggregator(IAggregator aggregator)
        {
            aggregators.Add(aggregator);
            return Gem.Infrastructure.Disposable.Create(aggregators, aggregator);
        }

        /// <summary>
        /// Sets all control's focus to false
        /// </summary>
        public void Reset()
        {
            foreach (var entry in entries)
            {
                entry.Control.HasFocus = false;
            }
        }

        /// <summary>
        /// Finds all the focused controls and fires the OnClicked event
        /// </summary>
        public void TriggerFocusedControl()
        {
            foreach (var entry in entries)
            {
                if (entry.Control.HasFocus)
                {
                    entry.Control.Events.OnClicked();
                }
            }
        }
        
        /// <summary>
        /// Gets te control by index
        /// </summary>
        /// <param name="entryId">The entry id</param>
        /// <returns>A control</returns>
        public AControl this[int entryId]
        {
            get { return entries[entryId].Control; }
        }
        
        /// <summary>
        /// Unfocuses previous control and focuses another
        /// </summary>
        /// <param name="controlIndex">The new focused control's index</param>
        public void FocusControlAt(int controlIndex)
        {
            entries[indexer.Current].Control.HasFocus = false;

            indexer.Current = controlIndex;
            entries[indexer.Current].Control.HasFocus = true;
        }

        /// <summary>
        /// Gets the time according to the elasped game time
        /// </summary>
        /// <param name="timeCalculator">The calculation delegate</param>
        /// <returns>The time as double</returns>
        public double Time(Func<TimeSpan, double> timeCalculator)
        {
            if (gameTime == null) return 0;
            return timeCalculator(this.gameTime.ElapsedGameTime);
        }

        /// <summary>
        /// Aggregates the entries
        /// </summary>
        /// <param name="gameTime">The elapsed gametime</param>
        public void Aggregate(GameTime gameTime)
        {
            this.gameTime = gameTime;

            foreach (var aggregator in aggregators)
            {
                this.FirstEntry = true;
                entries.ForEach(entry => aggregator.Aggregate(entry, this));
            }
        }
    }
}
