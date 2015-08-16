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
        private Dictionary<int, List<IBehaviorVirtualizationPiece>> nodeVisualizationInfo;
        private float nodeWidth;
        private float nodeSpan = 20.0f;
        private int nodeHeight = 70;

        #endregion

        #region Ctor

        public TreeVisualizer(Texture2D nodeBackgroundTexture, Texture2D lineTexture, Texture2D linkTexture, SpriteFont nodeInfoFont)
        {
            this.nodeBackgroundTexture = nodeBackgroundTexture;
            this.lineTexture = lineTexture;
            this.linkTexture = linkTexture;
            this.nodeInfoFont = nodeInfoFont;
        }

        #endregion

        #region Prepare Tree 

        public void Prepare<AIContext>(IBehaviorNode<AIContext> root)
        {
            var analyzer = new TreeAnalyzer<AIContext>(root);
            nodeVisualizationInfo = analyzer.AnalyzedTree;

            //find the largest name and calculate the node width
            int largestCharacterCount = 0;
            foreach (var noddeInfo in nodeVisualizationInfo.SelectMany(x => x.Value))
            {
                largestCharacterCount = CountLargestNameCharacters(noddeInfo as RenderedNode, largestCharacterCount);
            }
            nodeWidth = CalculateNodeWidth(largestCharacterCount + 2);

            int itemsPerRow = 1;
            foreach (var node in nodeVisualizationInfo.Values)
            {
                int countNodes = node.Where(item => item is RenderedNode).Count();
                if (countNodes > itemsPerRow)
                {
                    itemsPerRow = countNodes;
                }
            }
            float treeWidth = itemsPerRow * nodeWidth;
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

        private int CountLargestNameCharacters(RenderedNode node, int currentLargest)
        {
            int largestCharacterCount = currentLargest;
            if (node != null)
            {
                int nameCharacterCount = node.Name.Count();
                if (nameCharacterCount > largestCharacterCount)
                {
                    largestCharacterCount = nameCharacterCount;
                }

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

            batch.Draw(lineTexture,
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

        private void DrawNodeBackground(SpriteBatch batch, Vector2 position, Color nodeBackgroundColor, int sizeX)
        {

            batch.Draw(nodeBackgroundTexture,
                new Rectangle(
                    (int)position.X - sizeX / 2,
                    (int)position.Y,
                    sizeX,
                    nodeHeight),
                null,
                nodeBackgroundColor,
                0.0f,
                Vector2.Zero,
                SpriteEffects.None,
                0.1f);
        }

        private void DrawNodeContent(SpriteBatch batch, Vector2 position, string info, Color color)
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

        public void RenderTree(SpriteBatch batch)
        {
            int row = 0;
            int rowHeight = 50;
            int linkSize = 10;
            foreach (var treeRow in nodeVisualizationInfo.Values)
            {
                Vector2 linkLocation = Vector2.Zero;
                foreach (var nodeInfo in treeRow)
                {
                    var node = nodeInfo as RenderedNode;
                    if (node != null)
                    {
                        node = nodeInfo as RenderedNode;
                        var nodePosition = new Vector2(node.PositionX, rowHeight * (row + 1));

                        DrawNodeBackground(batch, nodePosition, new Color(41, 128, 185), (int)nodeWidth);
                        if (linkLocation != Vector2.Zero)
                            DrawLine(batch, linkLocation, nodePosition, new Color(189, 195, 199), 2);

                        string type = "(" + node.BehaviorType + ")";
                        var stringTypeSize = StringSize(type);
                        var stringNameSize = StringSize(node.Name);
                        DrawNodeContent(batch, nodePosition - new Vector2(stringTypeSize.X / 2, 0), type, new Color(236, 240, 241));
                        DrawNodeContent(batch, nodePosition - new Vector2(stringNameSize.X / 2, -stringNameSize.Y - 2), node.Name, new Color(189, 195, 199));
                     
                    }
                    else
                    {
                        var link = nodeInfo as LinkBase;
                        linkLocation = new Vector2(link.PositionX, rowHeight * row + rowHeight / 2);
                        DrawLink(batch, linkLocation, new Color(236, 240, 241), linkSize);
                    }
                }
                row += 2;
            }
        }


        #endregion

    }
}

