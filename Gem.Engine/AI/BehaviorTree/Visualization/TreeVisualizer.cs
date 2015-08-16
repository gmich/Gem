using Gem.Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.BehaviorTree.Visualization
{
    public class TreeVisualizer
    {

        #region Fields

        private readonly Texture2D lineTexture;
        private readonly Texture2D linkTexture;
        private readonly Texture2D nodeBackgroundTexture;
        private readonly SpriteFont nodeInfoFont;
        private readonly List<IVirtualizationItem> virtualizationItems = new List<IVirtualizationItem>();

        private readonly float nodeSpan = 20.0f;
        private readonly int rowHeight = 70;
        private readonly int linkSize = 10;
        private readonly int lineWidth = 2;
        private readonly float initialRow = 0.5f;
        private readonly float rowStep = 1.5f;

        private float treeWidth;
        private INodePainter painter;
        private Dictionary<int, List<IBehaviorVirtualizationPiece>> nodeVisualizationInfo;
        private readonly Dictionary<BehaviorResult, Texture2D> BehaviorTextures
            = new Dictionary<BehaviorResult, Texture2D>
            {
                [BehaviorResult.Failure] = null,
                [BehaviorResult.Success] = null,
                [BehaviorResult.Running] = null,
            };
        private float nodeWidth;

        public Vector2 TreeSize => new Vector2(
            treeWidth,
            ((nodeVisualizationInfo.Count * rowStep) + initialRow) * rowHeight);

        #endregion

        #region Ctor

        public TreeVisualizer(
            Texture2D nodeBackgroundTexture,
            Texture2D lineTexture,
            Texture2D linkTexture,
            Texture2D running,
            Texture2D success,
            Texture2D failure,
            SpriteFont nodeInfoFont)
        {
            this.nodeBackgroundTexture = nodeBackgroundTexture;
            this.lineTexture = lineTexture;
            this.linkTexture = linkTexture;
            this.nodeInfoFont = nodeInfoFont;
            BehaviorTextures[BehaviorResult.Failure] = failure;
            BehaviorTextures[BehaviorResult.Success] = success;
            BehaviorTextures[BehaviorResult.Running] = running;
        }

        #endregion

        #region Prepare Tree 

        public void Prepare<AIContext>(INodePainter painter, IBehaviorNode<AIContext> root)
        {
            this.painter = painter;
            var analyzer = new TreeAnalyzer<AIContext>(painter.Paint, root);
            nodeVisualizationInfo = analyzer.AnalyzedTree;
            //find the largest name and calculate the node width
            int largestCharacterCount = 0;
            foreach (var noddeInfo in nodeVisualizationInfo.SelectMany(x => x.Value))
            {
                largestCharacterCount = CountLargestNameCharacters(noddeInfo as RenderedNode, largestCharacterCount);
            }
            nodeWidth = CalculateNodeWidth(largestCharacterCount + 3);

            int itemsPerRow = 1;
            foreach (var node in nodeVisualizationInfo.Values)
            {
                int countNodes = node.Where(item => item is RenderedNode).Count();
                if (countNodes > itemsPerRow)
                {
                    itemsPerRow = countNodes;
                }
            }
            treeWidth = itemsPerRow * nodeWidth;
            analyzer.SetNodeWidth(nodeWidth + nodeSpan);
            analyzer.SetTreeWidth(treeWidth);

            //find any overlapping nodes and recalculate the tree's row width
            foreach (var treeRow in nodeVisualizationInfo.Values)
            {
                int nodesIterated = 0;
                float lastPositionX = 0;
                LinkBase lastLink = null;
                foreach (var nodeInfo in treeRow)
                {
                    var link = nodeInfo as LinkBase;
                    if (link != null)
                    {
                        lastLink = link;
                        nodesIterated = 0;
                    }
                    else
                    {
                        var node = (nodeInfo as RenderedNode);
                        node.onTriggered += (sender, args) =>
                        {
                            var senderNode = sender as RenderedNode;
                            AddBehaviorCover(senderNode, args);
                            AddIcon(senderNode, senderNode.BehaviorStatus);
                        };
                        if (nodesIterated == 0 && lastPositionX != 0)
                        {
                            if (lastPositionX > node.PositionX)
                            {
                                lastLink.LinkedNode.OffsetX += lastPositionX - node.PositionX + nodeSpan;
                            }
                        }
                        lastPositionX = node.PositionX + nodeWidth;
                        nodesIterated++;
                    }
                }
            }
        }

        private void AddBehaviorCover(RenderedNode node, EventArgs args)
        {
            var alreadyRegisteredNode = virtualizationItems.Where(x => x.Node == node).FirstOrDefault();
            if (alreadyRegisteredNode != null)
            {
                alreadyRegisteredNode.Reset();
                return;
            }

            var triggeredNode = new TriggeredNodeCover(
                nodeBackgroundTexture,
                node,
                () => new Vector2(node.PositionX, ((node.Row * rowStep) + initialRow) * rowHeight),
                2.0d,
                painter.Triggered);
            virtualizationItems.Add(triggeredNode);
        }

        private int CountLargestNameCharacters(RenderedNode node, int currentLargest)
        {
            int largestCharacterCount = currentLargest;
            if (node != null)
            {
                largestCharacterCount = Math.Max(
                            Math.Max(node.Name.Count(), currentLargest),
                            node.BehaviorType.Count());
            }
            return largestCharacterCount;
        }

        private float CalculateNodeWidth(int characters)
        {
            return
            nodeInfoFont
            .MeasureString(new String(Enumerable.Repeat('_', characters).ToArray())).X;
        }

        private Vector2 StringSize(string str)
        {
            return nodeInfoFont.MeasureString(str);
        }

        private void AddIcon(RenderedNode node, BehaviorResult? result)
        {
            if (result == null) return;

            var texture = BehaviorTextures[result.Value];

            var registeredItem = virtualizationItems
                    .Where(item => item is NodeStatusIcon)
                    .Where(x => x.Node == node)
                    .FirstOrDefault();

            if (registeredItem != null)
            {
                virtualizationItems.Remove(registeredItem);
            }

            virtualizationItems.Add(new NodeStatusIcon(texture, 
                node,
                () => new Vector2(node.PositionX, ((node.Row * rowStep) + initialRow) * rowHeight),
                2.0f));

        }
        #endregion

        #region Render Tree

        private void DrawLine(SpriteBatch batch, Vector2 start, Vector2 end, Color lineColor, int lineWidth)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            batch.Draw(lineTexture,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    lineWidth),
                null,
                lineColor,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0.0f);
        }

        private void DrawLink(SpriteBatch batch, Vector2 center, Color linkColor, int size)
        {

            batch.Draw(linkTexture,
                new Rectangle(
                    (int)center.X - size / 2,
                    (int)center.Y - size / 2,
                    size,
                    size),
                null,
                linkColor,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.2f);
        }
        
        private void DrawNodeBackground(SpriteBatch batch, Vector2 position, Color nodeBackgroundColor, int sizeX, int sizeY, float layer = 0.1f)
        {

            batch.Draw(nodeBackgroundTexture,
                new Rectangle(
                    (int)position.X - sizeX / 2,
                    (int)position.Y,
                    sizeX,
                    sizeY),
                null,
                nodeBackgroundColor,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                layer);
        }

        private void DrawString(SpriteBatch batch, Vector2 position, string info, Color color)
        {
            batch.DrawString(nodeInfoFont,
                            info,
                            position,
                            color,
                            0.0f,
                            Vector2.Zero,
                            1.0f,
                            SpriteEffects.None,
                            0.3f);
        }

        private void DrawNodeContent(RenderedNode node, Vector2 nodePosition, SpriteBatch batch)
        {
            string nodeName = node.Name;
            string nodeType = node.BehaviorType;

            var nodeNameSize = StringSize(nodeName);
            var nodeTypeSize = StringSize(nodeType);

            DrawString(batch, nodePosition - new Vector2(nodeTypeSize.X / 2, -rowHeight / 2), nodeType, painter.NodeBehaviorType);
            DrawNodeBackground(batch, nodePosition, painter.NodeTitleBackground, (int)nodeWidth, (int)nodeTypeSize.Y, 0.11f);
            DrawString(batch, nodePosition - new Vector2(nodeNameSize.X / 2, 0), nodeName, painter.NodeName);
        }

        public void Update(double timeDelta)
        {
            for (int i = 0; i < virtualizationItems.Count; i++)
            {
                virtualizationItems[i].Update(timeDelta);
                if (!virtualizationItems[i].IsActive)
                {
                    virtualizationItems.RemoveAt(i);
                    i++;
                }
            }
        }
        public void RenderTree(SpriteBatch batch)
        {
            float row = initialRow;
            foreach (var treeRow in nodeVisualizationInfo.Values)
            {
                Vector2 linkLocation = Vector2.Zero;
                foreach (var nodeInfo in treeRow)
                {
                    var node = nodeInfo as RenderedNode;
                    if (node != null)
                    {
                        node = nodeInfo as RenderedNode;
                        var nodePosition = new Vector2(node.PositionX, rowHeight * (row));

                        DrawNodeBackground(batch, nodePosition, painter.NodeBackground, (int)nodeWidth, rowHeight);
                        if (linkLocation != Vector2.Zero)
                        {
                            DrawLine(batch, linkLocation, nodePosition, painter.Line, lineWidth);
                        }
                        DrawNodeContent(node, nodePosition, batch);
                    }
                    else
                    {
                        var link = nodeInfo as LinkBase;
                        linkLocation = new Vector2(link.PositionX, rowHeight * (row - initialRow));
                        DrawLink(batch, linkLocation, painter.Link, linkSize);
                    }
                }
                row += rowStep;
            }

            foreach (var item in virtualizationItems)
            {
                item.Draw(batch, item.Position, (int)nodeWidth, rowHeight);
            }
        }


        #endregion

    }
}

