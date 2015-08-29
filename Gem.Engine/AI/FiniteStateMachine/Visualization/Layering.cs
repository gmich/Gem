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
        private readonly List<List<int>> layer = new List<List<int>>();

        public Layering(State<TStateContext> parentState)
        {
            BreakIntoEdgesAndVertices(parentState, 0);
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
                layer[depth].Add(vertex.NodeReference);
                return new Tuple<IVertex, bool>(vertex, true);
            }
        }

        private void BreakIntoEdgesAndVertices(State<TStateContext> parentState, int depth)
        {
            if (layer.Count <= depth)
            {
                layer.Add(new List<int>());
            }
            IVertex parentVertex = CreateVertex(parentState, depth).Item1;

            foreach (var connectedState in parentState.ConnectedStates())
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
