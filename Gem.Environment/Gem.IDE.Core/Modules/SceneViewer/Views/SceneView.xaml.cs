using System;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Modules.MonoGame.Controls;
using Gemini.Modules.Output;
using Microsoft.Xna.Framework;
using Gem.Engine.Primitives;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Gem.IDE.Core.Modules.SceneViewer.ViewModels;

namespace Gem.IDE.Core.Modules.SceneViewer.Views
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class SceneView : UserControl, ISceneView, IDisposable
    {
        private readonly IOutput output;
        private GraphicsDevice graphicsDevice;
        private ShapeDeclaration shapeDeclaration;
        private Action<MouseButtonState, Point> trackShape;
        private DynamicShape processedShape;
        public EventHandler ShapeAdded;

        public List<IShape> Shapes
        {
            get;
        } = new List<IShape>();

        public SceneView()
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
            processedShape = new DynamicShape(Color.White, graphicsDevice, (int)ActualWidth, (int)ActualHeight);
        }

        /// <summary>
        /// Invoked when our second control is ready to render.
        /// </summary>
        private void OnGraphicsControlDraw(object sender, DrawEventArgs e)
        {
            e.GraphicsDevice.Clear(Color.CornflowerBlue);
            DrawActiveShape();
            Shapes.ForEach(shape => shape.Draw());

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

        public void DrawActiveShape()
        {
            processedShape.Draw();
        }

        private void CaptureMousePosition(MouseButtonState state, Point mousePos)
        {
            if (state == MouseButtonState.Pressed)
            {
                var mousePosition = new Vector2(mousePos.X, mousePos.Y);
                if (shapeDeclaration.Capture(mousePosition))
                {
                    processedShape.AddVertex(mousePosition);
                    GraphicsControl.Invalidate();
                }
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
                shapeDeclaration = new ShapeDeclaration(new Vector2(mousePos.X, mousePos.Y));
                processedShape.AddVertex(shapeDeclaration.ViewportOffset);
                processedShape.AddVertex(shapeDeclaration.ViewportOffset);
                trackShape = CaptureMousePosition;
                GraphicsControl.Invalidate();
            }
        }

        private void AddShape(MouseButtonState state, Point mousePos)
        {
            int vertices = shapeDeclaration.VerticesPosition.Count;

            if (vertices == 1)
            {
                shapeDeclaration.VerticesPosition.Add(shapeDeclaration.VerticesPosition[0] + Vector2.One);
            }
            if (vertices != 0)
            {
                Shapes.Add(
                 new FixedBoundsShape(
                     shapeDeclaration.VerticesPosition,
                     shapeDeclaration.ViewportOffset.X,
                     shapeDeclaration.ViewportOffset.Y,
                     (int)ActualWidth,
                      (int)ActualHeight,
                       ((SceneViewModel)DataContext).Color,
                     graphicsDevice));
                ShapeAdded?.Invoke(this, EventArgs.Empty);            
            }
            trackShape = InitializeShape;
            processedShape.Reset();
            GraphicsControl.Invalidate();
        }

    }

}



