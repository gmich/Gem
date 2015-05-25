using Gem.Gui.Rendering;
using Gem.Gui.Transformations;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Alignment
{
    /// <summary>
    /// Holds all the alignment related information
    /// </summary>
    public class AlignmentContext
    {
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();
        
        public AlignmentContext(IHorizontalAlignable horizontal,
                                IVerticalAlignable vertical,
                                IAlignmentTransition transition)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
            this.Transition = transition;
        }

        #region Properties

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
        
        /// <summary>
        /// Static factory property that returns an instance of AlignmentContext with its default values
        /// </summary>
        public static AlignmentContext Default
        {
            get
            {
                return new AlignmentContext
                    (
                        Alignment.HorizontalAlignment.Manual,
                        Alignment.VerticalAlignment.Manual,
                        AlignmentTransition.Instant
                    );
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Flushes ongoing transformations
        /// </summary>
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
        /// <returns>A Transformation</returns>
        internal ITransformation GetAlignementTransformation(Region parent, Region child, Padding padding)
        {
            return Transition.CreateTransition(child, GetTargetRegion(parent, child, padding));
        }

        /// <summary>
        /// Adds a transformation to the object that's being trasnformed using the transformedObject 
        /// delegate and uses the list activeTransformations to handle the disposable entry.
        /// </summary>
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

        #endregion

        #region Events

        public event EventHandler<EventArgs> OnAlignmentChanged;

        private void OnAlignmentChangedAggregation()
        {
            var handler = OnAlignmentChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion

    }
}
