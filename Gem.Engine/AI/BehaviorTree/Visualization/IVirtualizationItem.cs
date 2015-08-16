using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.AI.BehaviorTree.Visualization
{
    internal interface IVirtualizationItem
    {
        RenderedNode Node { get; }

        Vector2 Position { get; }

        Texture2D Texture { get; }

        Color Color { get; }

        void Reset();

        bool IsActive { get; }

        void Update(double timeDelta);

        void Draw(SpriteBatch batch, Vector2 position, int sizeX, int sizeY);

    }
}
