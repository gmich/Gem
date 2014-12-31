using Gem.Animations.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gem.Animations
{
    public class AnimationStrip : IAnimation
    {

        public IAnimationEvent<IAnimation, EventArgs> Events
        {
            get { throw new NotImplementedException(); }
        }

        public void Stop(bool force = false)
        {
            throw new NotImplementedException();
        }

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public bool Enabled
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> EnabledChanged;

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int UpdateOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> UpdateOrderChanged;

        public void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<EventArgs> VisibleChanged;
    }
}
