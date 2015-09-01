using Gem.AI.NeuralNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class NeuralNetworkTests
    {

        [TestMethod]
        public void EncodingAndNormalizationTest()
        {
            string[] rawData = new[]
            {
                "Sex Age Locale Income Politics",
                "==============================================",
                "Male 25 Rural 63,000.00 Conservative",
                "Female 36 Suburban 55,000.00 Liberal",
                "Male 40 Urban 74,000.00 Moderate",
                "Female 23 Rural 28,000.00 Liberal"
            };

            char separator = ' ';
            string[] encodedData = rawData.Skip(2).ToArray();
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 4, separator, Encode.DependentVariable);
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 2, separator, Encode.Predictor);
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 0, separator, Encode.Predictor);
            var numericalData = ParsingUtilities.ConvertToNumeric(encodedData, separator);
            Normalize.Multiple(numericalData, Normalize.Gauss, 1);
            Normalize.Multiple(numericalData, Normalize.MinMax, 4);

            Assert.AreEqual("Moderate", ParsingUtilities.Decode(rawData.Skip(2).ToArray(), 4, ' ', new double[] { 0, 0, 1 }));
        }

        [TestMethod]
        public void AutomaticallyEncodedNeuralNetworkDataTest()
        {
            string[] rawData = new[]
            {
                "Sex Age Locale Income Politics",
                "==============================================",
                "Male 25 Rural 63,000.00 Conservative",
                "Female 36 Suburban 55,000.00 Liberal",
                "Male 40 Urban 74,000.00 Moderate",
                "Female 23 Rural 28,000.00 Liberal"
            };
            char separator = ' ';
            var networkData = new NeuralNetworkData(rawData.Skip(2).ToArray(), separator);
            var convertedData = networkData.ConvertedData;

            Assert.AreEqual("Moderate", networkData.DecodeOutput(0, 0, 1));
            Assert.AreEqual("Liberal", networkData.DecodeOutput(0, 1, 0));
            Assert.AreEqual("Conservative", networkData.DecodeOutput(1, 0, 0));
            Assert.AreEqual("Moderate", ParsingUtilities.Decode(rawData.Skip(2).ToArray(), 4, separator, new double[] { 0, 0, 1 }));
        }

        [TestMethod]
        public void IrisClasificationTest()
        {
            #region Training Data

            //Encoded
            // Iris setosa = 1 0 0 
            // Iris versicolor = 0 1 0
            // Iris virginica = 0 0 1

            string[] rawData = new[]
            {
                "setosa",
                "versicolor",
                "virginica"
            };

            double[][] encodedData = new double[150][];

            encodedData[0] = new double[] { 5.1, 3.5, 1.4, 0.2, 0, 0, 1 };
            encodedData[1] = new double[] { 4.9, 3.0, 1.4, 0.2, 0, 0, 1 };
            encodedData[2] = new double[] { 4.7, 3.2, 1.3, 0.2, 0, 0, 1 };
            encodedData[3] = new double[] { 4.6, 3.1, 1.5, 0.2, 0, 0, 1 };
            encodedData[4] = new double[] { 5.0, 3.6, 1.4, 0.2, 0, 0, 1 };
            encodedData[5] = new double[] { 5.4, 3.9, 1.7, 0.4, 0, 0, 1 };
            encodedData[6] = new double[] { 4.6, 3.4, 1.4, 0.3, 0, 0, 1 };
            encodedData[7] = new double[] { 5.0, 3.4, 1.5, 0.2, 0, 0, 1 };
            encodedData[8] = new double[] { 4.4, 2.9, 1.4, 0.2, 0, 0, 1 };
            encodedData[9] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            encodedData[10] = new double[] { 5.4, 3.7, 1.5, 0.2, 0, 0, 1 };
            encodedData[11] = new double[] { 4.8, 3.4, 1.6, 0.2, 0, 0, 1 };
            encodedData[12] = new double[] { 4.8, 3.0, 1.4, 0.1, 0, 0, 1 };
            encodedData[13] = new double[] { 4.3, 3.0, 1.1, 0.1, 0, 0, 1 };
            encodedData[14] = new double[] { 5.8, 4.0, 1.2, 0.2, 0, 0, 1 };
            encodedData[15] = new double[] { 5.7, 4.4, 1.5, 0.4, 0, 0, 1 };
            encodedData[16] = new double[] { 5.4, 3.9, 1.3, 0.4, 0, 0, 1 };
            encodedData[17] = new double[] { 5.1, 3.5, 1.4, 0.3, 0, 0, 1 };
            encodedData[18] = new double[] { 5.7, 3.8, 1.7, 0.3, 0, 0, 1 };
            encodedData[19] = new double[] { 5.1, 3.8, 1.5, 0.3, 0, 0, 1 };
            encodedData[20] = new double[] { 5.4, 3.4, 1.7, 0.2, 0, 0, 1 };
            encodedData[21] = new double[] { 5.1, 3.7, 1.5, 0.4, 0, 0, 1 };
            encodedData[22] = new double[] { 4.6, 3.6, 1.0, 0.2, 0, 0, 1 };
            encodedData[23] = new double[] { 5.1, 3.3, 1.7, 0.5, 0, 0, 1 };
            encodedData[24] = new double[] { 4.8, 3.4, 1.9, 0.2, 0, 0, 1 };
            encodedData[25] = new double[] { 5.0, 3.0, 1.6, 0.2, 0, 0, 1 };
            encodedData[26] = new double[] { 5.0, 3.4, 1.6, 0.4, 0, 0, 1 };
            encodedData[27] = new double[] { 5.2, 3.5, 1.5, 0.2, 0, 0, 1 };
            encodedData[28] = new double[] { 5.2, 3.4, 1.4, 0.2, 0, 0, 1 };
            encodedData[29] = new double[] { 4.7, 3.2, 1.6, 0.2, 0, 0, 1 };
            encodedData[30] = new double[] { 4.8, 3.1, 1.6, 0.2, 0, 0, 1 };
            encodedData[31] = new double[] { 5.4, 3.4, 1.5, 0.4, 0, 0, 1 };
            encodedData[32] = new double[] { 5.2, 4.1, 1.5, 0.1, 0, 0, 1 };
            encodedData[33] = new double[] { 5.5, 4.2, 1.4, 0.2, 0, 0, 1 };
            encodedData[34] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            encodedData[35] = new double[] { 5.0, 3.2, 1.2, 0.2, 0, 0, 1 };
            encodedData[36] = new double[] { 5.5, 3.5, 1.3, 0.2, 0, 0, 1 };
            encodedData[37] = new double[] { 4.9, 3.1, 1.5, 0.1, 0, 0, 1 };
            encodedData[38] = new double[] { 4.4, 3.0, 1.3, 0.2, 0, 0, 1 };
            encodedData[39] = new double[] { 5.1, 3.4, 1.5, 0.2, 0, 0, 1 };
            encodedData[40] = new double[] { 5.0, 3.5, 1.3, 0.3, 0, 0, 1 };
            encodedData[41] = new double[] { 4.5, 2.3, 1.3, 0.3, 0, 0, 1 };
            encodedData[42] = new double[] { 4.4, 3.2, 1.3, 0.2, 0, 0, 1 };
            encodedData[43] = new double[] { 5.0, 3.5, 1.6, 0.6, 0, 0, 1 };
            encodedData[44] = new double[] { 5.1, 3.8, 1.9, 0.4, 0, 0, 1 };
            encodedData[45] = new double[] { 4.8, 3.0, 1.4, 0.3, 0, 0, 1 };
            encodedData[46] = new double[] { 5.1, 3.8, 1.6, 0.2, 0, 0, 1 };
            encodedData[47] = new double[] { 4.6, 3.2, 1.4, 0.2, 0, 0, 1 };
            encodedData[48] = new double[] { 5.3, 3.7, 1.5, 0.2, 0, 0, 1 };
            encodedData[49] = new double[] { 5.0, 3.3, 1.4, 0.2, 0, 0, 1 };
            encodedData[50] = new double[] { 7.0, 3.2, 4.7, 1.4, 0, 1, 0 };
            encodedData[51] = new double[] { 6.4, 3.2, 4.5, 1.5, 0, 1, 0 };
            encodedData[52] = new double[] { 6.9, 3.1, 4.9, 1.5, 0, 1, 0 };
            encodedData[53] = new double[] { 5.5, 2.3, 4.0, 1.3, 0, 1, 0 };
            encodedData[54] = new double[] { 6.5, 2.8, 4.6, 1.5, 0, 1, 0 };
            encodedData[55] = new double[] { 5.7, 2.8, 4.5, 1.3, 0, 1, 0 };
            encodedData[56] = new double[] { 6.3, 3.3, 4.7, 1.6, 0, 1, 0 };
            encodedData[57] = new double[] { 4.9, 2.4, 3.3, 1.0, 0, 1, 0 };
            encodedData[58] = new double[] { 6.6, 2.9, 4.6, 1.3, 0, 1, 0 };
            encodedData[59] = new double[] { 5.2, 2.7, 3.9, 1.4, 0, 1, 0 };
            encodedData[60] = new double[] { 5.0, 2.0, 3.5, 1.0, 0, 1, 0 };
            encodedData[61] = new double[] { 5.9, 3.0, 4.2, 1.5, 0, 1, 0 };
            encodedData[62] = new double[] { 6.0, 2.2, 4.0, 1.0, 0, 1, 0 };
            encodedData[63] = new double[] { 6.1, 2.9, 4.7, 1.4, 0, 1, 0 };
            encodedData[64] = new double[] { 5.6, 2.9, 3.6, 1.3, 0, 1, 0 };
            encodedData[65] = new double[] { 6.7, 3.1, 4.4, 1.4, 0, 1, 0 };
            encodedData[66] = new double[] { 5.6, 3.0, 4.5, 1.5, 0, 1, 0 };
            encodedData[67] = new double[] { 5.8, 2.7, 4.1, 1.0, 0, 1, 0 };
            encodedData[68] = new double[] { 6.2, 2.2, 4.5, 1.5, 0, 1, 0 };
            encodedData[69] = new double[] { 5.6, 2.5, 3.9, 1.1, 0, 1, 0 };
            encodedData[70] = new double[] { 5.9, 3.2, 4.8, 1.8, 0, 1, 0 };
            encodedData[71] = new double[] { 6.1, 2.8, 4.0, 1.3, 0, 1, 0 };
            encodedData[72] = new double[] { 6.3, 2.5, 4.9, 1.5, 0, 1, 0 };
            encodedData[73] = new double[] { 6.1, 2.8, 4.7, 1.2, 0, 1, 0 };
            encodedData[74] = new double[] { 6.4, 2.9, 4.3, 1.3, 0, 1, 0 };
            encodedData[75] = new double[] { 6.6, 3.0, 4.4, 1.4, 0, 1, 0 };
            encodedData[76] = new double[] { 6.8, 2.8, 4.8, 1.4, 0, 1, 0 };
            encodedData[77] = new double[] { 6.7, 3.0, 5.0, 1.7, 0, 1, 0 };
            encodedData[78] = new double[] { 6.0, 2.9, 4.5, 1.5, 0, 1, 0 };
            encodedData[79] = new double[] { 5.7, 2.6, 3.5, 1.0, 0, 1, 0 };
            encodedData[80] = new double[] { 5.5, 2.4, 3.8, 1.1, 0, 1, 0 };
            encodedData[81] = new double[] { 5.5, 2.4, 3.7, 1.0, 0, 1, 0 };
            encodedData[82] = new double[] { 5.8, 2.7, 3.9, 1.2, 0, 1, 0 };
            encodedData[83] = new double[] { 6.0, 2.7, 5.1, 1.6, 0, 1, 0 };
            encodedData[84] = new double[] { 5.4, 3.0, 4.5, 1.5, 0, 1, 0 };
            encodedData[85] = new double[] { 6.0, 3.4, 4.5, 1.6, 0, 1, 0 };
            encodedData[86] = new double[] { 6.7, 3.1, 4.7, 1.5, 0, 1, 0 };
            encodedData[87] = new double[] { 6.3, 2.3, 4.4, 1.3, 0, 1, 0 };
            encodedData[88] = new double[] { 5.6, 3.0, 4.1, 1.3, 0, 1, 0 };
            encodedData[89] = new double[] { 5.5, 2.5, 4.0, 1.3, 0, 1, 0 };
            encodedData[90] = new double[] { 5.5, 2.6, 4.4, 1.2, 0, 1, 0 };
            encodedData[91] = new double[] { 6.1, 3.0, 4.6, 1.4, 0, 1, 0 };
            encodedData[92] = new double[] { 5.8, 2.6, 4.0, 1.2, 0, 1, 0 };
            encodedData[93] = new double[] { 5.0, 2.3, 3.3, 1.0, 0, 1, 0 };
            encodedData[94] = new double[] { 5.6, 2.7, 4.2, 1.3, 0, 1, 0 };
            encodedData[95] = new double[] { 5.7, 3.0, 4.2, 1.2, 0, 1, 0 };
            encodedData[96] = new double[] { 5.7, 2.9, 4.2, 1.3, 0, 1, 0 };
            encodedData[97] = new double[] { 6.2, 2.9, 4.3, 1.3, 0, 1, 0 };
            encodedData[98] = new double[] { 5.1, 2.5, 3.0, 1.1, 0, 1, 0 };
            encodedData[99] = new double[] { 5.7, 2.8, 4.1, 1.3, 0, 1, 0 };
            encodedData[100] = new double[] { 6.3, 3.3, 6.0, 2.5, 1, 0, 0 };
            encodedData[101] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            encodedData[102] = new double[] { 7.1, 3.0, 5.9, 2.1, 1, 0, 0 };
            encodedData[103] = new double[] { 6.3, 2.9, 5.6, 1.8, 1, 0, 0 };
            encodedData[104] = new double[] { 6.5, 3.0, 5.8, 2.2, 1, 0, 0 };
            encodedData[105] = new double[] { 7.6, 3.0, 6.6, 2.1, 1, 0, 0 };
            encodedData[106] = new double[] { 4.9, 2.5, 4.5, 1.7, 1, 0, 0 };
            encodedData[107] = new double[] { 7.3, 2.9, 6.3, 1.8, 1, 0, 0 };
            encodedData[108] = new double[] { 6.7, 2.5, 5.8, 1.8, 1, 0, 0 };
            encodedData[109] = new double[] { 7.2, 3.6, 6.1, 2.5, 1, 0, 0 };
            encodedData[110] = new double[] { 6.5, 3.2, 5.1, 2.0, 1, 0, 0 };
            encodedData[111] = new double[] { 6.4, 2.7, 5.3, 1.9, 1, 0, 0 };
            encodedData[112] = new double[] { 6.8, 3.0, 5.5, 2.1, 1, 0, 0 };
            encodedData[113] = new double[] { 5.7, 2.5, 5.0, 2.0, 1, 0, 0 };
            encodedData[114] = new double[] { 5.8, 2.8, 5.1, 2.4, 1, 0, 0 };
            encodedData[115] = new double[] { 6.4, 3.2, 5.3, 2.3, 1, 0, 0 };
            encodedData[116] = new double[] { 6.5, 3.0, 5.5, 1.8, 1, 0, 0 };
            encodedData[117] = new double[] { 7.7, 3.8, 6.7, 2.2, 1, 0, 0 };
            encodedData[118] = new double[] { 7.7, 2.6, 6.9, 2.3, 1, 0, 0 };
            encodedData[119] = new double[] { 6.0, 2.2, 5.0, 1.5, 1, 0, 0 };
            encodedData[120] = new double[] { 6.9, 3.2, 5.7, 2.3, 1, 0, 0 };
            encodedData[121] = new double[] { 5.6, 2.8, 4.9, 2.0, 1, 0, 0 };
            encodedData[122] = new double[] { 7.7, 2.8, 6.7, 2.0, 1, 0, 0 };
            encodedData[123] = new double[] { 6.3, 2.7, 4.9, 1.8, 1, 0, 0 };
            encodedData[124] = new double[] { 6.7, 3.3, 5.7, 2.1, 1, 0, 0 };
            encodedData[125] = new double[] { 7.2, 3.2, 6.0, 1.8, 1, 0, 0 };
            encodedData[126] = new double[] { 6.2, 2.8, 4.8, 1.8, 1, 0, 0 };
            encodedData[127] = new double[] { 6.1, 3.0, 4.9, 1.8, 1, 0, 0 };
            encodedData[128] = new double[] { 6.4, 2.8, 5.6, 2.1, 1, 0, 0 };
            encodedData[129] = new double[] { 7.2, 3.0, 5.8, 1.6, 1, 0, 0 };
            encodedData[130] = new double[] { 7.4, 2.8, 6.1, 1.9, 1, 0, 0 };
            encodedData[131] = new double[] { 7.9, 3.8, 6.4, 2.0, 1, 0, 0 };
            encodedData[132] = new double[] { 6.4, 2.8, 5.6, 2.2, 1, 0, 0 };
            encodedData[133] = new double[] { 6.3, 2.8, 5.1, 1.5, 1, 0, 0 };
            encodedData[134] = new double[] { 6.1, 2.6, 5.6, 1.4, 1, 0, 0 };
            encodedData[135] = new double[] { 7.7, 3.0, 6.1, 2.3, 1, 0, 0 };
            encodedData[136] = new double[] { 6.3, 3.4, 5.6, 2.4, 1, 0, 0 };
            encodedData[137] = new double[] { 6.4, 3.1, 5.5, 1.8, 1, 0, 0 };
            encodedData[138] = new double[] { 6.0, 3.0, 4.8, 1.8, 1, 0, 0 };
            encodedData[139] = new double[] { 6.9, 3.1, 5.4, 2.1, 1, 0, 0 };
            encodedData[140] = new double[] { 6.7, 3.1, 5.6, 2.4, 1, 0, 0 };
            encodedData[141] = new double[] { 6.9, 3.1, 5.1, 2.3, 1, 0, 0 };
            encodedData[142] = new double[] { 5.8, 2.7, 5.1, 1.9, 1, 0, 0 };
            encodedData[143] = new double[] { 6.8, 3.2, 5.9, 2.3, 1, 0, 0 };
            encodedData[144] = new double[] { 6.7, 3.3, 5.7, 2.5, 1, 0, 0 };
            encodedData[145] = new double[] { 6.7, 3.0, 5.2, 2.3, 1, 0, 0 };
            encodedData[146] = new double[] { 6.3, 2.5, 5.0, 1.9, 1, 0, 0 };
            encodedData[147] = new double[] { 6.5, 3.0, 5.2, 2.0, 1, 0, 0 };
            encodedData[148] = new double[] { 6.2, 3.4, 5.4, 2.3, 1, 0, 0 };
            encodedData[149] = new double[] { 5.9, 3.0, 5.1, 1.8, 1, 0, 0 };

            #endregion

            var networkData = new NeuralNetworkData(rawData, ' ');

            networkData.PrepareTrainingSet(
                encodedData: encodedData,
                trainDataPercentage: 0.80d,
                seed: 72);

            NeuralNetwork nn = new NeuralNetwork(
                inputNodes: 4,
                hiddenNodes: 7,
                outputNodes: 3,
                hiddenNodeActivation: Activation.HyperTan,
                outputNodeActivation: Activation.Softmax);

            nn.Train(
                networkData.TrainingData,
                maxEpochs: 1000,
                learnRate: 0.05d,
                momentum: 0.01d,
                meanSquaredErrorThreshold: 0.040d);

            Assert.IsTrue(nn.GetAccuracyFor(networkData.TrainingData) > 0.95d);
            Assert.IsTrue(nn.GetAccuracyFor(networkData.TestData) > 0.95d);
            Assert.AreEqual("virginica", networkData.DecodeOutput(nn.Predict(new double[] { 5.1, 3.5, 1.4, 0.2 })));
            Assert.AreEqual("versicolor", networkData.DecodeOutput(nn.Predict(new double[] { 7.0, 3.2, 4.7, 1.4 })));
            Assert.AreEqual("setosa", networkData.DecodeOutput(nn.Predict(new double[] { 7.4, 2.8, 6.1, 1.9 })));
        }

    }
}
