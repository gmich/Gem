using Gem.Console.Animations;
using Gem.Infrastructure.Functional;
using Microsoft.Xna.Framework;
using System;

namespace Gem.Console
{
    internal class CellBehavior : ICellBehavior
    {
        public CellBehavior(Color color, float rotation, float alpha)
        {
            this.Color = Behavior.Create(ctx => color);
            this.Content = (cell) => Behavior.Create(ctx => cell.Content);
            this.Rotation = rotation.Forever();
            this.Alpha = alpha.Forever();
        }

        public Behavior<Color> Color { get; set; }
        public Func<ICell, Behavior<string>> Content { get; set; }
        public Behavior<float> Rotation { get; set; }
        public Behavior<float> Alpha { get; set; }

        public Behavior<IEffect> CreateEffect(ICell cell)
        {
            return Animate.Cell(Content(cell), Color, Alpha, Rotation);
        }
    }
}
