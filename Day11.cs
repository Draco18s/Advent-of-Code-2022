using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022
{
	internal static class DayEleven
	{
		static long MODULO = 1;

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long sum = 0;
			List<Monkey> monkies = new List<Monkey>();
			Monkey lastMonkey = null;
			foreach (string lin in lines)
			{
				if (string.IsNullOrEmpty(lin)) continue;
				if (lin.Contains("Monkey"))
				{
					string[] p = lin.Split(' ');
					lastMonkey = new Monkey(p[1]);
					monkies.Add(lastMonkey);
				}
				else if (lin.Contains("Starting items"))
				{
					string[] parts = lin.Split(':')[1].Split(',');
					foreach (string p in parts)
					{
						lastMonkey.AddItem(int.Parse(p));
					}
				}
				else if (lin.Contains("Operation"))
				{
					lastMonkey.operation = lin.Split(':')[1];
				}
				else if (lin.Contains("Test"))
				{
					lastMonkey.test = lin.Split(':')[1];

					string[] qq = lastMonkey.test.Split(' ');
					lastMonkey.prime = int.Parse(qq[3]);
				}
				else if (lin.Contains("true"))
				{
					lastMonkey.ifTrue = lin.Split(':')[1];
				}
				else if (lin.Contains("false"))
				{
					lastMonkey.ifFalse = lin.Split(':')[1];
				}
			}
			sum = DoMonkeyLoops(monkies, 20);
			return sum;
		}

		private static long DoMonkeyLoops(List<Monkey> monkies, int v)
		{
			MODULO = 1;
			foreach (Monkey m in monkies)
			{
				MODULO *= m.prime;
			}
			for (int i = 0; i < v; i++)
			{
				foreach (Monkey m in monkies)
				{
					m.TakeTurn(monkies);
				}
			}
			var mk = monkies.OrderByDescending(m => m.inspections).ToArray();
			return mk[0].inspections * mk[1].inspections;
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			long sum = 0;
			List<Monkey> monkies = new List<Monkey>();
			Monkey lastMonkey = null;
			foreach (string lin in lines)
			{
				if (string.IsNullOrEmpty(lin)) continue;
				if (lin.Contains("Monkey"))
				{
					string[] p = lin.Split(' ');
					lastMonkey = new Monkey(p[1]);
					monkies.Add(lastMonkey);
				}
				else if (lin.Contains("Starting items"))
				{
					string[] parts = lin.Split(':')[1].Split(',');
					foreach (string p in parts)
					{
						lastMonkey.AddItem(int.Parse(p));
					}
				}
				else if (lin.Contains("Operation"))
				{
					lastMonkey.operation = lin.Split(':')[1];
				}
				else if (lin.Contains("Test"))
				{
					lastMonkey.test = lin.Split(':')[1];

					string[] qq = lastMonkey.test.Split(' ');
					lastMonkey.prime = int.Parse(qq[3]);
				}
				else if (lin.Contains("true"))
				{
					lastMonkey.ifTrue = lin.Split(':')[1];
				}
				else if (lin.Contains("false"))
				{
					lastMonkey.ifFalse = lin.Split(':')[1];
				}
			}
			sum = DoMonkeyLoops2(monkies, 10000);
			return sum;
		}

		private static long DoMonkeyLoops2(List<Monkey> monkies, int v)
		{
			MODULO = 1;
			foreach (Monkey m in monkies)
			{
				MODULO *= m.prime;
			}
			for (int i = 0; i < v; i++)
			{
				foreach (Monkey m in monkies)
				{
					m.TakeTurn2(monkies);
				}
			}
			var mk = monkies.OrderByDescending(m => m.inspections).ToArray();
			return mk[0].inspections * mk[1].inspections;
		}

		private class Monkey
		{
			public int prime;
			public readonly int ID;
			public readonly List<long> items;
			public string operation;
			public string test;
			public string ifTrue;
			public string ifFalse;
			public long inspections = 0;

			public override string ToString()
			{
				return inspections.ToString();
			}

			public Monkey(string id)
			{
				ID = int.Parse(id.Substring(0, id.Length - 1));
				items = new List<long>();
			}

			internal void AddItem(long v)
			{
				items.Add(v);
			}

			internal void TakeTurn(List<Monkey> monkies)
			{
				List<long> toInspect = new List<long>();
				toInspect.AddRange(items);
				items.Clear();
				foreach (int i in toInspect)
				{
					inspections++;
					long j = DoOp(i);
					if (DoTest(j))
					{
						string[] parts = ifTrue.Split(' ');
						int M = int.Parse(parts[parts.Length - 1]);
						monkies[M].AddItem(j);
					}
					else
					{
						string[] parts = ifFalse.Split(' ');
						int M = int.Parse(parts[parts.Length - 1]);
						monkies[M].AddItem(j);
					}
				}
			}

			private bool DoTest(long j)
			{
				string[] parts = test.Split(' ');
				int T = int.Parse(parts[parts.Length - 1]);
				return j % T == 0;
			}

			private long DoOp(long i)
			{
				string[] opList = operation.Split(' ');
				Stack<long> shunt = new Stack<long>();
				string math = "";
				foreach (string op in opList)
				{
					if (op.Contains("=") || op.Contains("new") || string.IsNullOrEmpty(op)) continue;
					if (op.Contains("old"))
					{
						shunt.Push(i);
					}
					else if (op.Contains("+"))
					{
						math = "+";
					}
					else if (op.Contains("-"))
					{
						math = "-";
					}
					else if (op.Contains("*"))
					{
						math = "*";
					}
					else if (op.Contains("/"))
					{
						math = "/";
					}
					else
					{
						shunt.Push(int.Parse(op));
					}
				}
				switch (math[0])
				{
					case '+':
						return (shunt.Pop() + shunt.Pop()) / 3;
					case '-':
						return (shunt.Pop() - shunt.Pop()) / 3;
					case '*':
						return shunt.Pop() * shunt.Pop() / 3;
					case '/':
						return shunt.Pop() / shunt.Pop() / 3;
				}
				return 0;
			}

			internal void TakeTurn2(List<Monkey> monkies)
			{
				List<long> toInspect = new List<long>();
				toInspect.AddRange(items);
				items.Clear();
				foreach (int i in toInspect)
				{
					inspections++;
					long j = DoOp2(i);
					if (DoTest(j))
					{
						string[] parts = ifTrue.Split(' ');
						int M = int.Parse(parts[parts.Length - 1]);
						monkies[M].AddItem(j);
					}
					else
					{
						string[] parts = ifFalse.Split(' ');
						int M = int.Parse(parts[parts.Length - 1]);
						monkies[M].AddItem(j);
					}
				}
			}

			private long DoOp2(long i)
			{
				string[] opList = operation.Split(' ');
				Stack<long> shunt = new Stack<long>();
				string math = "";
				foreach (string op in opList)
				{
					if (op.Contains("=") || op.Contains("new") || string.IsNullOrEmpty(op)) continue;
					if (op.Contains("old"))
					{
						shunt.Push(i);
					}
					else if (op.Contains("+"))
					{
						math = "+";
					}
					else if (op.Contains("-"))
					{
						math = "-";
					}
					else if (op.Contains("*"))
					{
						math = "*";
					}
					else if (op.Contains("/"))
					{
						math = "/";
					}
					else
					{
						shunt.Push(int.Parse(op));
					}
				}
				switch (math[0])
				{
					case '+':
						return (shunt.Pop() + shunt.Pop()) % MODULO;
					case '-':
						return (shunt.Pop() - shunt.Pop()) % MODULO;
					case '*':
						return shunt.Pop() * shunt.Pop() % MODULO;
					case '/':
						return shunt.Pop() / shunt.Pop() % MODULO;
				}
				return 0;
			}
		}
	}
}