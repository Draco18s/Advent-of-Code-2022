using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayThirteen {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			for(int i=0; i < lines.Length; i+=3)
			{
				string lin1 = lines[i + 0];
				string lin2 = lines[i + 1];
				ArrayNode listy1 = ParseArray(lin1);
				ArrayNode listy2 = ParseArray(lin2);
				tri b = Validate(listy1, listy2);
				if(b == tri.True)
				{
					int a = (i + 3) / 3;
					sum += a;
				}
			}
			
			return sum;
		}

		enum tri
		{
			False,True,Continue
		}

		private static tri Validate(ArrayNode left, ArrayNode right)
		{
			if(left.array == null && right.array == null)
			{
				if (left.value == right.value) return tri.Continue;
				if (left.value > right.value) return tri.False;
				return tri.True;
			}
			if(left.array != null && right.array != null)
			{
				for(int j=0; j < left.array.Count; j++)
				{
					bool b = j < right.array.Count;
					if (!b) return tri.False;
					tri t = Validate(left.array[j], right.array[j]);
					if (t == tri.Continue) continue;
					if (t == tri.True) return tri.True;
					if (t == tri.False) return tri.False;
				}
				return left.array.Count == right.array.Count ? tri.Continue : (left.array.Count < right.array.Count ? tri.True : tri.False);
			}
			if(left.array == null)
			{
				left.array = new List<ArrayNode>();
				left.array.Add(new ArrayNode() {
					value = left.value
				});
				return Validate(left, right);
				//if (t == tri.False) return tri.False;
			}
			if (right.array == null)
			{
				right.array = new List<ArrayNode>();
				right.array.Add(new ArrayNode()
				{
					value = right.value
				});
				return Validate(left, right);
			}
			return tri.True;
		}

		struct ArrayNode
		{
			public int value;
			public List<ArrayNode> array;

			public ArrayNode(int v)
			{
				value = v;
				array = new List<ArrayNode>();
			}

			public override string ToString()
			{
				if (array == null) return value.ToString();
				return $"[{string.Join(',', array)}]";
			}
		}

		static ArrayNode ParseArray(string str)
		{
			if (string.IsNullOrEmpty(str)) return new ArrayNode();
			var result = new List<ArrayNode>();
			var output = new List<int>();
			var trainyard = new Stack<char>();
			var value = -1;
			for (var i = 0; i < str.Length; i++)
			{
				var exp = str[i];
				if (exp == ' ') continue;
				if (exp >= '0' && exp <= '9')
				{
					if (value == -1) value = 0;
					value = value * 10 + int.Parse(exp.ToString());
					continue;
				}
				else if (value != -1)
				{
					output.Add(value);
					value = -1;
				}
				if (exp == ',') continue;
				switch (exp)
				{
					case '-':
					case '[':
						output.Add(exp);
						break;
					case ']':
						while (trainyard.Count > 0)
						{
							var c = trainyard.Pop();
							if (c == '[') break;
							output.Add(c);
						}
						output.Add(exp);
						break;
					default:
						if (!(exp >= '0' && exp <= '9'))
						{
							//
						}
						break;
				}
			}
			if (value != -1)
			{
				output.Add(value);
				value = -1;
			}
			while (trainyard.Count > 0)
			{
				output.Add(trainyard.Pop());
			}
			var arrayStack = new Stack<ArrayNode>();
			ArrayNode curArray = new ArrayNode(int.MinValue);
			while (output.Count > 0)
			{
				var exp = output[0];
				output.RemoveAt(0);
				switch (exp)
				{
					case '-':
						var v = -output[0];
						output.RemoveAt(0);
						curArray.value = v;
						break;
					case '[':
						var n = new ArrayNode(int.MinValue);
						curArray.array.Add(n);
						arrayStack.Push(curArray);
						curArray = n;
						break;
					case ']':
						var t = arrayStack.Pop();
						//t.array.Add(curArray);
						curArray = t;
						break;
					default:
						curArray.array.Add(new ArrayNode()
						{
							value = exp,
							array = null
						});
						break;
				}
			}
			return curArray.array[0];
		}

		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			List<ArrayNode> allPackets = new List<ArrayNode>();
			foreach (string lin in lines)
			{
				if (string.IsNullOrEmpty(lin)) continue;
				ArrayNode listy = ParseArray(lin);
				allPackets.Add(listy);
			}
			var two = ParseArray("[[2]]");
			var six = ParseArray("[[6]]");
			allPackets.Add(two);
			allPackets.Add(six);

			allPackets.Sort((a, b) =>
			{
				var ordered = Validate(a, b);
				return ordered == tri.True ? -1 : ordered == tri.Continue ? 0 : 1;
			});

			var aa = Validate(allPackets[0], allPackets[1]);

			return (allPackets.IndexOf(two)+1) * (allPackets.IndexOf(six)+1);
		}
	}
}