using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BioinfProjekt
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter filepath to the data:");
            var dataPath = Console.ReadLine();

            Console.WriteLine("Please enter filepath to spoa executable: ");
            var spoaPath = Console.ReadLine();

            Console.WriteLine("Please enter folder path to store result files in:");
            var resultPath = Console.ReadLine();


            var parser = new Parser();
            var algorithm = new BioAlgorithms();
            var genes = parser.ParseFile(dataPath);

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

            var distance = Math.Ceiling(mostOccuringLength * 3d / 100);
            var maxDistance = (int)Math.Ceiling(distance * 1.2d * 2);

            int acceptableNumberForCluster = Convert.ToInt32(Math.Round(genes.Count() * 8d / 100));


            var clusters = new List<Cluster>();
            Console.Out.Write("Number of sequences: " + genes.Count());
            Console.Out.WriteLine("\n");
            var firstCluster = new Cluster();
            clusters.Add(firstCluster);
            var firstPass = true;

            //For every gene we have
            for (int i = 0; i < genes.Count; i++)
            {
                var firstGene = genes[i];
                //If this is the first gene, add it to the first cluster
                if (firstPass && i == 0)
                {
                    clusters[0].sequences.Add(genes[i].sequence);
                    clusters[0].centroid = genes[i].sequence;
                    continue;
                }
                var clusterIndex = -1;
                var minDistance = 0;

                var validClusters = new List<int>();


                //For every cluster that we have
                for (int j = 0; j < clusters.Count(); j++)
                {
                    var clusterDistance = algorithm.LevensteinDistance(firstGene.sequence, clusters[j].centroid);



                    if (minDistance == 0 || clusterDistance < minDistance)
                    {
                        minDistance = clusterDistance;
                        clusterIndex = j;
                    }
                }

                if (minDistance < maxDistance)
                {
                    foreach (var cluster in clusters)
                    {
                        if (cluster.sequences.Contains(genes[i].sequence))
                        {
                            cluster.sequences.Remove(genes[i].sequence);
                        }
                    }

                    clusters[clusterIndex].sequences.Add(genes[i].sequence);

                    WriteGenesToFastaFiles(clusters[clusterIndex].sequences, resultPath + "/temp");
                    clusters[clusterIndex].centroid = CallSpoaForConsensus(resultPath + "/temp/genes.fasta", spoaPath);
                }
                else
                {
                    clusters.Add(new Cluster());
                    clusters[clusters.Count() - 1].sequences.Add(genes[i].sequence);
                    clusters[clusters.Count() - 1].centroid = genes[i].sequence;
                }

            }
            clusters = TakeBiggestClusters(clusters, acceptableNumberForCluster);

            var resultList = WriteClustersToFastaFiles(clusters, resultPath, spoaPath);

            foreach (var consensus in resultList)
            {
                Console.Out.Write("Consensus(" + consensus.Length + "):\n");
                Console.Out.WriteLine(consensus);
                Console.Out.Write("\n");
            }
            Console.Out.WriteLine("Done");
            Console.ReadKey();

        }

        private static List<Cluster> TakeBiggestClusters(List<Cluster> clusters, int numberOfGenes)
        {
            var newclusters = new List<Cluster>();
            foreach (var c in clusters)
            {
                if (c.sequences.Count() > numberOfGenes)
                {
                    newclusters.Add(c);
                }
            }
            return newclusters;
        }

        private static List<string> WriteClustersToFastaFiles(List<Cluster> clusters, string resultPath, string spoaPath)
        {
            var result = new List<string>();
            Directory.CreateDirectory(resultPath);
            for (int i = 0; i < clusters.Count; i++)
            {
                using (System.IO.StreamWriter file =
                    new System.IO.StreamWriter(resultPath + "/cluster" + (i + 1).ToString() + ".fasta"))
                {
                    var counter = 1;
                    foreach (var sequence in clusters[i].sequences)
                    {
                        file.WriteLine(">Gene" + counter.ToString());
                        counter++;
                        file.WriteLine(sequence);
                    }
                }

                var consensusForCluster = CallSpoaForConsensus(resultPath + "/cluster" + (i + 1).ToString() + ".fasta", spoaPath);
                result.Add(consensusForCluster);
            }

            return result;
        }

        private static void WriteGenesToFastaFiles(List<string> sequences, string fileSavePath)
        {
            Directory.CreateDirectory(fileSavePath);
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(fileSavePath + "/genes.fasta"))
            {
                var counter = 1;
                foreach (var sequence in sequences)
                {
                    file.WriteLine(">Gene" + counter.ToString());
                    counter++;
                    file.WriteLine(sequence);
                }
            }

        }

        private static string CallSpoaForConsensus(string pathToFile, string spoaPath)
        {
            Process compiler = new Process();
            compiler.StartInfo.FileName = spoaPath;
            compiler.StartInfo.Arguments = pathToFile;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            string output = compiler.StandardOutput.ReadToEnd();

            compiler.WaitForExit();

            var consensusList = output.Split('\n').ToList();

            var consenesus = "";

            consensusList.ForEach(s =>
            {
                var matchReg = "ACTG-";
                if (s.All(c => matchReg.Contains(c)) && s != "")
                {
                    consenesus = s;
                }
            });

            return consenesus;
        }
    }
}
