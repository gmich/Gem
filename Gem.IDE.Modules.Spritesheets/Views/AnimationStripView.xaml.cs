using System;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Modules.MonoGame.Controls;
using Gemini.Modules.Output;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.Composition;

namespace Gem.IDE.Modules.SpriteSheets.Views
{
    /// <summary>
    /// Interaction logic for AnimationStripView.xaml
    /// </summary>
    public partial class AnimationStripView : UserControl, ISceneView, IDisposable
    {
        private readonly IOutput output;
        private GraphicsDevice graphicsDevice;
        private Action<MouseButtonState, Point> trackShape;
        public EventHandler ShapeAdded;

        public AnimationStripView()
        {
            InitializeComponent();
            output = IoC.Get<IOutput>();
            trackShape = InitializeShape;
        }

        public void Invalidate()
        {
            GraphicsControl.Invalidate();
        }

        public void Dispose()
        {
            GraphicsControl.Dispose();
        }

        /// <summary>
        /// Invoked after either control has created its graphics device.
        /// </summary>
        private void OnGraphicsControlLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            graphicsDevice = e.GraphicsDevice;
        }

        /// <summary>
        /// Invoked when our second control is ready to render.
        /// </summary>
        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(Color.CornflowerBlue);

        }

        // Invoked when the mouse moves over the second viewport
        private void OnGraphicsControlMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            Paint(e.LeftButton, new Point((int)position.X, (int)position.Y));

        }

        // We use the left mouse button to do exclusive capture of the mouse so we can drag and drag
        // to rotate the cube without ever leaving the control
        private void OnGraphicsControlHwndLButtonDown(object sender, MouseEventArgs e)
        {
            output.AppendLine("Mouse left button down");
            GraphicsControl.CaptureMouse();
            GraphicsControl.Focus();
        }

        private void OnGraphicsControlHwndLButtonUp(object sender, MouseEventArgs e)
        {
            output.AppendLine("Mouse left button up");
            GraphicsControl.ReleaseMouseCapture();
        }

        private void OnGraphicsControlKeyDown(object sender, KeyEventArgs e)
        {
            output.AppendLine("Key down: " + e.Key);
        }

        private void OnGraphicsControlKeyUp(object sender, KeyEventArgs e)
        {
            output.AppendLine("Key up: " + e.Key);
        }

        private void OnGraphicsControlHwndMouseWheel(object sender, MouseWheelEventArgs e)
        {
            output.AppendLine("Mouse wheel: " + e.Delta);
        }

        public void Paint(MouseButtonState state, Point mousePos)
        {
            //if (graphicsDevice.Viewport.Bounds.Contains(mousePos))
            //{
            trackShape(state, mousePos);
            //}
        }


        private void CaptureMousePosition(MouseButtonState state, Point mousePos)
        {
            if (state == MouseButtonState.Pressed)
            {
                var mousePosition = new Vector2(mousePos.X, mousePos.Y);
        
            }
            else
            {
                trackShape = AddShape;
            }
        }

        private void InitializeShape(MouseButtonState state, Point mousePos)
        {
            if (state == MouseButtonState.Pressed)
            {
             
            }
        }

        private void AddShape(MouseButtonState state, Point mousePos)
        {
        
            GraphicsControl.Invalidate();
        }

    }
}
