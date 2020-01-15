using System;

namespace Corvo.Consola
{
    /// <summary>
    /// Console project used for messing around
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            var s1 = "Mirza";
            var s2 = "Mirz";
            var s3 = "Mir";
            var s4 = "ir";
            var s5 = "a";

            Console.WriteLine($"'{s2}' => '{s1}' requires {NextLevelShit.LevenshteinDistance(s2, s1)} chars edits.");
            //Console.WriteLine($"'{s3}' => '{s1}' requires {NextLevelShit.LevenshteinDistance(s3, s1)} chars edits.");
            //Console.WriteLine($"'{s4}' => '{s1}' requires {NextLevelShit.LevenshteinDistance(s4, s1)} chars edits.");
            //Console.WriteLine($"'{s5}' => '{s1}' requires {NextLevelShit.LevenshteinDistance(s5, s1)} chars edits.");
        }
    }

    internal class NextLevelShit
    {
        public static int LevenshteinDistance(string s, string t)
        {
            var n = s.Length;
            var m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // if value of s is empty, we need m characters to get s out of t
            if (n == 0)
            {
                return m;
            }

            // Vice versa
            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            Console.WriteLine("Init matrix");
            for (int i = 0; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    Console.Write(string.Format("{0} ", d[i, j]));
                }
                Console.Write(Environment.NewLine);
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            Console.WriteLine("Matrix after");
            for (int i = 0; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    Console.Write(string.Format("{0} ", d[i, j]));
                }
                Console.Write(Environment.NewLine);
            }

            return d[n, m];
        }
    }
}