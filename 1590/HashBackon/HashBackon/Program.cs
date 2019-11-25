using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace Backon
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputString = Console.ReadLine();
            var sufTree = new SuffixTree(inputString);
            Console.WriteLine(sufTree.Length);

        }

        public class SuffixTree
        {
            public class SuffixTreeNode
            {
                public int offset;
                public int length;
                public Dictionary<char, SuffixTreeNode> link = new Dictionary<char, SuffixTreeNode>();

                public SuffixTreeNode(int offset, int length)
                {
                    this.offset = offset;
                    this.length = length;
                }
            }

            private readonly SuffixTreeNode Root;
            public int Length;
            
            public SuffixTree(string text)
            {
                text += '$';
                Root = new SuffixTreeNode(-1, -1);
                Root.link[text[0]] = new SuffixTreeNode(0, text.Length);
                Length += text.Length - 1;
                for (int i = 1; i < text.Length; i++)
                {
                    var current = Root;
                    var j = i;
                    while (j < text.Length)
                    {
                        if (current.link.ContainsKey(text[j]))
                        {
                            var child = current.link[text[j]];
                            var label = text.Substring(child.offset, child.length);
                            var k = j + 1;
                            while (k - j < label.Length && text[k] == label[k - j])
                            {
                                k += 1;
                            }
                            if (k - j == label.Length)
                            {
                                current = child;
                                j = k;
                            }
                            else
                            {
                                var existChar = label[k - j];
                                var newChar = text[k];
                                var mid = new SuffixTreeNode(child.offset, k - j);
                                mid.link[newChar] = new SuffixTreeNode(k, text.Length - k);
                                Length += text.Length - 1 - k;
                                mid.link[existChar] = child;
                                child.offset += k - j;
                                child.length -= k - j;
                                current.link[text[j]] = mid;
                            }
                        }
                        else
                        {
                            current.link[text[j]] = new SuffixTreeNode(j, text.Length - j);
                            Length += text.Length - 1 - j;
                        }
                    }
                }
            }
        }
    }
}