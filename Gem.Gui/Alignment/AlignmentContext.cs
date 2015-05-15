using Gem.Gui.Controls;
using Gem.Gui.Rendering;
using Gem.Gui.Transformations;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Alignment
{
    public class AlignmentContext
    {
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();
        public event EventHandler<EventArgs> OnAlignmentChanged;

        public AlignmentContext(IHorizontalAlignable horizontal,
                                IVerticalAlignable vertical,
                                IAlignmentTransition transition)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
            this.Transition = transition;
        }

        private IHorizontalAlignable horizontal;
        public IHorizontalAlignable HorizontalAlignment
        {
            get
            {
                return horizontal;
            }
            set
            {
                horizontal = value;
                OnAlignmentChangedAggregation();
            }
        }

        private IVerticalAlignable vertical;
        public IVerticalAlignable VerticalAlignment
        {
            get
            {
                return vertical;
            }
            set
            {
                vertical = value;
                OnAlignmentChangedAggregation();
            }
        }

        public IAlignmentTransition Transition { get; set; }

        public static AlignmentContext Default
        {
            get
            {
                return new AlignmentContext
                    (
                        Alignment.HorizontalAlignment.Manual,
                        Alignment.VerticalAlignment.Manual,
                        AlignmentTransition.Fixed
                    );
            }
        }

        private void Flush()
        {
            foreach (var transformation in activeTransformations)
            {
                transformation.Dispose();
            }
        }

        public void ActiveTransformations(IDisposable transformation)
        {
            this.activeTransformations.Add(transformation);
        }

        /// <summary>
        /// Returns a list of transformations that adjusts a child region to its parent
        /// </summary>
        /// <remarks>AdjustLocation uses Horizontal and Vertical alignment.
        /// Invoke with Foreach to get both the ITransformation instances</remarks>
        /// <returns>An IEnumerable of Transformations</returns>
        internal ITransformation GetAlignementTransformation(Region parent, Region child, Padding padding)
        {
            return Transition.CreateTransition(child, GetTargetRegion(parent, child, padding));
        }

        internal void ManageTransformation(Func<ITransformation, IDisposable> transformedObject, Region parent, Region child, Padding padding)
        {
            Flush();
            ActiveTransformations(transformedObject(Transition.
                                   CreateTransition(child,
                                                    GetTargetRegion(parent, child, padding))));
        }

        internal void ManageTransformation(Func<ITransformation, IDisposable> transformedObject, Region current, Region target)
        {
            Flush();
            ActiveTransformations(transformedObject(Transition.CreateTransition(current,target)));
        }

        internal Region GetTargetRegion(Region parent, Region child, Padding padding)
        {
            var horizontal = HorizontalAlignment.Align(parent, child, padding);
            var vertical = VerticalAlignment.Align(parent, child, padding);

            return new Region(horizontal.Position, vertical.Position, horizontal.Size, vertical.Size);
        }

        private void OnAlignmentChangedAggregation()
        {
            var handler = OnAlignmentChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

    }
}
