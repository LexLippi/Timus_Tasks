using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segments
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var segments = new int[n, 2];
            var stack = new Stack<Tuple<int, int, int>>();
            var ip = 0;
            for (var i = 0; i < n; ++i)
            {
                var numbers = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
                segments[i, 0] = numbers[0];
                segments[i, 1] = numbers[1];
            }
            var m = int.Parse(Console.ReadLine());
            for (var i = 0; i < m; ++i)
            {
                var p = int.Parse(Console.ReadLine());
                while (true)
                {
                    while (ip < n && segments[ip, 0] <= p)
                    {
                        stack.Push(Tuple.Create(segments[ip, 0], segments[ip, 1], ++ip));
                    }
                    while (stack.Count > 0 && p > stack.Peek().Item2)
                    {
                        stack.Pop();
                    }
                    if (stack.Count == 0)
                    {
                        Console.WriteLine(-1);
                        break;
                    }
                    if (stack.Peek().Item2 >= p)
                    {
                        Console.WriteLine(stack.Peek().Item3);
                        break;
                    }
                }
            }
        }
    }
}
