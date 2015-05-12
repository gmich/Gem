using Gem.Gui.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Gem.Gui.ScreenSystem
{
    public interface IGuiHost
    {
        IEnumerable<AControl> Entries();

        AControl this[int id] { get; }

        ScreenState ScreenState { get; }

        ITransition Transition { get; }

        void EnterScreen();

        void ExitScreen();

        void HandleInput();

        void Update(GameTime gameTime);

        void Draw(SpriteBatch batch);
    }

}
