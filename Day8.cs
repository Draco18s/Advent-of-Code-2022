using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayEight {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			Grid forest = new Grid(lines[0].Length, lines.Length);

			for (int y = 0; y < lines.Length; y++)
			{
				DoVisCheckY(ref forest, lines, y);
			}

			for (int x = 0; x < lines[0].Length; x++)
			{
				DoVisCheckX(ref forest, lines, x);
			}

			for (int y = 0; y < lines.Length; y++)
			{
				for (int x = 0; x < lines[0].Length; x++)
				{
					if(forest[x,y] == 1)
					{
						sum++;
					}
				}
			}

			return sum;
		}

		private static void DoVisCheckY(ref Grid forest, string[] lines, int y)
		{
			char curHeight = '0';
			curHeight--;
			for (int x = 0; x < lines[0].Length; x++)
			{
				if (lines[y][x] > curHeight)
				{
					forest[x, y] = 1;
					curHeight = lines[y][x];
				}
			}
			curHeight = '0';
			curHeight--;
			for (int x = lines[0].Length - 1; x >= 0; x--)
			{
				if (lines[y][x] > curHeight)
				{
					forest[x, y] = 1;
					curHeight = lines[y][x];
				}
			}
		}

		private static void DoVisCheckX(ref Grid forest, string[] lines, int x)
		{
			char curHeight = '0';
			curHeight--;
			for (int y = 0; y < lines.Length; y++)
			{
				if (lines[y][x] > curHeight)
				{
					forest[x, y] = 1;
					curHeight = lines[y][x];
				}
			}
			curHeight = '0';
			curHeight--;
			for (int y = lines.Length - 1; y >= 0; y--)
			{
				if (lines[y][x] > curHeight)
				{
					forest[x, y] = 1;
					curHeight = lines[y][x];
				}
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long sum = 0;
			for (int y = 0; y < lines.Length; y++)
			{
				for (int x = 0; x < lines[0].Length; x++)
				{
					long v = GetScenicScore(lines, x, y);
					if (v > sum) sum = v;
				}
			}

			return sum;
		}

		private static long GetScenicScore(string[] lines, int x, int y)
		{
			int val = 1;
			val *= GetUp(lines, x, y, lines[y][x], 1);
			val *= GetUp(lines, x, y, lines[y][x], -1);
			val *= GetRight(lines, x, y, lines[y][x], 1);
			val *= GetRight(lines, x, y, lines[y][x], -1);
			return val;
		}

		private static int GetUp(string[] lines, int x, int y, char height, int inc)
		{
			int count = 0;
			for (y += inc; y >= 0 && y < lines.Length; y += inc)
			{
				count++;
				if (lines[y][x] >= height) break;
			}
			return count;
		}

		private static int GetRight(string[] lines, int x, int y, char height, int inc)
		{
			int count = 0;
			for (x += inc; x >= 0 && x < lines[0].Length; x += inc)
			{
				count++;
				if (lines[y][x] >= height) break;
			}
			return count;
		}
	}
}