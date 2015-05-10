using Gem.Gui.Rendering;
using Gem.Gui.Transformations;
using System;
using System.Collections.Generic;

namespace Gem.Gui.Alignment
{
    public class AlignmentContext
    {
        private readonly List<IDisposable> activeTransformations = new List<IDisposable>();

        public IHorizontalAlignable HorizontalAlignment { get; set; }
        public IVerticalAlignable VerticalAlignment { get; set; }
        public IAlignmentTransition Transition { get; set; }

        private static Lazy<AlignmentContext> defaultOptions = new Lazy<AlignmentContext>(() =>
            new AlignmentContext
            {
                HorizontalAlignment = Alignment.HorizontalAlignment.Manual,
                VerticalAlignment = Alignment.VerticalAlignment.Manual,
                Transition = AlignmentTransition.Fixed
            });

        public static AlignmentContext Default
        {
            get { return defaultOptions.Value; }
        }

        private void Flush()
        {
            foreach (var transformation in activeTransformations)
            {
                transformation.Dispose();
            }
        }

        /// <summary>
        /// Returns a list of transformations that adjusts a child region to its parent
        /// </summary>
        /// <remarks>AdjustLocation uses Horizontal and Vertical alignment.
        /// Invoke with Foreach to get both the ITransformation instances</remarks>
        /// <returns>An IEnumerable of Transformations</returns>
        internal IEnumerable<ITransformation> AdjustLocation(Region parent, Region child)
        {
            Flush();

            yield return Transition.CreateTransition(child, HorizontalAlignment.Align(parent, child));
            yield return Transition.CreateTransition(child, VerticalAlignment.Align(parent, child));
        }

    }
}
