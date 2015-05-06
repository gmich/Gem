using Microsoft.Xna.Framework;

namespace Gem.Gui.ScreenSystem
{
    public interface IGuiHost
    {
        ScreenState ScreenState { get; }

        float TransitionAlpha { get;  }

        bool IsPopup { get; }

        void HandleInput();

        void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);

        void Draw();
    }
}
