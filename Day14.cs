using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayFourteen {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Grid cave = new Grid(1000, 1000, 0, -1);
			for (int x = cave.MinX; x < cave.MaxX; x++)
			{
				for (int y = cave.MinY; y < cave.MaxY; y++)
				{
					if (cave[x, y, true] == 0)
					{
						cave[x, y, true] = '.';
					}
				}
			}
			cave[500, -1] = '+';
			foreach(string lin in lines) {
				if (string.IsNullOrEmpty(lin)) continue;
				string[] points = lin.Split(" -> ");
				Vector2 lastPoint = new Vector2(int.MinValue, int.MinValue);
				foreach (string point in points)
				{
					string[] ptVals = point.Split(',');
					Vector2 p1 = new Vector2(int.Parse(ptVals[0]), int.Parse(ptVals[1]));
					if(!cave.IsInside(p1))
					{
						cave.IncreaseGridToInclude(p1, Grid.returnZero);
					}
					if(lastPoint.x == int.MinValue)
					{
						lastPoint = p1;
						continue;
					}
					DrawLine(p1, lastPoint, cave);
					lastPoint = p1;
				}
			}
			cave.IncreaseGridBy(1, 1, Grid.returnZero);
			cave.IncreaseGridBy(-1, -1, Grid.returnZero);
			
			//Console.WriteLine(cave.ToString("char+0"));
			
			do
			{
				if(!DropOneSand(cave))
				{
					break;
				}
				//Console.WriteLine(cave.ToString("char+0"));
				sum++;
			} while (true);

			//Console.WriteLine(cave.ToString("char+0"));

			return sum;
		}

		private static bool DropOneSand(Grid cave)
		{
			Vector2 sand = new Vector2(500, 0);
			while (true)
			{
				if (!cave.IsInside(sand + new Vector2(0,1)))
				{
					return false;
				}
				if (cave[sand.x, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x, sand.y + 1);
				}
				else if (cave[sand.x - 1, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x - 1, sand.y + 1);
				}
				else if (cave[sand.x + 1, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x + 1, sand.y + 1);
				}
				else
				{
					cave[sand, true] = 'o';
					return true;
				}
			}
		}

		private static bool DropOneSand2(Grid cave)
		{
			Vector2 sand = new Vector2(500, 0);
			while (true)
			{
				if (cave[sand.x, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x, sand.y + 1);
				}
				else if (cave[sand.x - 1, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x - 1, sand.y + 1);
				}
				else if (cave[sand.x + 1, sand.y + 1, true] == '.')
				{
					sand = new Vector2(sand.x + 1, sand.y + 1);
				}
				else
				{
					cave[sand, true] = 'o';
					if (sand.y == 0)
					{
						return false;
					}
					return true;
				}
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Grid cave = new Grid(1000, 1000, 0, -1);
			//Grid cave = new Grid(20, 20, 490, -1);
			for (int x = cave.MinX; x < cave.MaxX; x++)
			{
				for (int y = cave.MinY; y < cave.MaxY; y++)
				{
					if (cave[x, y, true] == 0)
					{
						cave[x, y, true] = '.';
					}
				}
			}
			cave[500, 0] = '+';
			int maxY = -1000;
			foreach (string lin in lines)
			{
				if (string.IsNullOrEmpty(lin)) continue;
				string[] points = lin.Split(" -> ");
				Vector2 lastPoint = new Vector2(int.MinValue, int.MinValue);
				foreach (string point in points)
				{
					string[] ptVals = point.Split(',');
					Vector2 p1 = new Vector2(int.Parse(ptVals[0]), int.Parse(ptVals[1]));
					if (!cave.IsInside(p1))
					{
						cave.IncreaseGridToInclude(p1, Grid.returnZero);
					}
					if (lastPoint.x == int.MinValue)
					{
						lastPoint = p1;
						continue;
					}
					DrawLine(p1, lastPoint, cave);
					lastPoint = p1;
					if (p1.y > maxY) maxY = p1.y;
				}
			}
			maxY += 3;
			cave.IncreaseGridBy(maxY, 2, () => '.');
			cave.IncreaseGridBy(-maxY, -1, () => '.');
			maxY -= 1;
			for (int x=cave.MinX; x<cave.MaxX;x++)
			{
				cave[x, maxY] = '#';
			}

			//Console.WriteLine(cave.ToString("char+0"));

			do
			{
				if (!DropOneSand2(cave))
				{
					sum++;
					break;
				}
				sum++;
			} while (true);

			//Console.WriteLine(cave.ToString("char+0"));

			return sum;
		}

		#region Bresenham's Algorithm
		private static void DrawLine(Vector2 c1, Vector2 c2, Grid grid)
		{
			int x0 = c1.x;
			int x1 = c2.x;
			int y0 = c1.y;
			int y1 = c2.y;

			if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
			{
				if (x0 > x1)
					PlotLineLow(x1, y1, x0, y0, grid);
				else
					PlotLineLow(x0, y0, x1, y1, grid);
			}
			else if (y0 > y1)
				PlotLineHigh(x1, y1, x0, y0, grid);
			else
				PlotLineHigh(x0, y0, x1, y1, grid);
		}

		private static void PlotLineLow(int x0, int y0, int x1, int y1, Grid grid)
		{
			int dx = x1 - x0;
			int dy = y1 - y0;
			int yi = 1;
			if (dy < 0)
			{
				yi = -1;
				dy = -dy;
			}
			int D = (2 * dy) - dx;
			int y = y0;

			for (int x = x0; x <= x1; x++)
			{
				grid[x, y, true] = '#';
				//Console.WriteLine(grid.ToString("char+0"));
				//Console.WriteLine(" ");
				if (D > 0)
				{
					y += yi;
					D += 2 * (dy - dx);
				}
				else
					D += 2 * dy;
			}
		}

		private static void PlotLineHigh(int x0, int y0, int x1, int y1, Grid grid)
		{
			int dx = x1 - x0;
			int dy = y1 - y0;
			int xi = 1;
			if (dx < 0)
			{
				xi = -1;
				dx = -dx;
			}
			int D = (2 * dx) - dy;
			int x = x0;

			for (int y = y0; y <= y1; y++)
			{
				grid[x, y, true] = '#';
				///Console.WriteLine(grid.ToString("char+0"));
				//Console.WriteLine(" ");
				if (D > 0)
				{
					x += xi;
					D += 2 * (dx - dy);
				}
				else
					D += 2 * dx;
			}
		}
		#endregion
	}
}