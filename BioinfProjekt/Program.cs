using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BioinfProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter filepath to the data:");
            var dataPath = Console.ReadLine();
            Console.WriteLine("Please enter folder path to store result files in:");
            var resultPath = Console.ReadLine();
            var parser = new Parser();
            var algorithm = new BioAlgorithms();
            var genes = parser.ParseFile(dataPath);

            int acceptableNumberForCluster = Convert.ToInt32(Math.Round(genes.Count() * 12d / 100));


            var allignedClusters = new List<List<Gene>>();
            Console.Out.Write("Number of sequences: " + genes.Count());
            var firstCluster = new List<Gene>();
            allignedClusters.Add(firstCluster);
            var whileloop = 1;
            var firstPass = true;

            var allignedByAllGene = genes[0].sequence;
            for (int j = 1; j < genes.Count; j++)
            {
                allignedByAllGene = algorithm.NeedlemanWunschAllignFirst(allignedByAllGene, genes[j].sequence);
            }

            for (int i = 0; i < genes.Count; i++)
            {
                genes[i].allignedSequence = algorithm.NeedlemanWunschAllignFirst(genes[i].sequence, allignedByAllGene);
            }

            genes = parser.filterDataByAllignement(genes);

            var lastPassGenesCovered = 0;
            var beforelastPassGenesCovered = 0;

            while (whileloop < 20)
            {
                //For every gene we have
                for (int i = 0; i < genes.Count; i++)
                {
                    var firstGene = genes[i];
                    //If this is the first gene, add it to the first cluster
                    if (firstPass && i == 0)
                    {
                        allignedClusters[0].Add(genes[i]);
                        continue;
                    }
                    var clusterIndex = 0;
                    var minDistance = 0;


                    //For every cluster that we have
                    for (int j = 0; j < allignedClusters.Count(); j++)
                    {
                        var clusterDistance = 0;

                        //Calculate mean distance from cluster by summing distance from every gene 
                        //in that cluster and dividing it by number of genes in that cluster
                        for (int k = 0; k < allignedClusters[j].Count(); k++)
                        {
                            var secondGene = allignedClusters[j][k];
                            var distance = algorithm.AllignedDistance(firstGene.allignedSequence, secondGene.allignedSequence);

                            //clusterDistance += distance;
                            clusterDistance += distance;
                        }

                        var meanDistance = clusterDistance / allignedClusters[j].Count();


                        if (minDistance == 0 || meanDistance < minDistance)
                        {
                            minDistance = meanDistance;
                            clusterIndex = j;
                        }
                    }

                    if (minDistance > 12)
                    {
                        allignedClusters.Add(new List<Gene>());
                        allignedClusters[allignedClusters.Count() - 1].Add(genes[i]);
                    }
                    else
                    {
                        if (!firstPass)
                        {
                            foreach (var cluster in allignedClusters)
                            {
                                if (cluster.Contains(genes[i]))
                                {
                                    cluster.Remove(genes[i]);
                                }
                            }
                        }
                        if (!allignedClusters[clusterIndex].Contains(genes[i]))
                        {
                            allignedClusters[clusterIndex].Add(genes[i]);
                        }                        
                    }
                    //Console.Out.WriteLine(maxSimilarity);

                }

                
                if (firstPass)
                {
                    firstPass = false;
                }
                whileloop++;
                allignedClusters = TakeBiggestClusters(allignedClusters, acceptableNumberForCluster);
                var genesCovered = 0;
                allignedClusters.ForEach(c => genesCovered += c.Count);
                if (lastPassGenesCovered == genesCovered || beforelastPassGenesCovered == genesCovered) break;
                beforelastPassGenesCovered = lastPassGenesCovered;
                lastPassGenesCovered = genesCovered;
            }
            
            WriteClustersToFastaFiles(allignedClusters, resultPath);
            Console.Out.WriteLine("Done");
            Console.ReadKey();
            
        }

        private static List<List<Gene>> TakeBiggestClusters(List<List<Gene>> allignedClusters, int numberOfGenes)
        {
            var newAllignedClusters = new List<List<Gene>>();
            foreach (var c in allignedClusters)
            {
                if (c.Count() > numberOfGenes)
                {
                    newAllignedClusters.Add(c);
                }
            }
            return newAllignedClusters;
        }

        private static void WriteClustersToFastaFiles(List<List<Gene>> clusters, string resultPath)
        {
            Directory.CreateDirectory(resultPath);
            for(int i = 0; i < clusters.Count; i++)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(resultPath + "\\cluster" + (i+1).ToString() + ".fasta"))
                {
                    var counter = 1;
                    foreach (var gene in clusters[i])
                    {
                        file.WriteLine(">Gene" + counter.ToString());
                        counter++;
                        file.WriteLine(gene.sequence);
                    }
                }
            }
            
        }
    }
}
