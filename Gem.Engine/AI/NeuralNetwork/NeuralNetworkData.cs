using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace Gem.AI.NeuralNetwork
{
    public class NeuralNetworkData
    {
        private readonly List<Dictionary<string, int>> dataTokens = new List<Dictionary<string, int>>();
        private readonly string[] rawData;
        private int dependetVariableIndex;

        public NeuralNetworkData(string[] lines, char separator)
        {
            rawData = lines.ToArray();

            var tokens = lines[0].Split(separator);
            dataTokens = tokens
                        .Select((token, index) => GetTokens(lines, index, separator))
                        //.AsParallel()
                        .ToList();

            ConvertedData = Convert(lines, separator);
        }

        public double[][] ConvertedData { get; }
        public double[][] TrainingData { get; private set; }
        public double[][] TestData { get; private set; }

        public void PrepareTrainingSet(double[][] encodedData, double trainDataPercentage, int seed)
        {
            Random rnd = new Random(seed);
            int totRows = encodedData.Length;
            int numCols = encodedData[0].Length;
            int trainRows = (int)(totRows * trainDataPercentage);
            int testRows = totRows - trainRows;
            var trainData = new double[trainRows][];
            var testData = new double[testRows][];
            double[][] copy = new double[encodedData.Length][];

            for (int i = 0; i < copy.Length; ++i)
            {
                copy[i] = encodedData[i];
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
            TrainingData = trainData;
            TestData = testData;
        }

        private static Dictionary<string, int> GetTokens(string[] lines, int column, char separator)
        {
            Dictionary<string, int> tokensLookup = new Dictionary<string, int>();
            string[] tokens = null;
            int itemNum = 0;

            foreach (var line in lines)
            {
                tokens = line.Split(separator);
                if (tokensLookup.ContainsKey(tokens[column]) == false)
                {
                    tokensLookup.Add(tokens[column], itemNum++);
                }
            };
            return tokensLookup;
        }

        private double[][] Convert(string[] lines, char separator)
        {
            if (lines.Count() == 0)
            {
                throw new Exception("No lines found for convertion");
            }
            var tokens = lines[0].Split(' ');
            dependetVariableIndex = tokens.Count() - 1; //assummed to be the last one
            var numericValues = new Dictionary<int, int>();

            lines = EncodeColumnInLine(lines, dependetVariableIndex, separator, Encode.DependentVariable);
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
                    lines = EncodeColumnInLine(lines, i, separator, Encode.Predictor);
                    var lengthPostEncoding = lines[0].Split(separator).Count();

                    var growth = lengthPostEncoding - lengthPriorEncoding;
                    numericValues.Keys.ToList().ForEach(key => numericValues[key] += growth);
                }
            }

            var numericalData = ParsingUtilities.ConvertToNumeric(lines, separator);
            Normalize.Multiple(numericalData, Normalize.MinMax, numericValues.Select(entry=> entry.Key+entry.Value).ToArray());

            return numericalData;
        }

        private string[] EncodeColumnInLine(string[] lines, int column, char separator, Func<int, int, char, string> encodingType)
        {
            var tokenLookup = dataTokens.ElementAt(column);
            int N = tokenLookup.Count; // Number of distinct strings.
            var resultStream = new List<string>();

            string stream = null;
            foreach (var line in lines)
            {
                stream = string.Empty;
                string[] tokens = line.Split(separator);
                for (int i = 0; i < tokens.Length; ++i)
                {
                    if (i == column)
                    {
                        stream += encodingType(tokenLookup[tokens[i]], N, separator) + separator;
                    }
                    else
                    {
                        stream += tokens[i] + separator;
                    }
                }
                stream = stream.Remove(stream.Length - 1);
                resultStream.Add(stream);
            }
            return resultStream.ToArray();
        }

        public string DecodeOutput(params double[] encodedOutput)
        {
            var tokenLookup = dataTokens.ElementAt(dependetVariableIndex);

            encodedOutput = ParsingUtilities.Flatten(encodedOutput);

            for (int i = 0; i < encodedOutput.Count(); i++)
            {
                if (encodedOutput[i] == 1)
                {
                    return tokenLookup.Where(x => x.Value == i).Select(x => x.Key).First();
                }
            }
            throw new Exception("Unable to decode data");
        }
    

    }

}
