using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Gem.IDE.MonoGame.Interop.Helpers;

namespace Gem.IDE.MonoGame.Interop.Arguments
{
    public class GameMouseEventArgs
    {
        private readonly MouseEventArgs args;
        private readonly Lazy<Vector2> position;

        public GameMouseEventArgs(MouseEventArgs args, IInputElement inputElement)
        {
            this.args = args;
            position = new Lazy<Vector2>(() => args.GetPosition(inputElement).ToVector());
        }

        public Vector2 Position
        {
            get { return position.Value; }
        }
    }
}