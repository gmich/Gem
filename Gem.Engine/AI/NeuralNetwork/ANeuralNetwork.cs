using System;

namespace Gem.AI.NeuralNetwork
{
    public class ANeuralNetwork
    {

        #region Fields

        private static Random rnd;
        private readonly int inputNodesCount;
        private readonly int hidenNodesCount;
        private readonly int outputNodesCount;

        private readonly double[] inputs;

        private readonly double[][] ihWeights; // input-hidden 
        private readonly double[][] hoWeights; // hidden-output 
        private readonly double[] hBiases;
        private readonly double[] hOutputs;

        private readonly double[] oBiases;
        private readonly double[] outputs; // Back-propagation specific arrays.
        private readonly double[] oGrads; // Output gradients. 
        private readonly double[] hGrads; // Back-propagation momentum-specific arrays.

        private readonly double[][] ihPrevWeightsDelta;
        private readonly double[] hPrevBiasesDelta;
        private readonly double[][] hoPrevWeightsDelta;

        private readonly double[] oPrevBiasesDelta;

        private readonly Func<double, double> hiddenActivation;
        private readonly Func<double[], double[]> outputActivation;

        public int TotalNeurons =>
            ihWeights.GetLength(0) //* ihWeights.GetLength(1)
            + hoWeights.GetLength(0); //* hoWeights.GetLength(1);
        
        #endregion

        #region Ctor

        public ANeuralNetwork(int inputNodes,
                             int hiddenNodes,
                             int outputNodes,
                             Func<double, double> hiddenNodeActivation,
                             Func<double[], double[]> outputNodeActivation)
        {
            rnd = new Random(0);
            this.inputNodesCount = inputNodes;
            this.hidenNodesCount = hiddenNodes;
            this.outputNodesCount = outputNodes;
            this.hiddenActivation = hiddenNodeActivation;
            this.outputActivation = outputNodeActivation;
            inputs = new double[inputNodes];
            ihWeights = MakeMatrix(inputNodes, hiddenNodes);
            hBiases = new double[hiddenNodes];
            hOutputs = new double[hiddenNodes];
            hoWeights = MakeMatrix(hiddenNodes, outputNodes);
            oBiases = new double[outputNodes];
            outputs = new double[outputNodes];
            InitializeWeights();
            // Back-propagation related arrays below. 
            hGrads = new double[hiddenNodes];
            oGrads = new double[outputNodes];
            ihPrevWeightsDelta = MakeMatrix(inputNodes, hiddenNodes);
            hPrevBiasesDelta = new double[hiddenNodes];
            hoPrevWeightsDelta = MakeMatrix(hiddenNodes, outputNodes);
            oPrevBiasesDelta = new double[outputNodes];
        }

        /// <summary>
        /// Helper method to create a matrix for the hidden output weights according to the number of hidden nodes and output nodes
        /// </summary>
        private static double[][] MakeMatrix(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int r = 0; r < result.Length; ++r)
                result[r] = new double[cols];
            return result;
        }

        #endregion

        #region Weights

