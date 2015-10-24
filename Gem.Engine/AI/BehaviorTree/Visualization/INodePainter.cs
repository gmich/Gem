using Microsoft.Xna.Framework;

namespace Gem.Engine.AI.BehaviorTree.Visualization
{
    public interface INodePainter
    {
        Color Link
        { get; set; }

        Color NodeBackground
        { get; set; }

        Color NodeTitleBackground
        { get; set; }

        Color Line
        { get; set; }

        Color Triggered
        { get; set; }

        Color NodeName
        { get; set; }

        Color NodeBehaviorType
        { get; set; }

        void Paint<AIContext>(IBehaviorVirtualizationPiece piece, IBehaviorNode<AIContext> node);
    }
}
