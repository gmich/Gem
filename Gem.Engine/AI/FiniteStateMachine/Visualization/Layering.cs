using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.FiniteStateMachine.Visualization
{
    public class Layering<TStateContext>
    {
        private readonly List<Vertex> vertices = new List<Vertex>();
        private readonly List<Edge> edges = new List<Edge>();
        private readonly List<State<TStateContext>> states = new List<State<TStateContext>>();
        private readonly List<List<int>> gridBuilder = new List<List<int>>();
        private int[,] grid;

        public Layering(State<TStateContext> parentState)
        {
            BreakIntoEdgesAndVertices(parentState, 0);
            AssignPlaceHolderVertices();
            GenerateGrid();
            //NodeOrdering();
            //CoordinateAssignment();
        }

        private class OrderedVertex
        {
            public OrderedVertex(Vertex vertex, IEnumerable<Edge> edges)
            {
                Vertex = vertex;
                Edges = edges;
            }
            public Vertex Vertex { get; }

            public IEnumerable<Edge> Edges { get; }

            public bool Checked { get; set; } = false;
        }

        private IEnumerable<Edge> GetEdgesByLayer(int layerY)
        {
            return edges.Where(edge => PositionInLayer(edge.Source.NodeReference).Y == layerY);
        }

        private IEnumerable<Vertex> GetVerticesByLayer(int layerY)
        {
            return vertices.Where(vertex => PositionInLayer(vertex.NodeReference).Y == layerY);
        }

        private IEnumerable<Edge> GetEdgesByVertex(int vertexReference)
        {
            return edges.Where(edge => (edge.Source.NodeReference == vertexReference));
        }

        private void NodeOrdering()
        {
            Dictionary<int, OrderedVertex[]> orderedVertices = new Dictionary<int, OrderedVertex[]>();
            for (int layer = 1; layer < grid.GetLength(0); layer++)
            {
                var verticesByLayer = GetVerticesByLayer(layer);

                orderedVertices
                    .Add(layer, verticesByLayer.Select(vertex =>
                    new OrderedVertex(vertex, GetEdgesByVertex(vertex.NodeReference)))
                    .ToArray());
            }

            for(int row=1;row<orderedVertices.Count(); row++)
            {
                var nextRow = orderedVertices[row];
                var currentRow = orderedVertices[row - 1];

                for (int vertex = 0; vertex < currentRow.Count(); vertex++)
                {
                    var vertexPosition = PositionInLayer(currentRow[vertex].Vertex.NodeReference);
                    foreach (var edge in currentRow[vertex].Edges)
                    {
                        var targetPosition = PositionInLayer(edge.Target.NodeReference);
                       // if(targetPosition.X-vertexPosition.X)
                    }
                }

            }
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

        private Tuple<Vertex, bool> CreateVertex(State<TStateContext> state, int depth)
        {
            Vertex vertex;

            if (states.Contains(state))
            {
                //retrieve the vertex                   
                vertex = vertices.Where(v => v.NodeReference == states.FindIndex(x => x == state)).First();
                return new Tuple<Vertex, bool>(vertex, false);
            }
            else
            {
                //add a new one
                states.Add(state);
                vertex = new Vertex(states.Count - 1);
                vertices.Add(vertex);
                gridBuilder[depth].Add(vertex.NodeReference);
                return new Tuple<Vertex, bool>(vertex, true);
            }
        }

        private void BreakIntoEdgesAndVertices(State<TStateContext> parentState, int depth)
        {
            if (gridBuilder.Count <= depth)
            {
                gridBuilder.Add(new List<int>());
            }
            var parentCreationResult = CreateVertex(parentState, depth);
            Vertex parentVertex = parentCreationResult.Item1;
            var connectedStates = parentState.ConnectedStates();

            if (connectedStates.Count() == 0 && !parentCreationResult.Item2)
            {
                gridBuilder.Remove(gridBuilder.Last());
            }

            foreach (var connectedState in connectedStates)
            {
                var vertexCreationResult = CreateVertex(connectedState, depth);
                Vertex targetVertex = vertexCreationResult.Item1;

                //if the node is not visited, call the function recursively
                bool goDeeper = vertexCreationResult.Item2;

                Edge edge = null;
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
