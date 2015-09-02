using System;
using System.Collections.Generic;
using System.Linq;

namespace Gem.AI.Genetic
{
    public class GeneticAlgorithm
    {

        #region Public Properties

        public double ElitismChance { get; }
        public double CrossOverChance { get; }
        public double MutationChance { get; }
        public double AverageFitness { get; private set; }
        public double HighestFitness { get; private set; }
        public int Generation { get; private set; }
        public List<Genome> NextGeneration { get; private set; } = new List<Genome>();
        
        #endregion

        #region Events

        public EventHandler<EventArgs> OnGenerationEvolved;
        public EventHandler<Genome> OnBirth;
        
        #endregion

        #region Delegates

        private readonly Action<Genome, Random> MutationDelegate;
        private readonly Func<Genome, Genome, Random, Genome> CrossOverDelegate;

        #endregion

        #region Ctor

        public GeneticAlgorithm(int CrossOverChance,
                                int MutationChance,
                                Action<Genome, Random> mutationDelegate,
                                Func<Genome, Genome, Random, Genome> crossoverDelegate)
        {
            this.CrossOverChance = CrossOverChance;
            this.MutationChance = MutationChance;
            ElitismChance = 100 - CrossOverChance;
            MutationDelegate = mutationDelegate;
            CrossOverDelegate = crossoverDelegate;
        }

        #endregion

        #region Public Methods

        public double Evolve(IEnumerable<Genome> genomes)
        {
            NextGeneration.Clear();

            CalculateFitness(genomes);
            Elitism(genomes);
            CrossOver(genomes);
            Mutation();
            CopyGenomes(genomes.ToList());
            Generation++;
            NextGeneration.Clear();

            OnGenerationEvolved?.Invoke(this, EventArgs.Empty);

            return AverageFitness;
        }

        #endregion

        #region Genetic Algorithm Related

        private void Elitism(IEnumerable<Genome> genomes)
        {
            genomes = genomes.OrderByDescending(Creature => Creature.Fitness);
            int NrOfElites = (int)(genomes.Count() * (double)(ElitismChance / 100));
            for (int i = 0; i < NrOfElites; i++)
            {
                NextGeneration.Add(genomes.ElementAt(i));
            }
        }

        private void CalculateFitness(IEnumerable<Genome> genomes)
        {
            HighestFitness = 0;
            AverageFitness = 0;
            foreach (Genome c in genomes)
            {
                AverageFitness += c.Fitness;
                if (c.Fitness > HighestFitness)
                {
                    HighestFitness = c.Fitness;
                }
            }
            AverageFitness /= genomes.Count();

            foreach (Genome c in genomes)
            {
                c.ParentChance = (HighestFitness == 0) ?
                    100 : (c.Fitness / HighestFitness) * 100;
            }
        }

        private Genome Selection()
        {
            NextGeneration = NextGeneration.OrderBy(Creature => Guid.NewGuid()).ToList();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int ParentTreshold = random.Next(0, 100);
            foreach (Genome c in NextGeneration)
            {
                if (c.ParentChance > ParentTreshold)
                {
                    return c;
                }
            }
            return null;
        }

        private void CrossOver(IEnumerable<Genome> genomes)
        {
            int numOfCrossovers = (int)(genomes.Count() * (double)(CrossOverChance / 100));
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int j = 0; j < numOfCrossovers; j++)
            {
                Genome parentA = Selection();
                Genome parentB = Selection();
                var child = CrossOverDelegate(parentA, parentB, random);
                OnBirth?.Invoke(this, child);
                NextGeneration.Add(child);
            }
        }

        private void Mutation()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            foreach (Genome g in NextGeneration)
            {
                if (random.Next(0, 100) < MutationChance)
                {
                    MutationDelegate(g, random);
                }
            }
        }

        private void CopyGenomes(List<Genome> genome)
        {
            for (int i = 0; i < genome.Count(); i++)
            {
                genome[i] = NextGeneration[i];
            }
        }

        #endregion

    }
}

