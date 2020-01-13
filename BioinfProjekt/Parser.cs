using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Parser
{
    public List<Gene> ParseFile(string filePath)
    {
        var result = new List<Gene>();
        StreamReader reader = File.OpenText(filePath);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            var matchReg = "ACTG-";
            if (line.All(l => matchReg.Contains(l)))
            {
                var gene = new Gene(line);
                result.Add(gene);
            }
        }

        result = this.filterData(result);

        return result;
    }

    public List<Gene> filterData(List<Gene> genes)
    {
        var longest = 0;
        foreach (var gene in genes)
        {
            if (gene.sequence.Length > longest)
            {
                longest = gene.sequence.Length;
            }
        }
        var list = new List<int>(new int[longest + 1]);
        genes.ForEach(g => list[g.sequence.Length]++);
        var mostOccuringLength = list.IndexOf(list.Max());

        Console.Out.WriteLine("Most occuring sequence length: " + mostOccuringLength.ToString());

        //genes.RemoveAll(g => g.sequence.Length != mostOccuringLength);
        genes.RemoveAll(g => g.sequence.Length < mostOccuringLength - 5 || g.sequence.Length > mostOccuringLength + 5);

        return genes;
    }

    public List<Gene> filterDataByAllignement(List<Gene> genes)
    {
        var longest = 0;
        foreach (var gene in genes)
        {
            if (gene.allignedSequence.Length > longest)
            {
                longest = gene.allignedSequence.Length;
            }
        }
        var list = new List<int>(new int[longest + 1]);
        genes.ForEach(g => list[g.allignedSequence.Length]++);
        var mostOccuringLength = list.IndexOf(list.Max());

        Console.Out.WriteLine("Most occuring sequence length: " + mostOccuringLength.ToString() + "\n");
        Console.Out.WriteLine("Number of alligned readings before filtering:" + genes.Count + "\n");

        genes.RemoveAll(g => g.allignedSequence.Length != mostOccuringLength);
        Console.Out.WriteLine("Number of alligned readings after filtering:" + genes.Count + "\n");


        return genes;
    }
}
