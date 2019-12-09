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
            if(line.All(l => matchReg.Contains(l)) && (line.Length < 250 && line.Length > 230))
            {
                var gene = new Gene(line);
                result.Add(gene);
            }
        }

        return result;
    }
}
