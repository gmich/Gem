using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroGameEngine.Animations.Events
{

    /// <summary>
    /// TODO: set proper event args
    /// </summary>
    public class AnimationEventArgs :EventArgs
    {
        string Name { get; set; }
    }
    
    public class AnimationEventAggregator<TeventArgs> : IAnimationEvent<IAnimation, TeventArgs> 
        where TeventArgs : EventArgs
    {
        public void RaiseStartEvent(IAnimation sender, TeventArgs args)
        {
            var handler = OnStartEvent;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public void RaiseProgressEvent(IAnimation sender, TeventArgs args)
        {
            var handler = OnProgressEvent;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public void RaiseEndEvent(IAnimation sender, TeventArgs args)
        {
            var handler = OnEndEvent;
            if (handler != null)
            {
                handler(sender, args);
            }
        }

        public event EventHandler<TeventArgs> OnStartEvent;

        public event EventHandler<TeventArgs> OnProgressEvent;

        public event EventHandler<TeventArgs> OnEndEvent;
    }

}