        /// <summary>
        /// Copy weights and biases in weights[] array to i-h weights, i-h biases, h-o weights, h-o biases
        /// </summary>
        public void SetWeights(double[] weights)
        {

            int numWeights = (inputNodesCount * hidenNodesCount) + (hidenNodesCount * outputNodesCount) + hidenNodesCount + outputNodesCount;
            if (weights.Length != numWeights)
            {
                throw new Exception("Bad weights array length: ");
            }
            int k = 0;
            for (int i = 0; i < inputNodesCount; ++i)
            {
                for (int j = 0; j < hidenNodesCount; ++j)
                {
                    ihWeights[i][j] = weights[k++];
                }
            }
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                hBiases[i] = weights[k++];
            }
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                for (int j = 0; j < outputNodesCount; ++j)
                {
                    hoWeights[i][j] = weights[k++];
                }
            }
            for (int i = 0; i < outputNodesCount; ++i)
            {
                oBiases[i] = weights[k++];
            }
        }

        /// <summary>
        /// Initialize weights and biases to small random values prior training
        /// </summary>
        private void InitializeWeights()
        {
            int numWeights = (inputNodesCount * hidenNodesCount) + (hidenNodesCount * outputNodesCount) + hidenNodesCount + outputNodesCount;
            double[] initialWeights = new double[numWeights];
            double lo = -0.01;
            double hi = 0.01;
            for (int i = 0; i < initialWeights.Length; ++i)
            {
                initialWeights[i] = (hi - lo) * rnd.NextDouble() + lo;
            }
            SetWeights(initialWeights);
        }

        /// <summary>
        /// Returns an array of the i-h weights, i-h biases, h-o weights, h-o biases
        /// </summary>
        public double[] GetWeights()
        {
            int numWeights = (inputNodesCount * hidenNodesCount) + (hidenNodesCount * outputNodesCount) + hidenNodesCount + outputNodesCount;
            double[] result = new double[numWeights];
            int k = 0;
            for (int i = 0; i < ihWeights.Length; ++i)
            {
                for (int j = 0; j < ihWeights[0].Length; ++j)
                {
                    result[k++] = ihWeights[i][j];
                }
            }
            for (int i = 0; i < hBiases.Length; ++i)
            {
                result[k++] = hBiases[i];
            }
            for (int i = 0; i < hoWeights.Length; ++i)
            {
                for (int j = 0; j < hoWeights[0].Length; ++j)
                {
                    result[k++] = hoWeights[i][j];
                }
            }
            for (int i = 0; i < oBiases.Length; ++i)
            {
                result[k++] = oBiases[i];
            }
            return result;
        }

        #endregion

        #region Feed-Forward

        /// <summary>
        /// Applies the weights an biases to the data and returns the prediction
        /// </summary>
        private double[] ComputeOutputs(double[] xValues)
        {
            if (xValues.Length != inputNodesCount)
            {
                throw new Exception("Bad xValues array length");
            }
            double[] hSums = new double[hidenNodesCount];
            double[] oSums = new double[outputNodesCount];

            for (int i = 0; i < xValues.Length; ++i)
            {
                // Copy x-values to inputs
                inputs[i] = xValues[i];
            }
            for (int j = 0; j < hidenNodesCount; ++j)
            // Compute i-h sum of weights * inputs
            {
                for (int i = 0; i < inputNodesCount; ++i)
                {
                    hSums[j] += this.inputs[i] * ihWeights[i][j];
                }
            }
            // Add biases to input-to-hidden sums
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                hSums[i] += hBiases[i];
            }
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                // Apply activation
                hOutputs[i] = hiddenActivation(hSums[i]);
            }
            for (int j = 0; j < outputNodesCount; ++j)
            {
                // Compute h-o sum of weights * hOutputs
                for (int i = 0; i < hidenNodesCount; ++i)
                {
                    oSums[j] += hOutputs[i] * hoWeights[i][j];
                }
            }
            // Add biases to input-to-hidden sums.
            for (int i = 0; i < outputNodesCount; ++i)
            {
                oSums[i] += oBiases[i];
            }

            double[] softOut = outputActivation(oSums);
            Array.Copy(softOut, outputs, softOut.Length);
            double[] retResult = new double[outputNodesCount];
            Array.Copy(outputs, retResult, retResult.Length);

            return retResult;
        }

        public double[] Predict(double[] predictionData)
        {
            ComputeOutputs(predictionData);
            double[] result = new double[outputNodesCount];
            for (int i = 0; i < outputNodesCount; ++i)
                result[i] = outputs[i];
            return result;
        }

        #endregion

        #region Back-Propagation

        /// <summary>
        // Update the weights and biases using back-propagation
        // Assumes that SetWeights and ComputeOutputs have been called and matrices have values (other than 0.0)
        /// </summary>
        private void UpdateWeights(double[] tValues, double learnRate, double momentum)
        {
            if (tValues.Length != outputNodesCount)
            {
                throw new Exception("target values not same Length as output in UpdateWeights");
            }
            // 1. Compute output gradients.
            for (int i = 0; i < outputNodesCount; ++i)
            {
                // Derivative for softmax = (1 - y) * y (same as log-sigmoid)
                double derivative = (1 - outputs[i]) * outputs[i];
                // 'Mean squared error version' includes (1-y)(y) derivative
                oGrads[i] = derivative * (tValues[i] - outputs[i]);
            }
            // 2. Compute hidden gradients
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                // Derivative of tanh = (1 - y) * (1 + y).
                double derivative = (1 - hOutputs[i]) * (1 + hOutputs[i]);
                double sum = 0.0;
                for (int j = 0; j < outputNodesCount; ++j)
                {
                    // Each hidden delta is the sum of numOutput terms. 
                    double x = oGrads[j] * hoWeights[i][j];
                    sum += x;
                }
                hGrads[i] = derivative * sum;
            }
            // 3a. Update hidden weights (gradients must be computed right-to-left but weights can be updated in any order)
            for (int i = 0; i < inputNodesCount; ++i) // 0..2 (3)
            {
                for (int j = 0; j < hidenNodesCount; ++j) // 0..3 (4)
                {
                    double delta = learnRate * hGrads[j] * inputs[i];
                    // Compute the new delta. 
                    ihWeights[i][j] += delta;
                    // Update -- note '+' instead of '-'. 
                    // Now add momentum using previous delta
                    ihWeights[i][j] += momentum * ihPrevWeightsDelta[i][j];
                    ihPrevWeightsDelta[i][j] = delta;
                    // Don't forget to save the delta for momentum 
                }
            }
            // 3b. Update hidden biases. 
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                double delta = learnRate * hGrads[i];
                // 1.0 is constant input for bias
                hBiases[i] += delta; hBiases[i] += momentum * hPrevBiasesDelta[i];
                // Momentum
                // Don't forget to save the delta
                hPrevBiasesDelta[i] = delta;
            }
            // 4. Update hidden-output weights.
            for (int i = 0; i < hidenNodesCount; ++i)
            {
                for (int j = 0; j < outputNodesCount; ++j)
                {
                    double delta = learnRate * oGrads[j] * hOutputs[i];
                    hoWeights[i][j] += delta; hoWeights[i][j] += momentum * hoPrevWeightsDelta[i][j];
                    // Momentum. 
                    hoPrevWeightsDelta[i][j] = delta; // Save
                }
            } // 4b. Update output biases
            for (int i = 0; i < outputNodesCount; ++i)
            {
                double delta = learnRate * oGrads[i] * 1.0;
                oBiases[i] += delta; oBiases[i] += momentum * oPrevBiasesDelta[i];
                // Momentum
                oPrevBiasesDelta[i] = delta; // save 
            }
        }

        #endregion

        #region Training

        /// <summary>
        /// Train a back-propagation style NN classifier using learning rate and momentum.
        /// </summary>
        public void Train(double[][] trainData, int maxEpochs, double learnRate, double momentum, double meanSquaredErrorThreshold)
        {
            int epoch = 0;
            double[] xValues = new double[inputNodesCount];
            double[] tValues = new double[outputNodesCount];
            int[] sequence = new int[trainData.Length];

            for (int i = 0; i < sequence.Length; ++i)
            {
                sequence[i] = i;
            }
            while (epoch < maxEpochs)
            {
                double mse = MeanSquaredError(trainData);
                if (mse < meanSquaredErrorThreshold)
                {
                    break;
                }
                Shuffle(sequence);

                for (int i = 0; i < trainData.Length; ++i)
                {
                    int idx = sequence[i];
                    Array.Copy(trainData[idx], xValues, inputNodesCount);
                    Array.Copy(trainData[idx], inputNodesCount, tValues, 0, outputNodesCount);

                    ComputeOutputs(xValues); //stores values internally
                    UpdateWeights(tValues, learnRate, momentum);
                }
                ++epoch;
            }
        }

        /// <summary>
        /// Shuffles using the Fisher-Yates shuffle algorithm
        /// </summary>
        private static void Shuffle(int[] sequence)
        {
            for (int i = 0; i < sequence.Length; ++i)
            {
                int r = rnd.Next(i, sequence.Length);
                int tmp = sequence[r];
                sequence[r] = sequence[i]; sequence[i] = tmp;
            }
        }

        /// <summary>
        ///  Average squared error per training item
        /// </summary>
        /// <param name="trainData"></param>
        /// <returns></returns>
        private double MeanSquaredError(double[][] trainData)
        {
            double sumSquaredError = 0.0;
            double[] xValues = new double[inputNodesCount];
            // First numInput values in trainData.
            double[] tValues = new double[outputNodesCount];
            // Last numOutput values. 
            // Walk through each training case. Looks like (6.9 3.2 5.7 2.3) (0 0 1). 
            for (int i = 0; i < trainData.Length; ++i)
            {
                Array.Copy(trainData[i], xValues, inputNodesCount);
                Array.Copy(trainData[i], inputNodesCount, tValues, 0, outputNodesCount);
                // Get target values.
                double[] yValues = this.ComputeOutputs(xValues);
                // Outputs using current weights. 
                for (int j = 0; j < outputNodesCount; ++j)
                {
                    double err = tValues[j] - yValues[j];
                    sumSquaredError += err * err;
                }
            }
            return sumSquaredError / trainData.Length;
        }

        /// <summary>
        ///  Percentage correct using winner-takes all.
        /// </summary>
        public double GetAccuracyFor(double[][] testData)
        {
            int numCorrect = 0;
            int numWrong = 0;
            double[] xValues = new double[inputNodesCount];
            double[] tValues = new double[outputNodesCount];
            double[] yValues;

            for (int i = 0; i < testData.Length; ++i)
            {
                Array.Copy(testData[i], xValues, inputNodesCount);
                // Get x-values.
                Array.Copy(testData[i], inputNodesCount, tValues, 0, outputNodesCount);
                // Get t-values. 
                yValues = ComputeOutputs(xValues);
                int maxIndex = MaxIndex(yValues);
                // Which cell in yValues has the largest value? 
                if (tValues[maxIndex] == 1.0) // ugly
                {
                    ++numCorrect;
                }
                else
                {
                    ++numWrong;
                }
            }
            return (numCorrect * 1.0) / (numCorrect + numWrong);
            // No check for divide by zero. 
        }

        /// <summary>
        /// Helper for finding the highest probability and computing accuracy
        /// </summary>
        private static int MaxIndex(double[] vector)
        {
            int bigIndex = 0;
            double biggestVal = vector[0];
            for (int i = 0; i < vector.Length; ++i)
            {
                if (vector[i] > biggestVal)
                {
                    biggestVal = vector[i];
                    bigIndex = i;
                }
            }
            return bigIndex;
        }

        #endregion

    }
}
