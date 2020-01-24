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
            var data = new NextLevelShit
            {
                FirstName = "Next",
                LastName = "Level"
            };

            var len = data.FirstName.Length + 1 + data.LastName.Length + 1;

            var optimizedString = string.Create(len, data, (chars, state) =>
            {
                var position = 0;
                state.FirstName.AsSpan().CopyTo(chars);
                position += state.FirstName.Length;
                state.LastName.AsSpan().CopyTo(chars.Slice(position));
                position += state.LastName.Length;
            });

            Console.WriteLine(optimizedString);
            Console.WriteLine("Au revoir");
        }
    }

    internal class NextLevelShit
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

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

            for (int i = 0; i <= n; i++)
            {
                d[i, 0] = i;
            }

            for (int j = 0; j <= m; j++)
            {
                d[0, j] = j;
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}