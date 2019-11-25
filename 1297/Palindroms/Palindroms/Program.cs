using System;

namespace Palindroms
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputData = Console.ReadLine();
            var hashedString = new HashedString(inputData);
            var maxLen = 1;
            var start = 0;
            var oddCount = new int[hashedString.Length];
            for (var i = 0; i < hashedString.Length; ++i)
            {
                var left = 1;
                var right = Math.Min(i + 1, hashedString.Length - i);
                while (left <= right)
                {
                    var middle = (left + right) / 2;
                    if (hashedString.IsPalindrome(i - middle + 1, i + middle - 1))
                    {
                        oddCount[i] = middle;
                        left = middle + 1;
                    }
                    else
                    {
                        right = middle - 1;
                    }
                }
            }

            var evenCount = new int[hashedString.Length];
            for (var i = 0; i < hashedString.Length; ++i)
            {
                var left = 1;
                var right = Math.Min(i, hashedString.Length - i);
                while (left <= right)
                {
                    var middle = (left + right) / 2;
                    if (hashedString.IsPalindrome(i - middle, i + middle - 1))
                    {
                        evenCount[i] = middle;
                        left = middle + 1;
                    }
                    else
                    {
                        right = middle - 1;
                    }
                }
            }

            for (var i = 0; i < hashedString.Length; ++i)
            {
                var currentMaxLen = Math.Max(2 * evenCount[i], 2 * oddCount[i] - 1);
                if (maxLen < currentMaxLen)
                {
                    start = (2 * evenCount[i] < 2 * oddCount[i] - 1) 
                        ? i - (currentMaxLen - 1) / 2 : i - 1 - (currentMaxLen - 2) / 2;
                    maxLen = currentMaxLen;
                }
            }
            Console.WriteLine(inputData.Substring(start, maxLen));

        }
    }

    class HashedString
    {
        public int Length { get; }
        public long[] Hash { get; }
        public long[] ReverseHash { get; }
        public long[] Powers { get; }
        private readonly int mod = 1000003777;
        private readonly int prime = 37;


        public HashedString(string inputData)
        {
            Length = inputData.Length;
            Hash = new long[Length];
            ReverseHash = new long[Length];
            Powers = new long[Length];
            Hash[0] = inputData[0].GetHashCode();
            ReverseHash[0] = inputData[Length - 1].GetHashCode();
            Powers[0] = 1;
            for (var i = 1; i < Length; ++i)
            {
                Powers[i] = (prime * Powers[i - 1]) % mod;
                Hash[i] = (Hash[i - 1] * prime + inputData[i].GetHashCode()) % mod;
                ReverseHash[i] = (ReverseHash[i - 1] * prime + inputData[Length - 1 - i].GetHashCode()) % mod;
            }
        }

        public long GetHash(int left, int right) => left == 0 ? Hash[right] 
            : (Hash[right] - (Hash[left - 1] * Powers[right - left + 1]) % mod + mod) % mod;

        public long GetReverseHash(int left, int right) => right == Length - 1 ? ReverseHash[right - left]
            : (ReverseHash[Length - left - 1] - (ReverseHash[Length - right - 2] * Powers[right - left + 1]) % mod + mod) % mod;


        public bool IsPalindrome(int left, int right)
        {
            if (left == right) return true;
            if ((right - left + 1) % 2 == 0)
                return GetHash(left, (left + right) / 2) == GetReverseHash((left + right) / 2 + 1, right);
            else
                return GetHash(left, (left + right) / 2 - 1) == GetReverseHash((left + right) / 2 + 1, right);
        }
    }
}
