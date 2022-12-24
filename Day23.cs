using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayTwentythree {
		public class Elf
		{
			public Vector2 pos;
			public Vector2 proposedMove;
		}
		enum Direction { North, South, West, East }
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Grid ground = new Grid(input, true);
			List<Elf> elves = new List<Elf>();
			Direction nextConsider = Direction.North;
			for (int y = ground.MinY; y < ground.MaxY; y++)
			{
				for (int x = ground.MinX; x < ground.MaxX; x++)
				{
					if(ground[x,y] == '#')
					{
						elves.Add(new Elf()
						{
							pos = new Vector2(x, y)
						});
					}
				}
			}

			for(int round = 0; round < 10; round++)
			{
				//Console.WriteLine(ground.ToString("char+0"));
				Grid proposal = new Grid(ground.Width, ground.Height, ground.MinX, ground.MinY);
				ProposeMoves(ground, elves, proposal, nextConsider);
				ExecuteMoves(ground, elves, proposal, nextConsider);
				nextConsider = (Direction)(((int)nextConsider + 1) % 4);
				//Console.WriteLine(nextConsider);
			}

			ground.TrimGrid(()=>'.');
			//Console.WriteLine(ground.ToString("char+0"));
			return (ground.Width+2) * (ground.Height+2) - elves.Count;
		}

		private static bool ExecuteMoves(Grid ground, List<Elf> elves, Grid proposal, Direction nextConsider)
		{
			bool anyMoves = false;
			foreach (Elf elf in elves)
			{
				if (proposal[elf.proposedMove] == 1)
				{
					//if(elf.pos != elf.proposedMove)
						anyMoves = true;
					ground[elf.pos] = '.';
					ground.IncreaseGridToInclude(elf.proposedMove, () => '.');
					elf.pos = elf.proposedMove;
					ground[elf.pos] = '#';
				}
			}
			return anyMoves;
		}

		private static void ProposeMoves(Grid ground, List<Elf> elves, Grid proposal, Direction nextConsider)
		{
			foreach(Elf elf in elves)
			{
				int otherElves = 0;
				for (int x = -1; x <= 1; x++)
					for (int y = -1; y <= 1; y++)
						otherElves += ground[elf.pos + new Vector2(x, y), true, () => '.'] == '#' ? 1 : 0;

				if (otherElves <= 1)
				{
					elf.proposedMove = elf.pos;
					continue;
				}
				for (Direction dir = nextConsider; dir < nextConsider + 4; dir++)
					if (Consider(elf, dir, ground)) break;
				proposal.IncreaseGridToInclude(elf.proposedMove, () => 0);
				proposal[elf.proposedMove]++;
			}
		}

		private static bool Consider(Elf elf, Direction dir, Grid ground)
		{
			switch ((Direction)((int)dir % 4))
			{
				case Direction.North:
					if (ground[elf.pos + new Vector2(0, -1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(-1, -1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(1, -1), true, () => '.'] == '.')
					{
						elf.proposedMove = elf.pos + new Vector2(0, -1);
						return true;
					}
					break;
				case Direction.South:
					if (ground[elf.pos + new Vector2(0, 1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(-1, 1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(1, 1), true, () => '.'] == '.')
					{
						elf.proposedMove = elf.pos + new Vector2(0, 1);
						return true;
					}
					break;
				case Direction.West:
					if (ground[elf.pos + new Vector2(-1, 1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(-1, 0), true, () => '.'] == '.' && ground[elf.pos + new Vector2(-1, -1), true, () => '.'] == '.')
					{
						elf.proposedMove = elf.pos + new Vector2(-1, 0);
						return true;
					}
					break;
				case Direction.East:
					if (ground[elf.pos + new Vector2(1, 1), true, () => '.'] == '.' && ground[elf.pos + new Vector2(1, 0), true, () => '.'] == '.' && ground[elf.pos + new Vector2(1, -1), true, () => '.'] == '.')
					{
						elf.proposedMove = elf.pos + new Vector2(1, 0);
						return true;
					}
					break;
			}
			return false;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Grid ground = new Grid(input, true);
			List<Elf> elves = new List<Elf>();
			Direction nextConsider = Direction.North;
			for (int y = ground.MinY; y < ground.MaxY; y++)
			{
				for (int x = ground.MinX; x < ground.MaxX; x++)
				{
					if (ground[x, y] == '#')
					{
						elves.Add(new Elf()
						{
							pos = new Vector2(x, y)
						});
					}
				}
			}
			
			while(true)
			{
				sum++;
				//Console.WriteLine(ground.ToString("char+0"));
				Grid proposal = new Grid(ground.Width, ground.Height, ground.MinX, ground.MinY);
				ProposeMoves(ground, elves, proposal, nextConsider);
				bool r = ExecuteMoves(ground, elves, proposal, nextConsider);
				if (!r) break;
				nextConsider = (Direction)(((int)nextConsider + 1) % 4);
				//Console.WriteLine(nextConsider);
			}
			return sum;
		}
	}
}