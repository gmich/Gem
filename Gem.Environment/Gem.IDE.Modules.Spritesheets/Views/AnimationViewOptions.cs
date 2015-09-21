namespace Gem.IDE.Modules.SpriteSheets
{
    public struct AnimationViewOptions
    {
        public bool ShowNumbers { get; }
        public bool ShowGrid { get; }
        public bool Animate { get; }

        public AnimationViewOptions(bool showNumbers, bool showGrid,bool animate)
        {
            ShowNumbers = showNumbers;
            ShowGrid = showGrid;
            Animate = animate;
        }
    }
}
