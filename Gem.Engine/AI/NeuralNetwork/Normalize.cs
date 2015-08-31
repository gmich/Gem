using System;

namespace Gem.AI.NeuralNetwork
{
    public sealed class Normalize
    {

        public static void Multiple(double[][] data, Action<double[][], int> normalizationMethod, params int[] columns)
        {
            foreach (var column in columns)
            {
                normalizationMethod(data, column);
            }
        }

        public static void Gauss(double[][] data, int column)
        {
            int j = column;
            double sum = 0.0;
            for (int i = 0; i < data.Length; ++i) sum += data[i][j];
            double mean = sum / data.Length;
            double sumSquares = 0.0;
            for (int i = 0; i < data.Length; ++i)
            {
                sumSquares += (data[i][j] - mean) * (data[i][j] - mean);
            }
            double stdDev = Math.Sqrt(sumSquares / data.Length);
            for (int i = 0; i < data.Length; ++i)
            {
                data[i][j] = (data[i][j] - mean) / stdDev;
            }
        }

        public static void MinMax(double[][] data, int column)
        {
            int j = column;
            double min = data[0][j];
            double max = data[0][j];

            for (int i = 0; i < data.Length; ++i)
            {
                if (data[i][j] < min) min = data[i][j];
                if (data[i][j] > max) max = data[i][j];
            }
            double range = max - min;
            if (range == 0.0)
            {
                for (int i = 0; i < data.Length; ++i)
                {
                    data[i][j] = 0.5;
                }
                return;
            }
            for (int i = 0; i < data.Length; ++i)
            {
                data[i][j] = (data[i][j] - min) / range;
            }
        }

    }

}
