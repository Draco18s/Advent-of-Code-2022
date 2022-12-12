using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayTwelve {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			Grid mountain = new Grid(input, true);
			Grid distances = new Grid(mountain.Width, mountain.Height);
			int x = 0, y = 0;
			foreach(string l in lines)
			{
				if(l.Contains('S'))
				{
					x = l.IndexOf('S');
					break;
				}
				y++;
			}
			distances[x, y] = 0;
			if(lines[y-1][x] == 'a')
			{
				y--;
			}
			else if (lines[y + 1][x] == 'a')
			{
				y++;
			}
			/*if (lines[y][x - 1] == 'a')
			{
				x--;
			}*/
			else if (lines[y][x + 1] == 'a')
			{
				x++;
			}
			distances[x, y] = 1;

			int r = CheckPath(mountain, new Vector2(x, y), distances);
			//Console.WriteLine(distances.ToString("char"));
			x = y = 0;
			foreach (string l in lines)
			{
				if (l.Contains('E'))
				{
					x = l.IndexOf('E');
					break;
				}
				y++;
			}
			//Console.WriteLine(distances.ToString());
			return distances[x, y];
			//return -1;
		}

		private static int BestChar = 'a';

		private static int CheckPath(Grid mountain, Vector2 pos, Grid dist)
		{
			if (mountain[pos] == 'E')
			{
				//Console.WriteLine($"Final value: {dist[pos]}");
				return dist[pos];
			}
			int h = mountain[pos];
			if(mountain[pos] > BestChar)
			{
				char a = (char)h;
				BestChar = a;
				int v = dist[pos];
				//if(a >= 'p')
					//Console.WriteLine($"{a}:{dist[pos]}");
			}
			if(pos.x - 1 >= 0 && ((mountain[pos.x-1,pos.y] - h) <= 1 || mountain[pos.x - 1, pos.y] == 'E'))
			{
				if(dist[pos.x - 1, pos.y] == 0 || dist[pos.x - 1, pos.y] > dist[pos.x, pos.y]+1)
				{
					dist[pos.x - 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPath(mountain, new Vector2(pos.x - 1, pos.y), dist);
				}
			}
			if (pos.y - 1 >= 0 && ((mountain[pos.x, pos.y - 1] - h) <= 1 || mountain[pos.x, pos.y - 1] == 'E'))
			{
				if (dist[pos.x, pos.y - 1] == 0 || dist[pos.x, pos.y - 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y - 1] = dist[pos.x, pos.y] + 1;
					CheckPath(mountain, new Vector2(pos.x, pos.y - 1), dist);
				}
			}

			if (pos.x + 1 < mountain.Width && ((mountain[pos.x + 1, pos.y] - h) <= 1 || mountain[pos.x + 1, pos.y] == 'E'))
			{
				if (dist[pos.x + 1, pos.y] == 0 || dist[pos.x + 1, pos.y] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x + 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPath(mountain, new Vector2(pos.x + 1, pos.y), dist);
				}
			}
			if (pos.y + 1 < mountain.Height && ((mountain[pos.x, pos.y + 1] - h) <= 1 || mountain[pos.x, pos.y + 1] == 'E'))
			{
				if (dist[pos.x, pos.y + 1] == 0 || dist[pos.x, pos.y + 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y + 1] = dist[pos.x, pos.y] + 1;
					CheckPath(mountain, new Vector2(pos.x, pos.y + 1), dist);
				}
			}
			return -1;
		}

		private static int bestdist = int.MaxValue;
		private static char last = 'z';

		private static int CheckPath2(Grid mountain, Vector2 pos, Grid dist)
		{
			if (mountain[pos] == 'S')
			{
				return -1;
			}
			if (mountain[pos] == 'a')
			{
				int v = dist[pos];
				dist[pos] = 0;
			}
			if (mountain[pos] == 'E')
			{
				//Console.WriteLine($"Final value: {dist[pos]}");
				return dist[pos];
			}
			int h = mountain[pos];
			if (mountain[pos] > BestChar)
			{
				char a = (char)h;
				BestChar = a;
				int v = dist[pos];
				//if(a >= 'p')
				//Console.WriteLine($"{a}:{dist[pos]}");
			}
			if (pos.x - 1 >= 0 && ((mountain[pos.x - 1, pos.y] - h) <= 1 || mountain[pos.x - 1, pos.y] == 'E'))
			{
				if (dist[pos.x - 1, pos.y] == -1 || dist[pos.x - 1, pos.y] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x - 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPath2(mountain, new Vector2(pos.x - 1, pos.y), dist);
				}
			}
			if (pos.y - 1 >= 0 && ((mountain[pos.x, pos.y - 1] - h) <= 1 || mountain[pos.x, pos.y - 1] == 'E'))
			{
				if (dist[pos.x, pos.y - 1] == -1 || dist[pos.x, pos.y - 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y - 1] = dist[pos.x, pos.y] + 1;
					CheckPath2(mountain, new Vector2(pos.x, pos.y - 1), dist);
				}
			}

			if (pos.x + 1 < mountain.Width && ((mountain[pos.x + 1, pos.y] - h) <= 1 || mountain[pos.x + 1, pos.y] == 'E'))
			{
				if (dist[pos.x + 1, pos.y] == -1 || dist[pos.x + 1, pos.y] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x + 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPath2(mountain, new Vector2(pos.x + 1, pos.y), dist);
				}
			}
			if (pos.y + 1 < mountain.Height && ((mountain[pos.x, pos.y + 1] - h) <= 1 || mountain[pos.x, pos.y + 1] == 'E'))
			{
				if (dist[pos.x, pos.y + 1] == -1 || dist[pos.x, pos.y + 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y + 1] = dist[pos.x, pos.y] + 1;
					CheckPath2(mountain, new Vector2(pos.x, pos.y + 1), dist);
				}
			}
			return -1;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			Grid mountain = new Grid(input, true);
			Grid distances = new Grid(mountain.Width, mountain.Height);
			int x = 0, y = 0;
			for(x=0;x<distances.Width;x++)
			{
				for (y=0; y < distances.Height; y++)
				{
					distances[x, y] = -1;
				}
			}
			x = y = 0;
			foreach (string l in lines)
			{
				if (l.Contains('S'))
				{
					x = l.IndexOf('S');
					break;
				}
				y++;
			}
			distances[x, y] = 0;
			if (lines[y - 1][x] == 'a')
			{
				y--;
			}
			else if (lines[y + 1][x] == 'a')
			{
				y++;
			}
			else if (lines[y][x + 1] == 'a')
			{
				x++;
			}
			distances[x, y] = 0;

			int r = CheckPath2(mountain, new Vector2(x, y), distances);
			//Console.WriteLine(distances.ToString("char"));
			x = y = 0;
			foreach (string l in lines)
			{
				if (l.Contains('E'))
				{
					x = l.IndexOf('E');
					break;
				}
				y++;
			}
			//Console.WriteLine(distances.ToString());
			return distances[x, y];
		}

		//This code doesn't work for my input.
		//It should work, but it returns 352 which is LONGER than part 1.
		//Which makes NO SENSE what so ever.
		//Start at the end
		//work backwards
		//if we find an 'a' while at 'b', report that distance, get the best result.
		//right?
		//then why is it finding a path longer than part 1?
		/*internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			Grid mountain = new Grid(input, true);
			Grid distances = new Grid(mountain.Width, mountain.Height);
			int x = 0, y = 0;
			foreach (string l in lines)
			{
				if (l.Contains('E'))
				{
					x = l.IndexOf('E'); //find the end
					break;
				}
				y++;
			}
			distances[x, y] = 0;
			if (lines[y - 1][x] == 'z') //get the next z (there's only one adj)
			{
				y--;
			}
			else if (lines[y + 1][x] == 'z')
			{
				y++;
			}
			if (lines[y][x - 1] == 'z')
			{
				x--;
			}
			else if (lines[y][x + 1] == 'z')
			{
				x++;
			}
			distances[x, y] = 1; //z is one step from E
			BestChar = 'z';
			int r = CheckPathBroken(mountain, new Vector2(x, y), distances);
			return r;
		}

		private static int CheckPathBroken(Grid mountain, Vector2 pos, Grid dist)
		{
			if (mountain[pos] == 'E') return -1;
			if (mountain[pos] == 'a') //if at a, return best distance
			{
				if(dist[pos] < bestdist)
				{
					bestdist = dist[pos];
				}
				return bestdist;
			}
			int h = mountain[pos];
			char a = (char)h;
			last = a;

			//check one step forwards in each direction, if we find an 'a' and are on 'b', we found *a* path to *an* 'a'
			if (pos.x - 1 >= 0 && ((h - mountain[pos.x - 1, pos.y]) <= 1 || (mountain[pos.x - 1, pos.y] == 'a' && last == 'b')))
			{
				if (dist[pos.x - 1, pos.y] == 0 || dist[pos.x - 1, pos.y] > dist[pos.x, pos.y] + 1) //use the better distance
				{
					dist[pos.x - 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPathBroken(mountain, new Vector2(pos.x - 1, pos.y), dist); //recurse
				}
			}
			if (pos.y - 1 >= 0 && ((h - mountain[pos.x, pos.y - 1]) <= 1 || (mountain[pos.x, pos.y - 1] == 'a' && last == 'b')))
			{
				if (dist[pos.x, pos.y - 1] == 0 || dist[pos.x, pos.y - 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y - 1] = dist[pos.x, pos.y] + 1;
					CheckPathBroken(mountain, new Vector2(pos.x, pos.y - 1), dist);
				}
			}

			if (pos.x + 1 < mountain.Width && ((h-mountain[pos.x + 1, pos.y]) <= 1 || (mountain[pos.x + 1, pos.y] == 'a' && last == 'b')))
			{
				if (dist[pos.x + 1, pos.y] == 0 || dist[pos.x + 1, pos.y] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x + 1, pos.y] = dist[pos.x, pos.y] + 1;
					CheckPathBroken(mountain, new Vector2(pos.x + 1, pos.y), dist);
				}
			}
			if (pos.y + 1 < mountain.Height && ((h-mountain[pos.x, pos.y + 1]) <= 1 || (mountain[pos.x, pos.y + 1] == 'a' && last == 'b')))
			{
				if (dist[pos.x, pos.y + 1] == 0 || dist[pos.x, pos.y + 1] > dist[pos.x, pos.y] + 1)
				{
					dist[pos.x, pos.y + 1] = dist[pos.x, pos.y] + 1;
					CheckPathBroken(mountain, new Vector2(pos.x, pos.y + 1), dist);
				}
			}
			return bestdist;
		}*/
	}
}