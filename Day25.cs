using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventofCode2022 {
	internal static class DayTwentyfive {
		public struct SnafuNumber
		{
			char[] digits;

			public SnafuNumber(string value)
			{
				if (string.IsNullOrEmpty(value)) value = "0";
				digits = new char[value.Length];
				for(int x=0; x<value.Length;x++)
				{
					//radix == array index.
					//so 1s digit is at index 0
					//5s digit is at index 1
					//25s digit is at index 2
					//and so on
					//this makes adding two differently lengthed values easier
					//as they are both *right* aligned instead of *left*
					digits[value.Length - 1 - x] = (value[x]);
				}
			}

			public static SnafuNumber operator +(SnafuNumber a, SnafuNumber b)
			{
				string s = "";
				int carry = 0;
				for (int x = 0; true; x++)
				{
					char a0;
					if (x < a.digits.Length) a0 = a.digits[x];
					else a0 = ('0');
					char b0;
					if (x < b.digits.Length) b0 = b.digits[x];
					else b0 = ('0');

					int aa = "=-012".IndexOf(a0) - 2;
					int bb = "=-012".IndexOf(b0) - 2;
					int cc = aa + bb + carry;
					carry = 0;
					if (cc > 2)
					{
						cc -= 5;
						carry = 1;
					}
					else if (cc < -2)
					{
						cc += 5;
						carry = -1;
					}
					char dd = "=-012"[cc+2];
					if (x >= a.digits.Length && x >= b.digits.Length && carry == 0) break;
					s = dd + s;
				}
				return new SnafuNumber(s);
			}

			public override string ToString()
			{
				StringBuilder sb = new StringBuilder();
				foreach(char d in digits)
				{
					sb.Insert(0,d);
				}
				return sb.ToString();
			}
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			SnafuNumber answer = new SnafuNumber("0");
			List<int> carry = new List<int>();
			foreach(string lin in lines) {
				SnafuNumber sn = new SnafuNumber(lin);
				Console.WriteLine($"{answer} + {sn} => {answer + sn}");
				answer = answer + sn;
			}
			Console.WriteLine(answer.ToString());
			return -1;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<int> carry = new List<int>();
			foreach (string lin in lines)
			{
				
			}
			
			return -1;
		}
	}
}