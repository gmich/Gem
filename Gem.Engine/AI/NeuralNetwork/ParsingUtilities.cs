using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Gem.AI.NeuralNetwork
{
    public sealed class ParsingUtilities
    {

        #region File Related

        public static Dictionary<string, int> GetTokensByDelegate(Action<Action<string>> lineProvider, int column, char separator)
        {
            Dictionary<string, int> tokensLookup = new Dictionary<string, int>();
            string[] tokens = null;

            lineProvider(line =>
            {
                int itemNum = 0;
                tokens = line.Split(separator);
                if (tokensLookup.ContainsKey(tokens[column]) == false)
                {
                    tokensLookup.Add(tokens[column], itemNum++);
                }
            });
            return tokensLookup;
        }

        public static string EncodeColumn(Action<Action<string>> lineProvider,
                                          Dictionary<string, int> tokenLookup,
                                          Func<int, int, string> encodingType,
                                          int column,
                                          char separator)
        {
            string stream = null;
            string[] tokens = null;
            int N = tokenLookup.Count;

            lineProvider(line =>
            {
                stream = "";
                tokens = line.Split(separator);
                for (int i = 0; i < tokens.Length; ++i)
                {
                    if (i == column)
                    {
                        stream += encodingType(tokenLookup[tokens[i]], N) + separator;
                    }
                    else
                    {
                        stream += tokens[i] + separator;
                    }
                    //stream += Environment.NewLine;
                }
            });
            stream.Remove(stream.Length - 1);
            return stream;
        }

        private static Dictionary<string, int> GetTokensFromFile(string fileName, int column, char separator)
        {
            return GetTokensByDelegate(str => FileLineProvider(fileName, str), column, separator);
        }

        public static void FileLineProvider(string file, Action<string> lineProvider)
        {
            ReadFrom(file, sr =>
            {
                string line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    lineProvider(line);
                }
            });
        }

        public static void ReadFrom(string file, Action<StreamReader> reader)
        {
            using (var ifs = new FileStream(file, FileMode.Open))
            {
                using (var sr = new StreamReader(ifs))
                {
                    reader(sr);
                }
            }
        }

        private static string[] GetLinesFrom(string file)
        {
            return File.ReadAllLines(file);
        }

        public static void EncodeFile(string dataFile, string encodedFile, int column, char separator, Func<int, int, char, string> encodingType)
        {
            var tokensResult = GetTokensFromFile(dataFile, column, separator);

            var tokenLookup = tokensResult;
            int N = tokenLookup.Count; // Number of distinct strings.
            string[] tokens = null;

            ReadFrom(dataFile, sr =>
            {
                using (var ofs = new FileStream(encodedFile, FileMode.Create))
                {
                    using (var sw = new StreamWriter(ofs))
                    {
                        string stream = null;
                        string line = null;
                        while ((line = sr.ReadLine()) != null)
                        {
                            stream = "";
                            tokens = line.Split(separator);
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
                            stream.Remove(stream.Length - 1);
                            sw.WriteLine(stream);
                        }
                    }
                }
            });
        }


        #endregion

        public class AnalyzationResult
        {
            public AnalyzationResult(int[] encodingColumns, int[] normalizationColumns)
            {
                EncodingColumns = encodingColumns;
                NormalizationColumns = normalizationColumns;
            }
            public int[] EncodingColumns { get; }
            public int[] NormalizationColumns { get; }
        }

        public static AnalyzationResult AnalyzeColumns(string[] data, char separator)
        {
            List<int> columnsForEncoding = new List<int>();
            List<int> columnsForNormalization = new List<int>();

            foreach (var line in data)
            {
                var tokens = line.Split(separator);
                for (int i = 0; i < tokens.Length; i++)
                {
                    double conversionResult;
                    if (Double.TryParse(tokens[i], out conversionResult))
                    {
                        columnsForNormalization.Add(i);
                    }
                    else
                    {
                        columnsForEncoding.Add(i);
                    }
                }
            }
            return new AnalyzationResult(columnsForEncoding.ToArray(), columnsForNormalization.ToArray());
        }


        public static string[] EncodeColumnInLine(string[] lines, int column, char separator, Func<int, int, char, string> encodingType)
        {
            var tokenLookup = GetTokens(lines, column, separator);
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
                //stream += Environment.NewLine;
                resultStream.Add(stream);
            }
            return resultStream.ToArray();
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

        public static double[] Flatten(double[] data)
        {
            double[] flattenData = new double[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                flattenData[i] = (data[i] > 0.5) ? 1 : 0;
            }
            return flattenData;
        }
        

        public static string Decode(string[] lines, int column, char separator, double[] encodedData)
        {
            var tokenLookup = GetTokens(lines, column, separator);

            for (int i = 0; i < encodedData.Count(); i++)
            {
                if (encodedData[i] == 1)
                {
                    return tokenLookup.Where(x => x.Value == i).Select(x => x.Key).First();
                }
            }
            throw new Exception("Unable to decode data");
        }

        public static double[][] ConvertToNumeric(string[] lines, char separator)
        {
            var numericData = new List<double[]>();

            foreach (var line in lines)
            {
                var tokens = line.Split(separator).Where(str => str != Environment.NewLine);
                try
                {
                    numericData.Add(tokens.Select(token => double.Parse(token, CultureInfo.InvariantCulture)).ToArray());
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to convert raw data to numeric values " + ex.Message);
                }
            }
            return numericData.ToArray();
        }

    }
}

