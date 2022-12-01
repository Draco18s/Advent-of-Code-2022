using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayOne {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<int> carry = new List<int>();
			foreach(string lin in lines) {
				if(string.IsNullOrWhiteSpace(lin))
				{
					carry.Add(sum);
					sum = 0;
					continue;
				}
				int v = int.Parse(lin);
				sum += v;
			}
			return carry.Max();
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<int> carry = new List<int>();
			foreach (string lin in lines)
			{
				if (string.IsNullOrWhiteSpace(lin))
				{
					carry.Add(sum);
					sum = 0;
					continue;
				}
				int v = int.Parse(lin);
				sum += v;
			}
			carry.Sort();
			carry.Reverse();
			return carry[0]+carry[1]+carry[2];
		}
	}
}