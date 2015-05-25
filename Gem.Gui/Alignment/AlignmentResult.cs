namespace Gem.Gui.Alignment
{
    /// <summary>
    /// The result of an alignment by AlignmentContext
    /// </summary>
    public class AlignmentResult
    {
        private readonly float position;
        private readonly float size;

        public AlignmentResult(float position, float size)
        {
            this.position = position;
            this.size = size;
        }

        public float Position { get { return position; } }
        public float Size { get { return size; } }
    }
}
