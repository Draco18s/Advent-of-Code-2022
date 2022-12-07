using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DaySix
	{
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Queue<char> q = new Queue<char>();
			foreach(char c in lines[0])
			{
				sum++;
				q.Enqueue(c);
				if(q.Count == 4)
				{
					if(q.Distinct().Count() == 4)
					{
						return sum;
					}
					q.Dequeue();
				}
			}
			return sum;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Queue<char> q = new Queue<char>();
			foreach (char c in lines[0])
			{
				sum++;
				q.Enqueue(c);
				if (q.Count == 14)
				{
					if (q.Distinct().Count() == 14)
					{
						return sum;
					}
					q.Dequeue();
				}
			}
			return sum;
		}
	}
}