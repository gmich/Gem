using System;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xna.Framework.Graphics;
using Gem.IDE.MonoGame.Interop.Arguments;
using Gem.IDE.MonoGame.Interop.Module;
using Gem.IDE.MonoGame.Interop.Services;

namespace Gem.IDE.MonoGame.Interop.Controls
{
    public sealed class GameControl : ContentControl
    {
        public static readonly DependencyProperty GameModuleProperty;
        private IGameModule game;
        private IInternalGameModule internalModule;
        private DrawingSurface drawingSurface;

        static GameControl()
        {
            GameModuleProperty = DependencyProperty.Register(
                "GameModule",
                typeof(IGameModule),
                typeof(GameControl),
                new PropertyMetadata(default(IGameModule), (s, e) => ((GameControl)s).OnGameModuleChanged()));
        }

        public IGameModule GameModule
        {
            get { return (IGameModule)GetValue(GameModuleProperty); }
            set { SetValue(GameModuleProperty, value); }
        }

        private void OnGameModuleChanged()
        {
            if (game != null)
                throw new InvalidOperationException();

            game = GameModule;
            internalModule = (IInternalGameModule)GameModule;

            drawingSurface = new DrawingSurface();
            drawingSurface.Loaded += OnDrawingSurfaceLoaded;
            drawingSurface.Unloaded += OnDrawingSurfaceUnloaded;

            drawingSurface.LoadContent += OnLoadContent;
            drawingSurface.Draw += OnDraw;
            drawingSurface.MouseMove += OnMouseMove;
            drawingSurface.MouseDown += OnMouseDown;

            KeyDown += OnKeyDown;
            Content = drawingSurface;
        }

        private void OnDrawingSurfaceLoaded(object sender, RoutedEventArgs e)
        {
            if (!internalModule.IsRunning)
                internalModule.Run();
        }
        private void OnDrawingSurfaceUnloaded(object sender, RoutedEventArgs e)
        {
            if (internalModule.IsRunning)
                internalModule.Stop();
        }

        private void OnLoadContent(object sender, GraphicsDeviceEventArgs e)
        {
            var container = new ServiceContainer();
            container.AddService(typeof(IGraphicsDeviceService), MonoGameInteropInjection.Container.Resolve<GraphicsDeviceService>());
            container.AddService(typeof(GraphicsDevice), e.GraphicsDevice);

            internalModule.Prepare(drawingSurface, container);
            game.Initialize();
            game.LoadContent();
            internalModule.Run();
        }
        private void OnDraw(object sender, DrawEventArgs e)
        {
            internalModule.Draw(e);
        }

        // events
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            game.OnMouseMove(new GameMouseEventArgs(e, drawingSurface));
        }
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            game.OnMouseDown(new GameMouseButtonEventArgs(e, drawingSurface));
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            game.OnKeyDown(e);
        }
    }
}
