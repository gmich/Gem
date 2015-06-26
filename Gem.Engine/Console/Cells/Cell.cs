using Gem.Engine.Console.Rendering.Animations;
using Gem.Infrastructure.Functional;
using System;

namespace Gem.Engine.Console.Cells
{
    public class Cell : ICell
    {
        private readonly string content;
        private readonly int sizeX;
        private readonly int sizeY;

        public Cell(string content, int sizeX, int sizeY, Func<ICell,Behavior<IEffect>> behavior)
        {
            this.content = content;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.Behavior = behavior(this);
        }

        public string Content { get { return content; } }

        public int SizeX { get { return sizeX; } }

        public int SizeY { get { return sizeY; } }

        public Behavior<IEffect> Behavior { get; set; }
    }
}
