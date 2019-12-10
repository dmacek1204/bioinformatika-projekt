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
            if(line.All(l => matchReg.Contains(l)))
            {
                var gene = new Gene(line);
                result.Add(gene);
            }
        }

        result = this.filterData(result);

        return result;
    }

    private List<Gene> filterData(List<Gene> genes)
    {
        var sumLength = 0;
        genes.ForEach(g => sumLength += g.sequence.Length);
        var meanLength = sumLength / genes.Count;

        Console.Out.WriteLine("Average sequence length: " + meanLength.ToString());

        genes.RemoveAll(g => g.sequence.Length < 220 || g.sequence.Length >= 250);

        return genes;
    }
}
