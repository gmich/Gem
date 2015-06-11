namespace Gem.Console
{
    public class Cell : ICell
    {
        private readonly string content;
        private readonly int sizeX;
        private readonly int sizeY;

        public Cell(string content, int sizeX, int sizeY)
        {
            this.content = content;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public string Content { get { return content; } }

        public int SizeX { get { return sizeX; } }

        public int SizeY { get { return sizeY; } }
    }
}
