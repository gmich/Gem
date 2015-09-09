#region File Description
//-----------------------------------------------------------------------------
// Copyright 2011, Nick Gravelyn.
// Licensed under the terms of the Ms-PL: 
// http://www.microsoft.com/opensource/licenses.mspx#Ms-PL
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.IDE.MonoGame.Interop.Services
{
    /// <summary>
    /// Helper class responsible for creating and managing the GraphicsDevice.
    /// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
    /// so even though there can be many controls, there will only ever be a single
    /// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
    /// interface, which provides notification events for when the device is reset
    /// or disposed.
    /// </summary>
    internal class GraphicsDeviceService : IGraphicsDeviceService
    {
        // Keep track of how many controls are sharing the singletonInstance.
        private static int referenceCount;

        private GraphicsDevice graphicsDevice;

        // Store the current device settings.
        private PresentationParameters parameters;

        /// <summary>
        /// Gets the current graphics device.
        /// </summary>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                EnsureGraphicsDevice();
                return graphicsDevice;
            }
        }

        // IGraphicsDeviceService events.
        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        static GraphicsDeviceService()
        {
            MonoGameInteropInjection.Container.Map<GraphicsDeviceService>().ToSelf().AsService();
        }
        /// <summary>
        /// Constructor is private, because this is a singleton class:
        /// client controls should use the public AddRef method instead.
        /// </summary>
        [Obsolete("This constructor shouldn't be called directly. Instead, you should get the (singleton) instance from the IoC container.")]
        public GraphicsDeviceService() { }

        private void EnsureGraphicsDevice()
        {
            if (graphicsDevice != null)
                return;

            // Create the device using the main window handle, and a placeholder size (1,1).
            // The actual size doesn't matter because whenever we render using this GraphicsDevice,
            // we will make sure the back buffer is large enough for the window we're rendering into.
            // Also, the handle doesn't matter because we call GraphicsDevice.Present(...) with the
            // actual window handle to render into.
            CreateDevice(new WindowInteropHelper(Application.Current.MainWindow).Handle, 1, 1);
        }

        private void CreateDevice(IntPtr windowHandle, int width, int height)
        {
            parameters = new PresentationParameters
            {
                BackBufferWidth = Math.Max(width, 1),
                BackBufferHeight = Math.Max(height, 1),
                BackBufferFormat = SurfaceFormat.Color,
                DepthStencilFormat = DepthFormat.Depth24,
                DeviceWindowHandle = windowHandle,
                PresentationInterval = PresentInterval.Immediate,
                IsFullScreen = false
            };

            graphicsDevice = new GraphicsDevice(
                GraphicsAdapter.DefaultAdapter,
                GraphicsProfile.HiDef,
                parameters);

            if (DeviceCreated != null)
                DeviceCreated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets a reference to the singleton instance.
        /// </summary>
        public static GraphicsDeviceService AddRef(int width, int height)
        {
            var singletonInstance = MonoGameInteropInjection.Container.Resolve<GraphicsDeviceService>();

            // Increment the "how many controls sharing the device" reference count.
            if (Interlocked.Increment(ref referenceCount) == 1)
            {
                // If this is the first control to start using the
                // device, we must create the device.
                singletonInstance.EnsureGraphicsDevice();
            }

            return singletonInstance;
        }

        /// <summary>
        /// Releases a reference to the singleton instance.
        /// </summary>
        public void Release(bool disposing)
        {
            // Decrement the "how many controls sharing the device" reference count.
            if (Interlocked.Decrement(ref referenceCount) == 0)
            {
                // If this is the last control to finish using the
                // device, we should dispose the singleton instance.
                if (disposing)
                {
                    if (DeviceDisposing != null)
                        DeviceDisposing(this, EventArgs.Empty);

                    graphicsDevice.Dispose();
                }

                graphicsDevice = null;
            }
        }
        
        /// <summary>
        /// Resets the graphics device to whichever is bigger out of the specified
        /// resolution or its current size. This behavior means the device will
        /// demand-grow to the largest of all its GraphicsDeviceControl clients.
        /// </summary>
        public void ResetDevice(int width, int height)
        {
            int newWidth = Math.Max(parameters.BackBufferWidth, width);
            int newHeight = Math.Max(parameters.BackBufferHeight, height);

            if (newWidth != parameters.BackBufferWidth || newHeight != parameters.BackBufferHeight)
            {
                if (DeviceResetting != null)
                    DeviceResetting(this, EventArgs.Empty);

                parameters.BackBufferWidth = newWidth;
                parameters.BackBufferHeight = newHeight;

                // TODO
                //graphicsDevice.Reset(parameters);

                if (DeviceReset != null)
                    DeviceReset(this, EventArgs.Empty);
            }
        }
    }
}
