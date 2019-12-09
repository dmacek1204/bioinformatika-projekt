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
            var firstCluster = new List<Gene>();
            clusters.Add(firstCluster);

            //For every gene we have
            for (int i = 0; i < genes.Count; i++)
            {
                var firstGene = genes[i];
                //If this is the first gene, add it to the first cluster
                if (i == 0)
                {
                    clusters[0].Add(genes[i]);
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
                    for(int k = 0; k < clusters[j].Count(); k++)
                    {
                        var secondGene = clusters[j][k];
                        var distance = algorithm.MinDistance(firstGene.sequence, secondGene.sequence);
                        clusterDistance += distance;
                    }

                    var meanDistance = clusterDistance / clusters[j].Count();

                    if(minDistance == 0 || meanDistance < minDistance)
                    {
                        minDistance = meanDistance;
                        clusterIndex = j;
                    }
                }

                if(minDistance > 8)
                {
                    clusters.Add(new List<Gene>());
                    clusters[clusters.Count() - 1].Add(genes[i]);
                }
                else
                {
                    clusters[clusterIndex].Add(genes[i]);
                }

            }

            Console.Out.WriteLine("Tri sam dana kukuruze brala");
            Console.Out.Write(clusters.Count());

            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
