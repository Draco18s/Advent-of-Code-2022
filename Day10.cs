using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022
{
	internal static class DayTen
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			int ticks = 0;
			int register = 1;
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(' ');
				if (parts[0] == "noop")
				{
					sum += DoTick(ref ticks, ref register);
					continue;
				}
				if (parts[0] == "addx")
				{
					sum += DoTick(ref ticks, ref register);
					sum += DoTick(ref ticks, ref register);
					register += int.Parse(parts[1]);
				}
			}
			return sum;
		}

		private static int DoTick(ref int ticks, ref int register)
		{
			ticks++;
			if (ticks == 20 || ticks == 60 || ticks == 100 || ticks == 140 || ticks == 180 || ticks == 220)
			{
				return register * ticks;
			}
			return 0;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			int ticks = 0;
			int register = 1;
			Grid screen = new Grid(40, 6);
			int y = 0;
			int x = 0;
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(' ');
				if (parts[0] == "noop")
				{
					DoTick2(ref ticks, ref register, ref screen, ref x, ref y);
					continue;
				}
				if (parts[0] == "addx")
				{
					DoTick2(ref ticks, ref register, ref screen, ref x, ref y);
					DoTick2(ref ticks, ref register, ref screen, ref x, ref y);
					register += int.Parse(parts[1]);
				}
			}
			Console.WriteLine(screen.ToString("char+0"));
			return sum;
		}

		private static void DoTick2(ref int ticks, ref int register, ref Grid screen, ref int x, ref int y)
		{
			ticks++;
			screen[x, y] = Math.Abs(register - x) <= 1 ? '#' : '.';
			x++;
			if (x >= screen.Width)
			{
				x = 0;
				y++;
			}
			if (y >= screen.Height) y = 0;
		}
	}
}