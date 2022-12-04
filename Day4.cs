using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent_of_Code_2021
{
	internal static class DayFour
	{
		private class ElfTask
		{
			public int start;
			public int end;
		}

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(',');
				string[] p1 = parts[0].Split('-');
				string[] p2 = parts[1].Split('-');
				ElfTask a = new ElfTask()
				{
					start = int.Parse(p1[0]),
					end = int.Parse(p1[1])
				};
				ElfTask b = new ElfTask()
				{
					start = int.Parse(p2[0]),
					end = int.Parse(p2[1])
				};
				if(CanCombine(a, b))
				{
					sum++;
				}
			}
			return sum;
		}

		private static bool CanCombine(ElfTask taskA, ElfTask taskB)
		{
			if (taskA.start <= taskB.start && taskA.end >= taskB.end) return true;
			if (taskA.start >= taskB.start && taskA.end <= taskB.end) return true;
			return false;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(',');
				string[] p1 = parts[0].Split('-');
				string[] p2 = parts[1].Split('-');
				ElfTask a = new ElfTask()
				{
					start = int.Parse(p1[0]),
					end = int.Parse(p1[1])
				};
				ElfTask b = new ElfTask()
				{
					start = int.Parse(p2[0]),
					end = int.Parse(p2[1])
				};
				if (DoOverlap(a, b))
				{
					sum++;
				}
			}
			return sum;
		}

		private static bool DoOverlap(ElfTask taskA, ElfTask taskB)
		{
			if (taskA.end >= taskB.start && taskA.start <= taskB.end) return true;
			if (taskB.end >= taskA.start && taskB.start <= taskA.end) return true;
			return false;
		}
	}
}
