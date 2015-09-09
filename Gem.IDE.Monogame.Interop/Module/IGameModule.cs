using System.Windows.Input;
using Microsoft.Xna.Framework;
using Gem.IDE.MonoGame.Interop.Arguments;

namespace Gem.IDE.MonoGame.Interop.Module
{
    public interface IGameModule
    {
        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw();

        // events
        void OnMouseMove(GameMouseEventArgs e);
        void OnMouseDown(GameMouseButtonEventArgs e);
        void OnKeyDown(KeyEventArgs e);
    }
}