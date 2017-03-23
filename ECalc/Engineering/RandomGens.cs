using ECalc.Lib;
using Sublight.Utilities;
using System;
using System.Collections.Generic;
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

        public static Task<string> Passwords(int length, int count, bool lowercase, bool uppercase, bool numbers, bool special)
        {
            return Task.Run(() =>
            {
                var pool = new List<char>(90);
                if (lowercase) pool.AddRange("abcdefghijklmnopqrstuvwxyz".ToCharArray());
                if (uppercase) pool.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
                if (numbers) pool.AddRange("0123456789".ToCharArray());
                if (special) pool.AddRange("_+-/&#!?@[];:".ToCharArray());

                var randomized = new List<char>(pool.Count);
                var r = new CryptoRNG();
                while (pool.Count > 0)
                {
                    var index = r.Next(pool.Count);
                    randomized.Add(pool[index]);
                    pool.RemoveAt(index);
                }


                var buffer = new StringBuilder();

                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < length; j++)
                    {
                        var pick = r.Next(randomized.Count);
                        buffer.Append(randomized[pick]);
                    }
                    buffer.Append('\n');
                }

                return buffer.ToString();
            });
        }

        public static Task<string> Guids(int count)
        {
            return Task.Run(() =>
            {
                var buffer = new StringBuilder();
                for (int i=0; i<count; i++)
                {
                    var g = Guid.NewGuid();
                    buffer.AppendLine(g.ToString());
                }

                return buffer.ToString();
            });
        } 
    }
}
