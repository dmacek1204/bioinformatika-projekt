using System;

public class BioAlgorithms
{
    public int MinDistance(string word1, string word2)
    {
        var table = new int[word1.Length + 1, word2.Length + 1];
        for (int i = 0; i < word1.Length + 1; ++i)
        {
            for (int j = 0; j < word2.Length + 1; ++j)
            {
                if (i == 0)
                    table[i, j] = j;
                else if (j == 0)
                    table[i, j] = i;
                else
                {
                    if (word1[i - 1] == word2[j - 1])
                        table[i, j] = table[i - 1, j - 1];
                    else
                        table[i, j] = 1 + Math.Min(Math.Min(table[i - 1, j - 1],
                            table[i - 1, j]), table[i, j - 1]);
                }
            }
        }
        return table[word1.Length, word2.Length];
    }

    public int LevensteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        // Step 1
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Step 3
        for (int i = 1; i <= n; i++)
        {
            //Step 4
            for (int j = 1; j <= m; j++)
            {
                // Step 5
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Step 6
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }
        // Step 7
        return d[n, m];
    }

}