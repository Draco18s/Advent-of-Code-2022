using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayThree {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long sum = 0;
			foreach(string lin in lines)
			{
				string left = lin.Substring(0, lin.Length / 2);
				string right = lin.Substring(lin.Length / 2);
				for (int i =0; i < left.Length; i++)
				{
					if(right.Contains(left[i]))
					{
						char chr = left[i];
						int prio = GetPriority(chr);
						
						sum += prio;
						break;
					}
				}
			}

			return sum;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			for(int l = 0; l < lines.Length; l+=3)
			{
				string lin1 = lines[l];
				string lin2 = lines[l+1];
				string lin3 = lines[l + 2];
				foreach(char c in lin1)
				{
					if(lin2.Contains(c) && lin3.Contains(c))
					{
						int prio = GetPriority(c);
						sum += prio;
						break;
					}
				}
			}

			return sum;
		}

		static int GetPriority(char c)
		{
			if (c >= 'A' && c <= 'Z') return c - 'A' + 1 + 26;
			if (c >= 'a' && c <= 'z') return c - 'a' + 1;
			return 0;
		}
	}
}