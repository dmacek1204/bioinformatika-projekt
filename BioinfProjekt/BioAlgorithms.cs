using System;

public class BioAlgorithms
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

    public int NeedlemanWunsch(string word1, string word2)
    {
        int word1Cnt = word1.Length + 1;
        int word2Cnt = word2.Length + 1;

        int[,] scoringMatrix = new int[word2Cnt, word1Cnt];

        //Initailization Step - filled with 0 for the first row and the first column of matrix
        for (int i = 0; i < word2Cnt; i++)
        {
            for (int j = 0; j < word1Cnt; j++)
            {
                scoringMatrix[i, j] = 0;
            }
            // scoringMatrix[i, 0] = 0; 
        }

        //Matrix Fill Step
        for (int i = 1; i < word2Cnt; i++)
        {
            for (int j = 1; j < word1Cnt; j++)
            {
                int scroeDiag = 0;
                if (word1.Substring(j - 1, 1) == word2.Substring(i - 1, 1))
                    scroeDiag = scoringMatrix[i - 1, j - 1] + 0;
                else
                    scroeDiag = scoringMatrix[i - 1, j - 1] + -1;

                int scroeLeft = scoringMatrix[i, j - 1] - 1;
                int scroeUp = scoringMatrix[i - 1, j] - 1;

                int maxScore = Math.Max(Math.Max(scroeDiag, scroeLeft), scroeUp);

                scoringMatrix[i, j] = maxScore;
            }
        }

        var similarity = 0;

        for (int i = 1; i < word2Cnt; i++)
        {
            if(similarity < scoringMatrix[i, word1Cnt - 1])
            {
                similarity = scoringMatrix[i, word1Cnt - 1];
            }
        }

        return similarity;
    }

    public string NeedlemanWunschAllignFirst(string first, string second)
    {
        int firstCnt = first.Length + 1;
        int secondCnt = second.Length + 1;

        int[,] scoringMatrix = new int[secondCnt, firstCnt];

        //Initailization Step - filled with 0 for the first row and the first column of matrix
        for (int i = 0; i < secondCnt; i++)
        {
            for (int j = 0; j < firstCnt; j++)
            {
                scoringMatrix[i, j] = 0;
            }
            // scoringMatrix[i, 0] = 0; 
        }

        //Matrix Fill Step
        for (int i = 1; i < secondCnt; i++)
        {
            for (int j = 1; j < firstCnt; j++)
            {
                int scroeDiag = 0;
                if (first.Substring(j - 1, 1) == second.Substring(i - 1, 1))
                    scroeDiag = scoringMatrix[i - 1, j - 1] + 0;
                else
                    scroeDiag = scoringMatrix[i - 1, j - 1] + -1;

                int scroeLeft = scoringMatrix[i, j - 1] - 1;
                int scroeUp = scoringMatrix[i - 1, j] - 1;

                int maxScore = Math.Max(Math.Max(scroeDiag, scroeLeft), scroeUp);

                scoringMatrix[i, j] = maxScore;
            }
        }

        //Traceback Step
        char[] secondArray = second.ToCharArray();
        char[] firstArray = first.ToCharArray();

        string AlignmentA = string.Empty;
        string AlignmentB = string.Empty;
        int m = secondCnt - 1;
        int n = firstCnt - 1;
        while (m > 0 || n > 0)
        {
            int scroeDiag = 0;

            if (m == 0 && n > 0)
            {
                AlignmentA = firstArray[n - 1] + AlignmentA;
                AlignmentB = "-" + AlignmentB;
                n = n - 1;
            }
            else if (n == 0 && m > 0)
            {
                AlignmentA = "-" + AlignmentA;
                AlignmentB = secondArray[m - 1] + AlignmentB;
                m = m - 1;
            }
            else
            {
                //Remembering that the scoring scheme is +0 for a match, -1 for a mismatch, and -1 for a gap
                if (secondArray[m - 1] == firstArray[n - 1])
                    scroeDiag = 0;
                else
                    scroeDiag = -1;

                if (m > 0 && n > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n - 1] + scroeDiag)
                {
                    AlignmentA = firstArray[n - 1] + AlignmentA;
                    AlignmentB = secondArray[m - 1] + AlignmentB;
                    m = m - 1;
                    n = n - 1;
                }
                else if (n > 0 && scoringMatrix[m, n] == scoringMatrix[m, n - 1] - 1) // gap
                {
                    AlignmentA = firstArray[n - 1] + AlignmentA;
                    AlignmentB = "-" + AlignmentB;
                    n = n - 1;
                }
                else if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -1)
                {
                    AlignmentA = "-" + AlignmentA;
                    AlignmentB = secondArray[m - 1] + AlignmentB;
                    m = m - 1;
                }
            }
        }

        return AlignmentA;
    }

    public int AllignedDistance(string word1, string word2)
    {
        var distance = 0;
        for(int i = 0; i < word1.Length; i++)
        {
            if(word1[i] != word2[i])
            {
                distance++;
            }
        }
        return distance;
    }

}
