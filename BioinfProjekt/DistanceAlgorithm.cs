using System;

public class DistanceAlgorithm
{
    public int MinDistance(string word1, string word2)
    {
        var table = new int[word1.Length + 1,word2.Length + 1];
        for (int i = 0; i < word1.Length + 1; ++i)
        {
            for (int j = 0; j < word2.Length + 1; ++j)
            {
                if (i == 0)
                    table[i,j] = j;
                else if (j == 0)
                    table[i,j] = i;
                else
                {
                    if (word1[i - 1] == word2[j - 1])
                        table[i,j] = table[i - 1,j - 1];
                    else
                        table[i,j] = 1 + Math.Min(Math.Min(table[i - 1,j - 1],
                            table[i - 1,j]), table[i,j - 1]);
                }
            }
        }
        return table[word1.Length,word2.Length];
    }
}
