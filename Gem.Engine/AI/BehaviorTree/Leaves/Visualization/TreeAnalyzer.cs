using System.Collections.Generic;
using System.Linq;
using System;

namespace Gem.AI.BehaviorTree.Visualization
{

    internal class TreeAnalyzer<AIContext>
    {

        #region Fields

        //int represents tree depth
        private readonly Dictionary<int, List<IBehaviorVirtualizationPiece>> nodeVisualizationInfo
            = new Dictionary<int, List<IBehaviorVirtualizationPiece>>();

        private float nodeWidth;
        private float treeWidth;

        private Func<float> TreeWidth => () => treeWidth;
        private Func<float> NodeWidth => () => nodeWidth;

        private readonly Action<IBehaviorVirtualizationPiece, IBehaviorNode<AIContext>> onAddAction;

        #endregion

        #region Ctor

        public TreeAnalyzer(Action<IBehaviorVirtualizationPiece,IBehaviorNode<AIContext>> onAddAction,
                            IBehaviorNode<AIContext> root)
        {
            this.onAddAction = onAddAction;
            string nodeType = root.GetType().Name;
            var rootInfo = new RenderedNode(
                        nodeType.Substring(0, nodeType.Count() - 2),
                        0,
                        root.Name,
                        () => AlignRelativeTo(() => TreeWidth() / 2, NodeWidth(), 1, 0,0));
            onAddAction(rootInfo, root);

            nodeVisualizationInfo.Add(0,
               new List<IBehaviorVirtualizationPiece>(new[] { rootInfo }));

            var subNodes = root.SubNodes.ToArray();
            Func<float> linkPositionX = () => rootInfo.PositionX;

            LinkBase link = new LinkBase(
                rootInfo,
                linkPositionX,
                subNodes.Count());

            nodeVisualizationInfo.Add(1,
                new List<IBehaviorVirtualizationPiece>(new[] {
                  link as IBehaviorVirtualizationPiece,
                }));
            for (int nodeIndex = 0; nodeIndex < subNodes.Count(); nodeIndex++)
            {
                AddNode(subNodes[nodeIndex],
                    () => TreeWidth(),
                    () => NodeWidth(),
                    linkPositionX,
                    subNodes.Count() + 1,
                    nodeIndex,
                    1);
            }
        }

        #endregion

        #region Public Methods and Properties

        public void SetNodeWidth(float newNodeWidth)
        {
            nodeWidth = newNodeWidth;
        }

        public void SetTreeWidth(float newTreeWidth)
        {
            treeWidth = newTreeWidth;
        }

        public Dictionary<int, List<IBehaviorVirtualizationPiece>> AnalyzedTree
        {
            get { return nodeVisualizationInfo; }
        }

        #endregion

        #region Private Helpers

        private void AddNode(IBehaviorNode<AIContext> node, Func<float> rowWidth, Func<float> nodeWidth, Func<float> linkX, int nodeCount, int column, int depth)
        {
            string nodeType = node.GetType().Name;
            var nodeInfo = new RenderedNode(
                        nodeType.Substring(0, nodeType.Count() - 2),
                        depth,
                        node.Name,
                        () => AlignRelativeTo(() => linkX(), nodeWidth(), nodeCount, column,depth));
            onAddAction(nodeInfo, node);
            nodeVisualizationInfo[depth].Add(nodeInfo);

            var subNodes = node.SubNodes.ToArray();
            Func<float> linkPositionX = () => nodeInfo.PositionX;

            for (int nodeIndex = 0; nodeIndex < subNodes.Count(); nodeIndex++)
            {
                if (nodeIndex == 0)
                {
                    var newLink = new LinkBase(nodeInfo, linkPositionX, subNodes.Count());
                    if (nodeVisualizationInfo.ContainsKey(depth + 1))
                    {
                        nodeVisualizationInfo[depth + 1].Add(newLink);
                    }
                    else
                    {
                        nodeVisualizationInfo.Add(depth + 1,
                        new List<IBehaviorVirtualizationPiece>(new[] {
                                                    newLink as IBehaviorVirtualizationPiece,
                        }));
                    }
                }
                AddNode(subNodes[nodeIndex],
                    () => NodeWidth() * (subNodes.Count()),
                    () => NodeWidth(),
                    linkPositionX,
                    subNodes.Count() + 1,
                    nodeIndex,
                    depth + 1);
            }
        }

        private float AlignRelativeTo(Func<float> relativeX, float nodeWidth, int nodeCount, int column, int row)
        {
            var position = relativeX() - (((nodeWidth) * nodeCount) / 2) + ((nodeWidth) * (column + 1));

            if (column > 0)
            {
                if (nodeVisualizationInfo[row][column].PositionX + nodeWidth > position)
                {
                    position += (nodeVisualizationInfo[row][column].PositionX+nodeWidth - position);
                }
            }
            return position;
        }

        #endregion

    }
}
