using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

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

        public static NeuralNetworkData PrepareTrainingData(double[][] allData, double trainDataPercentage, int seed)
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

        public static double[][] Convert(string[] lines, char separator)
        {
            if (lines.Count() == 0)
            {
                throw new Exception("No lines found for convertion");
            }
            var tokens = lines[0].Split(' ');
            int dependetVariableIndex = tokens.Count() - 1; //assummed to be the last one
            var numericValues = new Dictionary<int, int>();

            lines = ParsingUtilities.EncodeColumnInLine(lines, dependetVariableIndex, separator, Encode.DependentVariable);
            for (int i = dependetVariableIndex - 1; i >= 0; i--)
            {
                try
                {
                    double.Parse(tokens[i], CultureInfo.InvariantCulture);
                    numericValues.Add(i, 0);
                }
                catch
                {
                    var lengthPriorEncoding = lines[0].Split(separator).Count();
                    lines = ParsingUtilities.EncodeColumnInLine(lines, i, separator, Encode.Predictor);
                    var lengthPostEncoding = lines[0].Split(separator).Count();

                    var growth = lengthPostEncoding - lengthPriorEncoding;
                    numericValues.Keys.ToList().ForEach(key => numericValues[key] += growth);
                }
            }
            var numericalData = ParsingUtilities.ConvertToNumeric(lines, separator);
            Normalize.Multiple(numericalData, Normalize.MinMax, numericValues.Values.ToArray());

            return numericalData;
        }
    }
}
