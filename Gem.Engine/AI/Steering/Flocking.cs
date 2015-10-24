using Gem.Infrastructure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.Engine.AI.Steering
{
    
    public class Flocking
    {
        private readonly List<Agent> agents = new List<Agent>();
        private int flockDistance;

        public Flocking(int flockDistance)
        {
            FlockDistance = flockDistance;
        }

        public int FlockDistance
        {
            get { return flockDistance; }
            set { flockDistance = MathHelper.Max(1, value); }
        }

        public IDisposable AddAgent(Agent agent)
        {
            agents.Add(agent);
            return Disposable.Create(agents, agent);
        }

        private Vector2 Align(IEnumerable<Agent> neighbours, Agent agent)
        {
            return Vector2.Normalize(
                neighbours.Sum(neighbour => neighbour.Velocity)
                / neighbours.Count());
        }

        private Vector2 Cohes(IEnumerable<Agent> neighbours, Agent agent)
        {
            return Vector2.Normalize(
                agent.Position
                - (neighbours.Sum(neighbour => neighbour.Position)
                / neighbours.Count()));
        }

        private Vector2 Separate(IEnumerable<Agent> neighbours, Agent agent)
        {
            return Vector2.Normalize(
                        Vector2.Negate(
                            agent.Position
                            - (neighbours.Sum(neighbour => agent.Position - neighbour.Position)
                            / neighbours.Count())));
        }
        
        public void Assign()
        {
            foreach (var agent in agents)
            {
                agent.VelocityProvider = () =>
                {
                    var neighbours = agents.Where(neighbour => 
                        (neighbour.Position - agent.Position).Length() < FlockDistance);

                    return agent.Velocity
                          + (Align(neighbours, agent) * agent.AlignmentWeight)
                          + (Cohes(neighbours, agent) * agent.CohesionWeight)
                          + (Separate(neighbours, agent) * agent.SeparationWeight);
                };
            }
        }
    }

}