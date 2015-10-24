using Gem.Engine.AI.NeuralNetwork;
using System;


namespace Gem.Engine.AI.Genetic
{
    public class EvolvableBrain : Genome
    {

        public ANeuralNetwork NeuralNetwork { get; }
        public Func<ANeuralNetwork> NeuralNetworkFactory { get; }

        public EvolvableBrain(Func<ANeuralNetwork> neuralNetworkFactory)
        {
            NeuralNetworkFactory = neuralNetworkFactory;
            NeuralNetwork = neuralNetworkFactory();
        }

        public static void Mutation(Genome g, Random random)
        {
            var genome = (g as EvolvableBrain);
            int MutationPoint = random.Next(0, genome.NeuralNetwork.TotalNeurons);
            double[] Weights = genome.NeuralNetwork.GetWeights();
            Weights[MutationPoint] = random.NextDouble();
            genome.NeuralNetwork.SetWeights(Weights);
        }

        public static Genome CrossOver(Genome parentA, Genome parentB, Random random)
        {
            double[] ParentAWeights = (parentA as EvolvableBrain).NeuralNetwork.GetWeights();
            double[] ParentBWeights = (parentA as EvolvableBrain).NeuralNetwork.GetWeights();

            double[] ChildWeights = new double[ParentAWeights.Length];

            int CrossOverPoint = random.Next(0, ParentAWeights.Length);

            for (int i = 0; i < CrossOverPoint; i++)
            {
                ChildWeights[i] = ParentAWeights[i];
            }
            for (int i = CrossOverPoint; i < ParentAWeights.Length; i++)
            {
                ChildWeights[i] = ParentBWeights[i];
            }
            EvolvableBrain child = new EvolvableBrain((parentA as EvolvableBrain).NeuralNetworkFactory);
            child.NeuralNetwork.SetWeights(ChildWeights);

            return child;
        }
    }
}
