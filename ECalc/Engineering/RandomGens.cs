using ECalc.Lib;
using Sublight.Utilities;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ECalc.Engineering
{
    internal static class RandomGens
    {
        public static Task<string> DefaultRandom(int count, int minimum, int maximum)
        {
            return Task.Run(() =>
            {
                var buffer = new StringBuilder();
                var r = new Random();
                for (int i=0; i<count; i++)
                {
                    buffer.Append(r.Next(minimum, maximum));
                    buffer.Append("\r\n");
                }
                return buffer.ToString();
            });
        }

        public static Task<string> CryptoRandom(int count, int minimum, int maximum)
        {
            return Task.Run(() =>
            {
                var buffer = new StringBuilder();
                var r = new CryptoRNG();
                for (int i = 0; i < count; i++)
                {
                    buffer.Append(r.Next(minimum, maximum));
                    buffer.Append("\r\n");
                }
                return buffer.ToString();
            });
        }

        public static Task<string> QuantumRandom(int count, int minimum, int maximum)
        {
            return Task.Run(() =>
            {
                var buffer = new StringBuilder();
                var r = new QuantumRandomNumberGenerator();
                for (int i = 0; i < count; i++)
                {
                    buffer.Append(r.Next(minimum, maximum));
                    buffer.Append("\r\n");
                }
                return buffer.ToString();
            });
        }

        public static Task<string> LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numParagraphs)
        {
            return Task.Run(() =>
            {
                var words = new[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed",
                                    "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna",
                                    "aliquam", "erat" };

                var rand = new Random();
                int numSentences = rand.Next(maxSentences - minSentences) + minSentences + 1;
                int numWords = rand.Next(maxWords - minWords) + minWords + 1;

                var result = new StringBuilder();

                for (int p = 0; p < numParagraphs; p++)
                {
                    for (int s = 0; s < numSentences; s++)
                    {
                        for (int w = 0; w < numWords; w++)
                        {
                            if (w > 0) { result.Append(" "); }
                            result.Append(words[rand.Next(words.Length)]);
                        }
                        result.Append(". ");
                    }
                    result.Append("\r\n\r\n");
                }

                return result.ToString();
            });
        }


    }
}
