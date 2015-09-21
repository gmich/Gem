namespace Gem.IDE.Modules.SpriteSheets
{
    public struct AnimationViewOptions
    {
        public bool ShowNumbers { get; }
        public bool ShowGrid { get; }
        public bool Animate { get; }
        public bool ShowTileSheet { get; }
        
        public AnimationViewOptions(bool showNumbers, bool showGrid,bool animate,bool showTileSheet)
        {
            ShowNumbers = showNumbers;
            ShowGrid = showGrid;
            Animate = animate;
            ShowTileSheet = showTileSheet;
        }
    }
}
