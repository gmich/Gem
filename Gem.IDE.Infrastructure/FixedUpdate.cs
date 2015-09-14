using Microsoft.Xna.Framework;
using System;

namespace Gem.IDE.Infrastructure
{
    public class FixedUpdate : Game
    {
        private readonly Action<double> fixedUpdatedAction;
        private GraphicsDeviceManager graphics;
        public FixedUpdate(Action<double> fixedUpdatedAction)
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            this.fixedUpdatedAction = fixedUpdatedAction;       
        }

        public void Stop()
        {
            Dispose();
        }
        protected override void Update(GameTime gameTime)
        {
            fixedUpdatedAction(gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Initialize()
        {
         
            base.Initialize();
        }

    }
}
