using Gem.Gui.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Gui.ScreenSystem
{
    public interface IGuiHost
    {
        event EventHandler<EventArgs> OnEntering;
        event EventHandler<EventArgs> OnExiting;

        event EventHandler<EventArgs> OnEnter;
        event EventHandler<EventArgs> OnExit;

        IEnumerable<AControl> Entries();

        AControl this[int id] { get; }

        ScreenState ScreenState { get; }

        ITransition Transition { get; set; }

        void EnterScreen();

        void ExitScreen();

        void HandleInput(GameTime gameTime);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch batch);

    }

}
