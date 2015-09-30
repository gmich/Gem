using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Map.Chunk
{
    public class ChunkEndEventArgs : EventArgs
    {
    }
    public class Chunk
    {
        public Chunk(int chunkWidth, int chunkHeight, int locationX, int locationY, int id, int next, int previous)
        {
            ChunkWidth = chunkWidth;
            ChunkHeight = chunkHeight;
            LocationX = locationX;
            LocationY = locationY;
            Id = id;
            Next = next;
            Previous = previous;
        }

        int ChunkWidth { get; }

        int ChunkHeight { get; }

        int LocationX { get; }

        int LocationY { get; }

        int Id { get; }

        int Next { get; set; }

        int Previous { get; set; }
    }

    public class ChunkTracker
    {

        public Chunk Chunk { get; }

        public event EventHandler<ChunkEndEventArgs> onReachedTopThresHold;
        public event EventHandler<ChunkEndEventArgs> onReachedBottomThresHold;
        public event EventHandler<ChunkEndEventArgs> onReachedLeftThresHold;
        public event EventHandler<ChunkEndEventArgs> onReachedRightThresHold;


    }

    public class UnitSpecification
    {
        public UnitSpecification(int horizontalPixels, int verticalPixels)
        {
            HorizontalPixels = horizontalPixels;
            VerticalPixels = verticalPixels;
        }
        public int HorizontalPixels { get; }
        public int VerticalPixels { get; }
    }
    public class ChunkLocationTransformer
    {

        public Rectangle WorldPort { get; }
        int LocationOffset { get; }
    }
}
