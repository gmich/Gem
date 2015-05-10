using Gem.Gui.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.ScreenSystem
{
    public interface IGuiHost
    {

        ScreenState ScreenState { get; }

        float TransitionAlpha { get;  }

        bool IsPopup { get; }

        void HandleInput();

        void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen);

        void Draw(SpriteBatch batch);

    }
}
