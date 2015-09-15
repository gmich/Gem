using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.IDE.Infrastructure
{
    public class DeviceManager : IGraphicsDeviceService
    {
        public DeviceManager(GraphicsDevice device)
        {
            GraphicsDevice = device;
        }
        public GraphicsDevice GraphicsDevice
        {
            get;
        }

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;
    }
}
