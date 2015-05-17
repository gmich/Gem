namespace Gem.Gui.Rendering
{
    public interface IRenderable
    {
        Region Region { get; }
        RenderParameters RenderParameters { get; }
    }
}
