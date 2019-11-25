using System;
using System.Globalization;


namespace Exchange
{
    class Program
    {
        static void Main(string[] args)
        {
            var bid = new FenwickTree();
            var answer = 0L;
            while (true)
            {
                var currentAction = Console.ReadLine().Split(' ');
                if (currentAction.Length == 1) break;
                var name = currentAction[0];
                var value = double.Parse(currentAction[1], CultureInfo.InvariantCulture);
                var start = 1000001 - (int)(value * 100.0 + 0.1);
                if (name == "BID")
                {
                    bid.Add(start, 1);
                }
                if (name == "DEL")
                {
                    bid.Add(start, -1);
                }   
                if (name == "SALE")
                {
                    var count = int.Parse(currentAction[2]);
                    answer += Math.Max(0, Math.Min(count, bid.Sum(start)));
                }
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Console.WriteLine(answer / 100.0);
        }

        public class FenwickTree
        {
            private readonly int[] data = new int[1000001]; 

            public int Sum(int number)
            {
                var res = 0;
                while (number > 0)
                {
                    res += data[number];
                    number -= number & (-number);
                }
                return res;
            }

            public void Add(int number, int value)
            {
                while (number < 1000001)
                {
                    data[number] += value;
                    number += number & (-number);
                }
            }
        }
    }
}
