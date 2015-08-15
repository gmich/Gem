using Microsoft.Xna.Framework;
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
        private readonly int linkSize = 4;

        private float nodeWidth;
        private float treeWidth;

        private Func<float> TreeWidth => () => treeWidth;
        private Func<float> NodeWidth => () => nodeWidth;

        #endregion

        #region Ctor

        public TreeAnalyzer(IBehaviorNode<AIContext> root)
        {
            var rootInfo = new RenderedNode(
                   root.GetType().Name,
                   nameof(root),
                   () => AlignToCenterOfX(TreeWidth() / 2));

            nodeVisualizationInfo.Add(0,
               new List<IBehaviorVirtualizationPiece>(new[] { rootInfo }));

            var subNodes = root.SubNodes.ToArray();
            LinkBase link = new LinkBase(
                () => rootInfo.PositionX + NodeWidth() / 2,
                subNodes.Count(),
                linkSize);

            for (int nodeIndex = 0; nodeIndex < subNodes.Count(); nodeIndex++)
            {
                AddNode(subNodes[nodeIndex],
                    () => TreeWidth(),
                    link,
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

        private void AddNode(IBehaviorNode<AIContext> node, Func<float> nodeWidth, LinkBase link, int column, int depth)
        {
            var nodeInfo = new RenderedNode(
                        node.GetType().Name,
                        nameof(node),
                        () => AlignRelativeTo(link.Center, nodeWidth(), link.NodeCount, column));

            if (nodeVisualizationInfo.ContainsKey(depth))
            {
                nodeVisualizationInfo[depth].Add(nodeInfo);
            }
            else
            {
                nodeVisualizationInfo.Add(depth,
                    new List<IBehaviorVirtualizationPiece>(new[] {
                        link as IBehaviorVirtualizationPiece,
                        nodeInfo
                    }));
            }

            var subNodes = node.SubNodes.ToArray();
            for (int nodeIndex = 0; nodeIndex < subNodes.Count(); nodeIndex++)
            {
                AddNode(subNodes[nodeIndex],
                    () => subNodes.Count() * NodeWidth(),
                    new LinkBase(() => nodeInfo.PositionX + (NodeWidth() / 2), subNodes.Count(), linkSize),
                    nodeIndex,
                    depth);
            }
        }

        private float NodePosition(float rowWidth, int column, int nodeCount)
        {
            return ((rowWidth / (nodeCount + 1)) * column) - (nodeWidth / 2);
        }
        private float AlignToCenterOfX(float XtoAlignTo)
        {
            return XtoAlignTo - (nodeWidth / 2);
        }

        private float AlignRelativeTo(float relativeX, float nodeWidth, int nodeCount, int column)
        {
            return relativeX - ((nodeWidth * nodeCount) / 2) - ((nodeWidth / 2) * column);
        }

        #endregion

    }
}
