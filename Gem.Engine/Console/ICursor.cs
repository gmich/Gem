using Gem.Console.Animations;
using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Console
{
    public interface ICursor
    {
        event EventHandler<CursorEventArgs> CursorChanged;

        Behavior<IEffect> Effect { get; set; }

        int Row { get; }
        int Head { get; set; }
        int HeadInRow { get; }

        void Up();
        void Down();
        void Left();
        void Right();

        void Update(IEnumerable<Row> rows);

    }
}
