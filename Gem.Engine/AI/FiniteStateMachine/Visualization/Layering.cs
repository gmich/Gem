using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.FiniteStateMachine.Visualization
{
    public class Layering<TStateContext>
    {
        private readonly List<IVertex> vertices = new List<IVertex>();
        private readonly List<IEdge> edges = new List<IEdge>();
        private readonly List<State<TStateContext>> states = new List<State<TStateContext>>();
        private readonly List<List<int>> gridBuilder = new List<List<int>>();
        private int[,] grid;
        public Layering(State<TStateContext> parentState)
        {
            BreakIntoEdgesAndVertices(parentState, 0);
            AssignPlaceHolderVertices();
            GenerateGrid();
        }

        private void NodeOrdering()
        {

        }

        /// <summary>
        /// Makes sure that the Y axis has enough space to connect the edges
        /// </summary>
        private void AssignPlaceHolderVertices()
        {
            foreach (var edge in edges)
            {
                Point sourceLayer = PositionInLayer(edge.Source.NodeReference);
                Point targetLayer = PositionInLayer(edge.Target.NodeReference);
                int distance = sourceLayer.Y - targetLayer.Y;
                if (Math.Abs(distance) > 1)
                {
                    int position = sourceLayer.Y;
                    //assign placeholder vertices between them
                    while (position != distance)
                    {
                        position = Approach(position, distance);
                        gridBuilder[targetLayer.Y + position].Add(Vertex.PlaceHolder.NodeReference);
                    }
                }
            }

        }

        private void GenerateGrid()
        {
            grid = new int[gridBuilder.Count, gridBuilder.Max(x => x.Count)];

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[y, x] = -1;
                }
            }
            for (int y = 0; y < gridBuilder.Count; y++)
            {
                for (int x = 0; x < gridBuilder[y].Count; x++)
                {
                    grid[y, x] = gridBuilder[y][x];
                }
            }
        }

        private int Approach(int start, int end)
        {
            if (start == end) return start;
            return (start > end) ?
                 start - 1 : start + 1;
        }

        private Point PositionInLayer(int nodeReference)
        {
            for (int y = 0; y < gridBuilder.Count; y++)
            {
                for (int x = 0; x < gridBuilder[y].Count; x++)
                {
                    if (gridBuilder[y][x] == nodeReference)
                    {
                        return new Point(x, y);
                    }
                }
            }
            throw new StateVisualizationException("The node reference doesn't match an element in any layer");
        }

        private Tuple<IVertex, bool> CreateVertex(State<TStateContext> state, int depth)
        {
            IVertex vertex;

            if (states.Contains(state))
            {
                //retrieve the vertex                   
                vertex = vertices.Where(v => v.NodeReference == states.FindIndex(x => x == state)).First();
                return new Tuple<IVertex, bool>(vertex, false);
            }
            else
            {
                //add a new one
                states.Add(state);
                vertex = new Vertex(states.Count - 1);
                vertices.Add(vertex);
                gridBuilder[depth].Add(vertex.NodeReference);
                return new Tuple<IVertex, bool>(vertex, true);
            }
        }

        private void BreakIntoEdgesAndVertices(State<TStateContext> parentState, int depth)
        {
            if (gridBuilder.Count <= depth)
            {
                gridBuilder.Add(new List<int>());
            }
            var parentCreationResult = CreateVertex(parentState, depth);
            IVertex parentVertex = parentCreationResult.Item1;
            var connectedStates = parentState.ConnectedStates();

            if (connectedStates.Count() == 0 && !parentCreationResult.Item2)
            {
                gridBuilder.Remove(gridBuilder.Last());
            }

            foreach (var connectedState in connectedStates)
            {
                var vertexCreationResult = CreateVertex(connectedState, depth);
                IVertex targetVertex = vertexCreationResult.Item1;

                //if the node is not visited, call the function recursively
                bool goDeeper = vertexCreationResult.Item2;

                IEdge edge = null;
                //if the target vertex already has an edge on the parent vertex mark as bi-directional
                edge = edges.Where(x => x.Source == targetVertex && x.Target == parentVertex).FirstOrDefault();
                if (edge != null)
                {
                    edge.Direction = Direction.BiDirectional;
                }
                else
                {
                    edge = new Edge(parentVertex, targetVertex);
                    edges.Add(edge);
                    if (parentVertex == targetVertex)
                    {
                        edge.Direction = Direction.SelfDirectional;
                    }
                }
                if (goDeeper)
                {
                    BreakIntoEdgesAndVertices(connectedState, depth + 1);
                }
            }
        }
    }
}
