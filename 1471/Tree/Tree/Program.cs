using System;
using System.Collections.Generic;
using System.Linq;

namespace Tree
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var graph = new Graph();
            for (var i = 1; i < n; ++i)
            {
                var input = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
                graph.AddNodes(input[0], input[1], input[2]);
            }
            graph.Preprocess(n);
            var m = int.Parse(Console.ReadLine());
            for (var i = 0; i < m; ++i)
            {
                var input = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
                Console.WriteLine(graph.Sum[input[0]] + graph.Sum[input[1]]
                                  - 2L * graph.Sum[graph.LCA(input[0], input[1])]);
            }
        }

        public class Graph
        {
            public const int maxNodes = 50005;
            public const int maxHeight = 22;
            public long[] Sum = new long[maxNodes];
            private readonly List<Tuple<int, int>>[] matrix = new List<Tuple<int, int>>[maxNodes];
            private readonly int[] level = new int[maxNodes];
            private readonly int[,] ancestors = new int[maxNodes, maxHeight];

            public void AddNodes(int firstNode, int secondNode, int weight)
            {
                if (matrix[firstNode] is null)
                {
                    matrix[firstNode] = new List<Tuple<int, int>>();
                }
                if (matrix[secondNode] is null)
                {
                    matrix[secondNode] = new List<Tuple<int, int>>();
                }
                matrix[firstNode].Add(Tuple.Create(secondNode, weight));
                matrix[secondNode].Add(Tuple.Create(firstNode, weight));
            }

            public void DFS(int node)
            {
                var stack = new Stack<int>();
                stack.Push(node);
                while (stack.Count != 0)
                {
                    var currentNode = stack.Pop();
                    foreach (var secondNode in matrix[currentNode])
                    {
                        if (secondNode.Item1 != ancestors[currentNode, 0])
                        {
                            level[secondNode.Item1] = level[currentNode] + 1;
                            ancestors[secondNode.Item1, 0] = currentNode;
                            Sum[secondNode.Item1] = Sum[currentNode] + secondNode.Item2;
                            stack.Push(secondNode.Item1);
                        }
                    }
                }
            }

            public void Preprocess(int n)
            {
                if (n != 1)
                {
                    DFS(0);
                }
                for (var j = 1; (1 << j) < n; ++j)
                {
                    for (var i = 0; i < n; ++i)
                    {
                        ancestors[i, j] = ancestors[ancestors[i, j - 1], j - 1];
                    }
                }
            }

            public int LCA(int firstNode, int secondNode)
            {
                var levelDist = Math.Abs(level[firstNode] - level[secondNode]);
                if (level[firstNode] > level[secondNode])
                {
                    var swap = firstNode;
                    firstNode = secondNode;
                    secondNode = swap;
                }
                for (var i = 0; (1 << i) <= levelDist; ++i)
                {
                    if ((levelDist & (1 << i)) != 0)
                    {
                        secondNode = ancestors[secondNode, i];
                    }
                }
                if (firstNode == secondNode)
                {
                    return firstNode;
                }
                for (var i = maxHeight - 1; i >= 0; --i)
                {
                    if (ancestors[firstNode, i] != ancestors[secondNode, i])
                    {
                        firstNode = ancestors[firstNode, i];
                        secondNode = ancestors[secondNode, i];
                    }
                }
                return ancestors[firstNode, 0];
            }
        }
    }
}