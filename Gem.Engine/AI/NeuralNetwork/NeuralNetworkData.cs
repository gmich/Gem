using System;

namespace Gem.AI.NeuralNetwork
{
    public class NeuralNetworkData
    {
        private NeuralNetworkData(double[][] trainingData, double[][] testData)
        {
            TrainingData = trainingData;
            TestData = testData;
        }
        public double[][] TrainingData { get; }
        public double[][] TestData { get; }

        public static NeuralNetworkData PrepareTrainingData(double[][] allData, double trainDataPercentage,int seed)
        {
            Random rnd = new Random(seed);
            int totRows = allData.Length;
            int numCols = allData[0].Length;
            int trainRows = (int)(totRows * trainDataPercentage);
            int testRows = totRows - trainRows;
            var trainData = new double[trainRows][];
            var testData = new double[testRows][];
            double[][] copy = new double[allData.Length][];

            for (int i = 0; i < copy.Length; ++i)
            {
                copy[i] = allData[i];
            }
            for (int i = 0; i < copy.Length; ++i)
            {
                int r = rnd.Next(i, copy.Length);
                double[] tmp = copy[r];
                copy[r] = copy[i]; copy[i] = tmp;
            }
            for (int i = 0; i < trainRows; ++i)
            {
                trainData[i] = new double[numCols];
                for (int j = 0; j < numCols; ++j)
                {
                    trainData[i][j] = copy[i][j];
                }
            }
            for (int i = 0; i < testRows; ++i)
            {
                testData[i] = new double[numCols];
                for (int j = 0; j < numCols; ++j)
                {
                    testData[i][j] = copy[i + trainRows][j];
                }
            }
            return new NeuralNetworkData(trainData, testData);
        }

    }
}
