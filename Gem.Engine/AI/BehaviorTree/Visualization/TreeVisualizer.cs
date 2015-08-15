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
            nodeWidth = CalculateNodeWidth(largestCharacterCount);
            float treeWidth = nodeVisualizationInfo.Values.Select(x => x.Count).Max() * nodeWidth;
            analyzer.SetNodeWidth(nodeWidth);
            analyzer.SetTreeWidth(treeWidth);

            //find any overlapping nodes and recalculate the tree's row width
            foreach (var treeRow in nodeVisualizationInfo.Values)
            {
                int nodesIterated = 0;
                float lastPositionX = 0;
                foreach (var nodeInfo in treeRow)
                {
                    var link = nodeInfo as LinkBase;
                    if (link != null)
                    {
                        nodesIterated = 0;
                    }
                    else
                    {
                        var node = (nodeInfo as RenderedNode);
                        if (nodesIterated == 0 && lastPositionX != 0)
                        {
                            if (lastPositionX > node.PositionX)
                            {
                                treeWidth += lastPositionX - node.PositionX;
                                analyzer.SetTreeWidth(treeWidth);
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
                0);
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
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        private void DrawNodeBackground(SpriteBatch batch, Vector2 position, Color nodeBackgroundColor, int size)
        {

            batch.Draw(nodeBackgroundTexture,
                new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    size,
                    size),
                null,
                nodeBackgroundColor,
                0.0f,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        private void DrawNodeContent(SpriteBatch batch,Vector2 position, string info, Color color)
        {
            batch.DrawString(nodeInfoFont,
                            info,
                            position,
                            color);

        }

        public void RenderTree(SpriteBatch batch)
        {
            int row = 0;
            int rowHeight = 10;
            int linkSize = 5;
            foreach (var treeRow in nodeVisualizationInfo.Values)
            {
                Vector2 linkLocation = Vector2.Zero;
                foreach (var nodeInfo in treeRow)
                {
                    var link = nodeInfo as LinkBase;
                    if (link != null)
                    {
                        linkLocation = new Vector2(link.Center, rowHeight * row);
                        DrawLink(batch, linkLocation, link.Color, linkSize);

                    }
                    else
                    {
                        var node = nodeInfo as RenderedNode;
                            var nodePosition = new Vector2(nodeInfo.PositionX, rowHeight * row);
                        DrawLine(batch, linkLocation, nodePosition, Color.Black, 2);
                        DrawNodeBackground(batch, nodePosition, node.Color, (int)nodeWidth);
                        DrawNodeContent(batch, nodePosition, node.Name, Color.Black);
                    }
                }
                row++;
            }
        }


        #endregion

    }
}

