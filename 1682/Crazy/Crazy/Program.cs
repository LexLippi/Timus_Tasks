using System;
using System.Collections.Generic;

namespace Crazy
{
    class Program
    {
        static void Main(string[] args)
        {
            var parameter = int.Parse(Console.ReadLine());
            Console.WriteLine(new DisjointSets().GetResult(parameter));
        }
    }

    public class Node
    {
        public Node Parent;
        public int Rank;
    }

    class DisjointSets
    {
        private const int maxParameter = 100001;
        private const long maxNodes = 2 * maxParameter + 10;
        private readonly List<long>[] data = new List<long>[maxNodes];        
        private readonly Node[] nodes = new Node[maxNodes];

        public DisjointSets()
        {
            for (var i = 0; i < maxNodes; ++i)
            {
                nodes[i] = new Node();
                data[i] = new List<long>();
            }
        }

        public void Connect(Node firstNode, Node secondNode)
        {
            if (firstNode.Rank > secondNode.Rank)
            {
                secondNode.Parent = firstNode;
            }
            else if (firstNode.Rank == secondNode.Rank)
            {
                firstNode.Parent = secondNode;
                secondNode.Rank++;
            }
            else
            {
                firstNode.Parent = secondNode;
            }
        }

        public Node Find(Node node)
        {
            if (node != node.Parent)
            {
                node.Parent = Find(node.Parent);
            }
            return node.Parent;
        }

        public void Join(Node firstNode, Node secondNode) => 
            Connect(Find(firstNode), Find(secondNode));

        public long GetResult(int parameter)
        {
            for (var i = 1L; i <= 2 * parameter + 5; ++i)
            {
                data[i * i % parameter].Add(i);
                nodes[i].Parent = nodes[i];
            }
            for (var i = 1L;; ++i)
            {
                var b1 = Math.Abs(parameter * maxNodes - i * i) % parameter;
                if (b1 != 0 && b1 < i)
                {
                    if(Find(nodes[b1]) == Find(nodes[i]))
                    {
                        return i;
                    }
                    else
                    {
                        Join(nodes[b1], nodes[i]);
                    }
                }
                var x = Math.Abs(parameter * maxNodes - i) % parameter;
                foreach (var b2 in data[x])
                {
                    if (b2 >= i)
                    {
                        break;
                    }
                    if (b2 != b1)
                    {
                        if (Find(nodes[b2]) == Find(nodes[i]))
                        {
                            return i;
                        }
                        else
                        {
                            Join(nodes[b2], nodes[i]);
                        }
                    }
                }
            }
        }
    }
}
