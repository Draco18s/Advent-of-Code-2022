using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayFive {

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<string> crates = new List<string>();
			List<string> instructions = new List<string>();

			List<Stack<char>> crateStacks = new List<Stack<char>>();
			bool stacks = true;
			foreach (string lin in lines) {
				if(string.IsNullOrWhiteSpace(lin)) {
					stacks = false;
					continue;
				}
				if(stacks)
					crates.Add(lin);
				else
					instructions.Add(lin);
			}
			ParseCrateStack(crates, ref crateStacks);
			ProcessMoves1(ref crateStacks, instructions);
			string res = "";
			foreach(Stack<char> stack in crateStacks)
			{
				res += stack.Pop();
			}
			Console.WriteLine(res);
			return sum;
		}

		private static void ProcessMoves1(ref List<Stack<char>> crateStacks, List<string> instructions)
		{
			foreach (string inst in instructions)
			{
				string[] parts = inst.Split(' ');
				int numToMove = int.Parse(parts[1]);
				int fromStack = int.Parse(parts[3]) - 1;
				int toStack = int.Parse(parts[5]) - 1;

				for(int i = 0; i < numToMove; i++)
				{
					char C = crateStacks[fromStack].Pop();
					crateStacks[toStack].Push(C);
				}
			}
		}

		private static void ProcessMoves2(ref List<Stack<char>> crateStacks, List<string> instructions)
		{
			Stack<char> buffer = new Stack<char>();
			foreach (string inst in instructions)
			{
				buffer.Clear();
				string[] parts = inst.Split(' ');
				int numToMove = int.Parse(parts[1]);
				int fromStack = int.Parse(parts[3]) - 1;
				int toStack = int.Parse(parts[5]) - 1;

				for (int i = 0; i < numToMove; i++)
				{
					if (crateStacks[fromStack].Count == 0) break;
					buffer.Push(crateStacks[fromStack].Pop());
				}
				foreach (char c in buffer)
				{
					crateStacks[toStack].Push(c);
				}
			}
		}

		private static void ParseCrateStack(List<string> crates, ref List<Stack<char>> crateStacks)
		{
			List<string> transposed = new List<string>();
			for(int i = 0; i < crates.Max(x => x.Length); i++)
			{
				string temp = "";
				foreach(string stc in crates)
				{
					if(i < stc.Length)
					{
						temp += stc[i];
					}
				}
				transposed.Add(temp);
			}
			foreach(string t in transposed)
			{
				if (string.IsNullOrWhiteSpace(t)) continue;
				if (t.Contains('[') || t.Contains(']')) continue;
				char[] chars = t.ToCharArray().Reverse().ToArray();
				Stack<char> stk = new Stack<char>();
				crateStacks.Add(stk);
				foreach (char c in chars)
				{
					if (c >= 'A' && c <= 'Z')
						stk.Push(c);
				}
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<string> crates = new List<string>();
			List<string> instructions = new List<string>();

			List<Stack<char>> crateStacks = new List<Stack<char>>();
			bool stacks = true;
			foreach (string lin in lines)
			{
				if (string.IsNullOrWhiteSpace(lin))
				{
					stacks = false;
					continue;
				}
				if (stacks)
					crates.Add(lin);
				else
					instructions.Add(lin);
			}
			ParseCrateStack(crates, ref crateStacks);
			ProcessMoves2(ref crateStacks, instructions);
			string res = "";
			foreach (Stack<char> stack in crateStacks)
			{
				res += stack.Pop();
			}
			Console.WriteLine(res);
			return sum;
		}
	}
}