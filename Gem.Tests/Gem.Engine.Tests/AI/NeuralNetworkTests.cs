using Gem.AI.NeuralNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;

namespace Gem.Engine.Tests.AI
{
    [TestClass]
    public class NeuralNetworkTests
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

        [TestMethod]
        public void EncodingAndNormalizationTest()
        {
            char separator = ' ';
            string[] encodedData = rawData.Skip(2).ToArray();
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 4, separator, Encode.DependentVariable);
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 2, separator, Encode.Predictor);
            encodedData = ParsingUtilities.EncodeColumnInLine(encodedData, 0, separator, Encode.Predictor);
            var numericalData = ParsingUtilities.ConvertToNumeric(encodedData, separator);
            Normalize.Multiple(numericalData, Normalize.Gauss, 1);
            Normalize.Multiple(numericalData, Normalize.MinMax, 4);
        }
    }
}
