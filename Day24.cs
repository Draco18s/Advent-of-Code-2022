using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayTwentyfour {
		public enum Direction {  N, S, E, W }
		public class Blizzard
		{
			public Vector2 pos;
			public Vector2 dir;
		}

		public class ExpeditionPath
		{
			public Vector2 pos;

			public List<ExpeditionPath> next = new List<ExpeditionPath>();

			public override bool Equals(object obj)
			{
				if (obj is ExpeditionPath other)
					return other.pos == pos;
				return false;
			}

			public override int GetHashCode()
			{
				return pos.GetHashCode(); 
			}
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<Blizzard> blizzards = new List<Blizzard>();
			ExpeditionPath expedition = new ExpeditionPath();
			Vector2 exit = expedition.pos = new Vector2(-1, -1);
			for (int y = 0; y < lines.Length; y++) {
				for (int x = 0; x < lines[y].Length; x++)
				{
					if (lines[y][x] == '.' && expedition.pos.y == -1)
						expedition.pos = new Vector2(x, 0);
					else if (y == lines.Length - 1 && lines[y][x] == '.' && exit.y == -1)
						exit = new Vector2(x, y);
					if (lines[y][x] == '<')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(-1, 0)
						});
					}
					if (lines[y][x] == '>')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(1, 0)
						});
					}
					if (lines[y][x] == '^')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(0, -1)
						});
					}
					if (lines[y][x] == 'v')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(0, 1)
						});
					}
				}
			}

			int w = lines[0].Length;
			int h = lines.Length;

			List<ExpeditionPath> open = new List<ExpeditionPath>();
			List<ExpeditionPath> buffer = new List<ExpeditionPath>();
			open.Add(expedition);
			while (open.Count > 0)
			{
				sum++;
				UpdateBlizzards(blizzards, w - 1, h - 1);
				foreach (ExpeditionPath currExp in open)
				{
					GetExpeditionMoves(currExp, blizzards, exit, w - 1, h - 1);
					foreach (ExpeditionPath exp in currExp.next)
					{
						if (exp.pos == exit) return sum;
						if(!buffer.Contains(exp))
							buffer.Add(exp);
					}
				}
				open.Clear();
				open.AddRange(buffer);
				buffer.Clear();
			}

			return -1;
		}

		private static void GetExpeditionMoves(ExpeditionPath expedition, List<Blizzard> blizzards, Vector2 exit, int w, int h)
		{
			List<Vector2> open = new List<Vector2>();
			if (!blizzards.Any(b => b.pos == expedition.pos + new Vector2( 1,  0))) open.Add(expedition.pos + new Vector2(1, 0));
			if (!blizzards.Any(b => b.pos == expedition.pos + new Vector2(-1,  0))) open.Add(expedition.pos + new Vector2(-1, 0));
			if (!blizzards.Any(b => b.pos == expedition.pos + new Vector2( 0,  1))) open.Add(expedition.pos + new Vector2(0, 1));
			if (!blizzards.Any(b => b.pos == expedition.pos + new Vector2( 0, -1))) open.Add(expedition.pos + new Vector2(0, -1));
			
			open.RemoveAll(p => (p.x <= 0 || p.y <= 0 || p.x >= w || p.y >= h) && p != exit);

			if (!blizzards.Any(b => b.pos == expedition.pos)) open.Add(expedition.pos);

			/*if(blizzards.Any(b => b.pos == expedition.pos))
			{
				var m = blizzards.First(b => b.pos == expedition.pos);

				for (int y = 0; y <= h; y++)
				{
					for (int x = 0; x <= w; x++)
					{
						var o = new Vector2(x, y);
						Console.Write(blizzards.Count(x => x.pos == o));
					}
					Console.Write('\n');
				}

			}*/

			foreach (Vector2 p in open)
			{
				expedition.next.Add(new ExpeditionPath()
				{
					pos = p
				});
			}
		}

		private static void UpdateBlizzards(List<Blizzard> blizzards, int w, int h)
		{
			foreach(Blizzard bl in blizzards)
			{
				bl.pos += bl.dir;
				if (bl.pos.x == w || bl.pos.x == 0)
					bl.pos = new Vector2(w - bl.pos.x + bl.dir.x, bl.pos.y);
				if (bl.pos.y == h || bl.pos.y == 0)
					bl.pos = new Vector2(bl.pos.x, h - bl.pos.y + bl.dir.y);
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<Blizzard> blizzards = new List<Blizzard>();
			ExpeditionPath expedition = new ExpeditionPath();
			Vector2 exit = expedition.pos = new Vector2(-1, -1);
			Vector2 start = exit;
			for (int y = 0; y < lines.Length; y++)
			{
				for (int x = 0; x < lines[y].Length; x++)
				{
					if (lines[y][x] == '.' && expedition.pos.y == -1)
						expedition.pos = new Vector2(x, 0);
					else if (y == lines.Length - 1 && lines[y][x] == '.' && exit.y == -1)
						exit = new Vector2(x, y);
					if (lines[y][x] == '<')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(-1, 0)
						});
					}
					if (lines[y][x] == '>')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(1, 0)
						});
					}
					if (lines[y][x] == '^')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(0, -1)
						});
					}
					if (lines[y][x] == 'v')
					{
						blizzards.Add(new Blizzard()
						{
							pos = new Vector2(x, y),
							dir = new Vector2(0, 1)
						});
					}
				}
			}
			start = expedition.pos;
			int w = lines[0].Length;
			int h = lines.Length;

			List<ExpeditionPath> open = new List<ExpeditionPath>();
			List<ExpeditionPath> buffer = new List<ExpeditionPath>();
			open.Add(expedition);
			while (open.Count > 0)
			{
				sum++;
				UpdateBlizzards(blizzards, w - 1, h - 1);
				foreach (ExpeditionPath currExp in open)
				{
					GetExpeditionMoves(currExp, blizzards, exit, w - 1, h - 1);
					foreach (ExpeditionPath exp in currExp.next)
					{
						if (exp.pos == exit) goto outerbreak;
						if (!buffer.Contains(exp))
							buffer.Add(exp);
					}
				}
				open.Clear();
				open.AddRange(buffer);
				buffer.Clear();
			}
		outerbreak:
			open.Clear();
			buffer.Clear();
			open.Add(new ExpeditionPath() { pos=exit });
			while (open.Count > 0)
			{
				sum++;
				UpdateBlizzards(blizzards, w - 1, h - 1);
				foreach (ExpeditionPath currExp in open)
				{
					GetExpeditionMoves(currExp, blizzards, start, w - 1, h - 1);
					foreach (ExpeditionPath exp in currExp.next)
					{
						if (exp.pos == start) goto outerbreak2;
						if (!buffer.Contains(exp))
							buffer.Add(exp);
					}
				}
				open.Clear();
				open.AddRange(buffer);
				buffer.Clear();
			}
		outerbreak2:
			open.Clear();
			buffer.Clear();
			open.Add(new ExpeditionPath() { pos = start });
			while (open.Count > 0)
			{
				sum++;
				UpdateBlizzards(blizzards, w - 1, h - 1);
				foreach (ExpeditionPath currExp in open)
				{
					GetExpeditionMoves(currExp, blizzards, exit, w - 1, h - 1);
					foreach (ExpeditionPath exp in currExp.next)
					{
						if (exp.pos == exit) return sum;
						if (!buffer.Contains(exp))
							buffer.Add(exp);
					}
				}
				open.Clear();
				open.AddRange(buffer);
				buffer.Clear();
			}

			return -1;
		}
	}
}