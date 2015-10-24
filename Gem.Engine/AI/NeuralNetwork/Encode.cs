using System;
using System.Collections.Generic;
using System.IO;

namespace Gem.Engine.AI.NeuralNetwork
{

    public sealed class Encode
    {

        public static string Predictor(int index, int N, char separator)
        {
            // If N = 3 and index = 0 -> 1,0. 
            // If N = 3 and index = 1 -> 0,1. 
            // If N = 3 and index = 2 -> -1,-1.
            if (N == 2) // Special case. 
            {
                return (index == 0) ?
                    "-1" : "1";
            }
            int[] values = new int[N - 1];

            if (index == N - 1)

            {
                for (int i = 0; i < values.Length; ++i)
                {
                    values[i] = -1;
                }
            }
            else
            {
                values[index] = 1;
            }
            string s = values[0].ToString();
            for (int i = 1; i < values.Length; ++i)
            {
                s += separator.ToString() + values[i];
            }
            return s;
        }

        public static string DependentVariable(int index, int N, char separator)
        {
            int[] values = new int[N];
            values[index] = 1;
            string s = values[0].ToString();
            for (int i = 1; i < values.Length; ++i)
            {
                s += separator.ToString() + values[i];
            }
            return s;
        }

    }
}

