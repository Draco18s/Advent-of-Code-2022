using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2021 {
	internal static class DayTwo {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<(RPS opponent, RPS play)> list = new List<(RPS, RPS)>();
			foreach(string lin in lines) {
				if (string.IsNullOrWhiteSpace(lin)) continue;
				RPS op = (RPS)(lin[0] - 'A' + 1);
				RPS me = (RPS)(lin[2] - 'X' + 1);
				list.Add((op, me));
			}
			foreach(var g in list)
			{
				sum += (int)g.play;
				sum += ResolveP1(g);
			}
			return sum;
		}

		private static int ResolveP1((RPS opponent, RPS play) g)
		{
			if (g.opponent == g.play) return 3;
			if (g.opponent == g.play+1 || (g.opponent == RPS.ROCK && g.play == RPS.SCISSORS)) return 0;
			return 6;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<(RPS opponent, RPS play)> list = new List<(RPS, RPS)>();
			foreach (string lin in lines)
			{
				if (string.IsNullOrWhiteSpace(lin)) continue;
				RPS op = (RPS)(lin[0] - 'A' + 1);
				RPS me = (RPS)(lin[2] - 'X' + 1);
				list.Add((op, me));
			}
			foreach (var g in list)
			{
				sum += GetScore(g.play);
				sum += ResolveP2(g);
			}
			return sum;
		}

		private static int GetScore(RPS play)
		{
			switch(play)
			{
				case RPS.ROCK: return 0; //loss
				case RPS.PAPER: return 3; //tie
				case RPS.SCISSORS: return 6; //win
			}
			return 0;
		}

		private static int ResolveP2((RPS opponent, RPS play) g)
		{
			int v = 0;
			if (g.play == RPS.PAPER) //end in tie
			{
				return (int)g.opponent;
			}
			if (g.play == RPS.SCISSORS) //end in win
			{
				v = ((int)g.opponent + 1);
				if (v == 4) v = 1;
				return v;
			}
			v = ((int)g.opponent - 1);
			if (v == 0) v = 3;
			return v;
		}

		private enum RPS
		{
			NONE,//0
			ROCK,//1
			PAPER,//2
			SCISSORS,//3
		}
	}
}