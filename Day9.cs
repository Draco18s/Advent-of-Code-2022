using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayNine {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			Grid area = new Grid(1000, 1000, -500, -500);
			Vector2 headpos = new Vector2(0, 0);
			Vector2 tailpos = new Vector2(0, 0);
			foreach (string lin in lines)
			{
				MoveRope(ref area, lin, ref headpos, ref tailpos);
			}

			for (int y = 0; y < area.Height; y++)
			{
				for (int x = 0; x < area.Width; x++)
				{
					//Console.Write(area[x, y, false] == 1 ? '#' : '.');
					if (area[x, y, false] == 1)
					{
						sum++;
					}
				}
				//Console.Write('\n');
			}

			return sum;
		}

		private static void MoveRope(ref Grid area, string lin, ref Vector2 headpos,ref  Vector2 tailpos)
		{
			string[] parts = lin.Split(' ');
			int steps = int.Parse(parts[1]);
			for (int a = 0; a < steps; a++)
			{
				switch (parts[0][0])
				{
					case 'U':
						headpos = new Vector2(headpos.x, headpos.y - 1);
						break;
					case 'D':
						headpos = new Vector2(headpos.x, headpos.y + 1);
						break;
					case 'L':
						headpos = new Vector2(headpos.x - 1, headpos.y);
						break;
					case 'R':
						headpos = new Vector2(headpos.x + 1, headpos.y);
						break;
				}
				Vector2 dist = (headpos - tailpos);
				if (dist.magnitude > 1.5)
				{
					tailpos = new Vector2(tailpos.x + Math.Sign(dist.x), tailpos.y + Math.Sign(dist.y));
				}
				area[tailpos] = 1;
			}
		}

		private static void MoveRope2(ref Grid area, string lin, ref List<Vector2> pos)
		{
			string[] parts = lin.Split(' ');
			int steps = int.Parse(parts[1]);
			for (int a = 0; a < steps; a++)
			{
				switch (parts[0][0])
				{
					case 'U':
						pos[0] = new Vector2(pos[0].x, pos[0].y - 1);
						break;
					case 'D':
						pos[0] = new Vector2(pos[0].x, pos[0].y + 1);
						break;
					case 'L':
						pos[0] = new Vector2(pos[0].x - 1, pos[0].y);
						break;
					case 'R':
						pos[0] = new Vector2(pos[0].x + 1, pos[0].y);
						break;
				}
				/*Vector2 dist = (headpos - tailpos);
				if (dist.magnitude > 1.5)
				{
					tailpos = new Vector2(tailpos.x + Math.Sign(dist.x), tailpos.y + Math.Sign(dist.y));
				}
				area[tailpos] = 1;*/
				for(int i=1;i<pos.Count;i++)
				{
					Vector2 dist = (pos[i-1] - pos[i]);
					if (dist.magnitude > 1.5)
					{
						pos[i] = new Vector2(pos[i].x + Math.Sign(dist.x), pos[i].y + Math.Sign(dist.y));
					}
				}
				area[pos[9]] = 1;
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Grid area = new Grid(3000, 3000, -1500, -1500);
			List<Vector2> pos = new List<Vector2>();
			for (int i = 0; i < 10; i++) pos.Add(new Vector2(0, 0));
			foreach (string lin in lines)
			{
				MoveRope2(ref area, lin, ref pos);
			}

			for (int y = 0; y < area.Height; y++)
			{
				for (int x = 0; x < area.Width; x++)
				{
					//Console.Write(area[x, y, false] == 1 ? '#' : '.');
					if (area[x, y, false] == 1)
					{
						sum++;
					}
				}
				//Console.Write('\n');
			}

			return sum;
		}
	}
}