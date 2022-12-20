using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayTwenty {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<int> carry = new List<int>();
			foreach(string lin in lines) {
				int l = int.Parse(lin);
				carry.Add(l);
			}
			List<(int index,int value)> pairs = new List<(int index, int value)>();
			int ind = 0;
			(int index, int value) origin = (0,0);
			foreach(int i in carry)
			{
				if (i == 0) origin = (ind, i);
				pairs.Add((ind, i));
				ind++;
			}
			RotationalQueue<(int index, int value)> queue = new RotationalQueue<(int index, int value)>(pairs);
			Mix(ref queue, pairs);
			int indZero = queue.IndexOf(origin);
			queue.RotateLeft(indZero);
			queue.RotateLeft(1000);
			(int index, int value) a = queue.Peek();
			queue.RotateLeft(1000);
			(int index, int value) b = queue.Peek();
			queue.RotateLeft(1000);
			(int index, int value) c = queue.Peek();
			return a.value + b.value + c.value;
		}

		private static void Mix(ref RotationalQueue<(int index, int value)> queue, List<(int index, int value)> order)
		{
			foreach((int,int) i in order)
			{
				int f = queue.IndexOf(i);
				queue.RotateLeft(f);
				(int index, int value) j = queue.Dequeue();
				if (j.value > 0)
				{
					queue.RotateLeft(j.value);
				}
				else if (j.value < 0)
				{
					queue.RotateRight(-j.value);
				}
				queue.Enqueue(j);
			}
		}

		private static void MixLong(ref RotationalQueue<(long index, long value)> queue, List<(long index, long value)> order)
		{
			for (int k = 0; k < 10; k++)
			{
				foreach ((long, long) i in order)
				{
					long f = queue.IndexOf(i);
					queue.RotateLeft(f);
					(long index, long value) j = queue.Dequeue();
					if (j.value > 0)
					{
						queue.RotateLeft(j.value);
					}
					else if (j.value < 0)
					{
						queue.RotateRight(-j.value);
					}
					queue.Enqueue(j);
				}
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<long> carry = new List<long>();
			foreach (string lin in lines)
			{
				int l = int.Parse(lin);
				carry.Add(l* 811589153L);
			}
			List<(long index, long value)> pairs = new List<(long index, long value)>();
			int ind = 0;
			(long index, long value) origin = (0, 0);
			foreach (long i in carry)
			{
				if (i == 0) origin = (ind, i);
				pairs.Add((ind, i));
				ind++;
			}
			RotationalQueue<(long index, long value)> queue = new RotationalQueue<(long index, long value)>(pairs);
			MixLong(ref queue, pairs);
			int indZero = queue.IndexOf(origin);
			queue.RotateLeft(indZero);
			queue.RotateLeft(1000);
			(long index, long value) a = queue.Peek();
			queue.RotateLeft(1000);
			(long index, long value) b = queue.Peek();
			queue.RotateLeft(1000);
			(long index, long value) c = queue.Peek();
			return a.value + b.value + c.value;
		}

		public class RotationalQueue<T> : IEnumerable<T>
		{
			List<T> backingList;

			public int IndexOf(T val)
			{
				return backingList.IndexOf(val);
			}

			public RotationalQueue(List<T> values)
			{
				backingList = new List<T>();
				backingList.AddRange(values);
			}

			public void RotateLeft(long rotate)
			{
				int dist = (int)rotate;
				if (rotate < backingList.Count)
				{
					backingList = backingList.Skip(dist).Concat(backingList.Take(dist)).ToList();
				}
				else
				{
					rotate = rotate % backingList.Count;
					RotateLeft(rotate);
				}
			}

			public void RotateRight(long rotate)
			{
				int dist = (int)rotate;
				if (rotate < backingList.Count)
				{
					backingList = backingList.Skip(backingList.Count - dist).Concat(backingList.Take(backingList.Count - dist)).ToList();
				}
				else
				{
					rotate = rotate % backingList.Count;
					RotateRight(rotate);
				}
			}

			public void Enqueue(T val)
			{
				backingList.Add(val);
			}

			public T Dequeue()
			{
				T r = backingList[0];
				backingList.RemoveAt(0);
				return r;
			}

			public IEnumerator<T> GetEnumerator()
			{
				return backingList.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return backingList.GetEnumerator();
			}

			public T Peek()
			{
				return backingList[0];
			}
		}
	}
}