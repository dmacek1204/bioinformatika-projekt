using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Please enter filepath to the data:");
            var path = Console.ReadLine();
            var parser = new Parser();
            var algorithm = new DistanceAlgorithm();
            var genes = parser.ParseFile(path);

        
            var clusters = new List<List<Gene>>();
            Console.Out.Write("Number of sequences: " + genes.Count());
            var firstCluster = new List<Gene>();
            clusters.Add(firstCluster);
            var whileloop = 1;
            var firstPass = true;
            var lastPassGenesCovered = 0;

            while (clusters.Count() != 3 || whileloop < 20)
            {
                //For every gene we have
                for (int i = 0; i < genes.Count; i++)
                {
                    var firstGene = genes[i];
                    //If this is the first gene, add it to the first cluster
                    if (firstPass)
                    {
                        clusters[0].Add(genes[i]);
                        firstPass = false;
                        continue;
                    }
                    var clusterIndex = 0;
                    var minDistance = 0;


                    //For every cluster that we have
                    for (int j = 0; j < clusters.Count(); j++)
                    {
                        var clusterDistance = 0;

                        //Calculate mean distance from cluster by summing distance from every gene 
                        //in that cluster and dividing it by number of genes in that cluster
                        for (int k = 0; k < clusters[j].Count(); k++)
                        {
                            var secondGene = clusters[j][k];
                            var distance = algorithm.MinDistance(firstGene.sequence, secondGene.sequence);
                            clusterDistance += distance;
                        }

                        var meanDistance = clusterDistance / clusters[j].Count();

                        if (minDistance == 0 || meanDistance < minDistance)
                        {
                            minDistance = meanDistance;
                            clusterIndex = j;
                        }
                    }

                    if (minDistance > 12)
                    {
                        clusters.Add(new List<Gene>());
                        clusters[clusters.Count() - 1].Add(genes[i]);
                    }
                    else
                    {
                        if (!clusters[clusterIndex].Contains(genes[i]))
                        {
                            clusters[clusterIndex].Add(genes[i]);

                        }
                    }

                }

                clusters = TakeThreeBiggestClusters(clusters);
                var genesCovered = 0;
                clusters.ForEach(c => genesCovered += c.Count);
                if (lastPassGenesCovered == genesCovered) break;
                lastPassGenesCovered = genesCovered;
                whileloop++;
            }
            

            Console.Out.WriteLine("Number of clusters: " + clusters.Count);
            for (int i = 0; i < clusters.Count; i++)
            {
                Console.Out.WriteLine(i + ". cluster: \n");
                clusters[i].ForEach(g => Console.Out.WriteLine(g.sequence + "\n"));
            }
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }

        private static List<List<Gene>> TakeThreeBiggestClusters(List<List<Gene>> clusters)
        {
            var first = 0;
            var second = 0;
            var third = 0;
            foreach(var c in clusters)
            {
                if (c.Count > first)
                {
                    first = c.Count;
                }
                if (c.Count > second && c.Count < first)
                {
                    second = c.Count;
                }
                if (c.Count > third && c.Count < second)
                {
                    third = c.Count;
                }
            }
            clusters = clusters.Where(c => c.Count == first || c.Count == second || c.Count == third).ToList();
            return clusters;
        }
    }
}
