using System;

namespace Gem.Engine.AI.NeuralNetwork
{
    public class Activation
    {
        public static double HyperTan(double x)
        {
            if (x < -20.0)
            {
                return -1.0;
            }
            else if (x > 20.0)
            {
                return 1.0;
            }
            else
            {
                return Math.Tanh(x);
            }
        }

        public static double[] SoftmaxNaive(double[] oSums)
        {
            double denom = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
            {
                denom += Math.Exp(oSums[i]);
            }
            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
            {
                result[i] = Math.Exp(oSums[i]) / denom;
            }
            return result;
        }

        /// <summary>
        /// Use it when the output values ==2
        /// </summary>
        public static double LogSigmoid(double x)
        {
            if (x < -45.0)
            {
                return 0.0;
            }
            else if (x > 45.0)
            {
                return 1.0;
            }
            else
            {
                return 1.0 / (1.0 + Math.Exp(-x));
            }
        }
        /// <summary>
        /// Use it when the output values >2
        /// </summary>
        public static double[] Softmax(double[] oSums)
        {
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
            {
                if (oSums[i] > max)
                {
                    max = oSums[i];
                }
            }
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
            {
                scale += Math.Exp(oSums[i] - max);
            }
            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
            {
                result[i] = Math.Exp(oSums[i] - max) / scale;
            }
            return result; 
        }
    }
}
