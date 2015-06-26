using Gem.Engine.Console.Cells;
using Gem.Engine.Console.Rendering.Animations;
using Gem.Infrastructure.Functional;
using System;
using System.Collections.Generic;

namespace Gem.Engine.Console.EntryPoint
{
    public interface ICursor
    {
        event EventHandler<CursorEventArgs> CursorChanged;

        Behavior<IEffect> Effect { get; set; }

        int Row { get; }
        int Head { get; set; }
        int HeadInRow { get; }

        void Left();
        void Right();

        void Update(IEnumerable<Row> rows);

    }
}
