using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventofCode2022 {
	internal static class DayTwentyone
	{
		internal static long Part1(string input) {
			Dictionary<string, long> variables = new Dictionary<string, long>();
			Regex flatVal = new Regex("([a-z]+): (\\d+)");
			Regex math = new Regex("([a-z]+): ([a-z0-0]+) ([-+*/]) ([a-z0-0]+)");
			string[] lines = input.Split('\n');
			foreach (string lin in lines)
			{
				if (flatVal.IsMatch(lin))
				{
					Match m = flatVal.Match(lin);
					variables[m.Groups[1].Value] = int.Parse(m.Groups[2].Value);
				}
			}
			List<string> remaining = new List<string>();
			remaining.AddRange(lines.Where(x => !flatVal.IsMatch(x)));
			lines = remaining.ToArray();
			do
			{
				remaining.Clear();
				foreach (string lin in lines)
				{
					Match m = math.Match(lin);
					if (variables.ContainsKey(m.Groups[2].Value) && variables.ContainsKey(m.Groups[4].Value))
					{
						long a = int.MinValue;
						switch (m.Groups[3].Value)
						{
							case "+":
								a = variables[m.Groups[2].Value] + variables[m.Groups[4].Value];
								break;
							case "-":
								a = variables[m.Groups[2].Value] - variables[m.Groups[4].Value];
								break;
							case "*":
								a = variables[m.Groups[2].Value] * variables[m.Groups[4].Value];
								break;
							case "/":
								a = variables[m.Groups[2].Value] / variables[m.Groups[4].Value];
								break;

						}
						variables[m.Groups[1].Value] = a;
						if (m.Groups[1].Value == "root")
						{
							return a;
						}
						remaining.Add(lin);
					}
					lines = lines.Where(x => !remaining.Contains(x)).ToArray();
				}
			} while (true);
		}

		internal static long Part2(string input) {
			Dictionary<string, long> variables = new Dictionary<string, long>();
			Regex flatVal = new Regex("([a-z]+): (\\d+)");
			Regex math = new Regex("([a-z]+): ([a-z0-0]+) ([-+*/]) ([a-z0-0]+)");
			string[] lines = input.Split('\n');
			foreach (string lin in lines)
			{
				if (flatVal.IsMatch(lin))
				{
					Match m = flatVal.Match(lin);
					variables[m.Groups[1].Value] = int.Parse(m.Groups[2].Value);
				}
			}
			List<string> remaining = new List<string>();
			remaining.AddRange(lines.Where(x => !flatVal.IsMatch(x)));
			lines = remaining.ToArray();
			variables.Remove("humn");
			do
			{
				remaining.Clear();
				foreach (string lin in lines)
				{
					Match m = math.Match(lin);
					if (variables.ContainsKey(m.Groups[2].Value) && variables.ContainsKey(m.Groups[4].Value))
					{
						long a = int.MinValue;
						switch (m.Groups[3].Value)
						{
							case "+":
								a = variables[m.Groups[2].Value] + variables[m.Groups[4].Value];
								break;
							case "-":
								a = variables[m.Groups[2].Value] - variables[m.Groups[4].Value];
								break;
							case "*":
								a = variables[m.Groups[2].Value] * variables[m.Groups[4].Value];
								break;
							case "/":
								a = variables[m.Groups[2].Value] / variables[m.Groups[4].Value];
								break;

						}
						variables[m.Groups[1].Value] = a;
						if (m.Groups[1].Value == "root")
						{
							return a;
						}
						remaining.Add(lin);
					}
					else if(m.Groups[1].Value == "root" && (variables.ContainsKey(m.Groups[2].Value) || variables.ContainsKey(m.Groups[4].Value))) {
						remaining.Add("root");
						if (variables.ContainsKey(m.Groups[2].Value))
							variables[m.Groups[4].Value] = variables[m.Groups[2].Value];
						else
							variables[m.Groups[2].Value] = variables[m.Groups[4].Value];
						lines = lines.Where(x => !remaining.Contains(x)).ToArray();
						return ReverseSolve(lines, variables);
					}

					lines = lines.Where(x => !remaining.Contains(x)).ToArray();
				}
			} while (true);
		}

		private static long ReverseSolve(string[] lines, Dictionary<string, long> variables)
		{                       //		1		   2		  3			4
			List<string> remaining = new List<string>();
			List<string> toAdd = new List<string>();
			Regex math = new Regex("([a-z]+): ([a-z0-0]+) ([-+*/]) ([a-z0-0]+)");
			do {
				foreach (string lin in lines)
				{
					Match m = math.Match(lin);
					if (variables.ContainsKey(m.Groups[2].Value) && variables.ContainsKey(m.Groups[4].Value))
					{
						long a = int.MinValue;
						switch (m.Groups[3].Value)
						{
							case "+":
								a = variables[m.Groups[2].Value] + variables[m.Groups[4].Value];
								break;
							case "-":
								a = variables[m.Groups[2].Value] - variables[m.Groups[4].Value];
								break;
							case "*":
								a = variables[m.Groups[2].Value] * variables[m.Groups[4].Value];
								break;
							case "/":
								a = variables[m.Groups[2].Value] / variables[m.Groups[4].Value];
								break;

						}
						variables[m.Groups[1].Value] = a;
						if (m.Groups[1].Value == "humn")
						{
							return a;
						}
						remaining.Add(lin);
					}
					else if (variables.ContainsKey(m.Groups[1].Value))
					{
						if (variables.ContainsKey(m.Groups[2].Value))
						{   //a = B + c
							//a - B = c
							if (m.Groups[3].Value == "+")
							{
								toAdd.Add($"{m.Groups[4].Value}: {m.Groups[1].Value} - {m.Groups[2].Value}");
							}
							else if (m.Groups[3].Value == "-")
							{
								toAdd.Add($"{m.Groups[4].Value}: {m.Groups[2].Value} - {m.Groups[1].Value}");
							}
							else if (m.Groups[3].Value == "*")
							{
								toAdd.Add($"{m.Groups[4].Value}: {m.Groups[1].Value} / {m.Groups[2].Value}");
							}
							else if (m.Groups[3].Value == "/")
							{
								toAdd.Add($"{m.Groups[4].Value}: {m.Groups[2].Value} / {m.Groups[1].Value}");
							}
						}
						else if (variables.ContainsKey(m.Groups[4].Value))
						{   //a - C = b
							//b = a - C
							if (m.Groups[3].Value == "+")
							{
								toAdd.Add($"{m.Groups[2].Value}: {m.Groups[1].Value} - {m.Groups[4].Value}");
							}
							else if (m.Groups[3].Value == "-")
							{
								toAdd.Add($"{m.Groups[2].Value}: {m.Groups[4].Value} + {m.Groups[1].Value}");
							}
							else if (m.Groups[3].Value == "*")
							{
								toAdd.Add($"{m.Groups[2].Value}: {m.Groups[1].Value} / {m.Groups[4].Value}");
							}
							else if (m.Groups[3].Value == "/")
							{
								toAdd.Add($"{m.Groups[2].Value}: {m.Groups[1].Value} * {m.Groups[4].Value}");
							}
						}
					}
				}
				lines = lines.Where(x => !remaining.Contains(x)).Concat(toAdd).ToArray();
				remaining.Clear();
				toAdd.Clear();
			} while (true);
		}
	}
}