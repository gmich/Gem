using Gem.AI.BehaviorTree.Composites;
using Gem.AI.BehaviorTree.Decorators;
using Gem.AI.BehaviorTree.Leaves;
using Microsoft.Xna.Framework;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{
    public class MinimalColorPainter: INodePainter
    {
        public Color Link
        { get; set; } = new Color(153, 153, 103);

        public Color NodeBackground
        { get; set; } = new Color(204, 204, 154);

        public Color NodeTitleBackground
        { get; set; } = new Color(153, 153, 103);

        public Color Line
        { get; set; } = new Color(204, 204, 204);

        public Color Triggered
        { get; set; } = new Color(52, 73, 94);

        public Color NodeName
        { get; set; } = new Color(204, 204, 154);

        public Color NodeBehaviorType
        { get; set; } = new Color(153, 153, 103);

        public void Paint<AIContext>(IBehaviorVirtualizationPiece piece, IBehaviorNode<AIContext> node)
        {
            if (piece is RenderedNode)
            {
                node.OnBehaved += (sender, args) =>
                {
                    var renderedNode = (piece as RenderedNode);
                    renderedNode.BehaviorStatus = (args as BehaviorInvokationEventArgs).Result;
                    renderedNode.Trigger();
                };
            }
        }
    }
}
