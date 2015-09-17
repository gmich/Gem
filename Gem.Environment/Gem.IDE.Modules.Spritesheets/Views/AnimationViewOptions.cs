namespace Gem.IDE.Modules.SpriteSheets
{
    public struct AnimationViewOptions
    {
        public bool ShowNumbers { get; }
        public bool ShowGrid { get; }

        public AnimationViewOptions(bool showNumbers, bool showGrid)
        {
            this.ShowNumbers = showNumbers;
            this.ShowGrid = showGrid;
        }
    }
}
