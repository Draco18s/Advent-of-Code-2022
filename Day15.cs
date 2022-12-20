using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventofCode2022 {
	class Sensor
	{
		public Vector2 pos;
		public Vector2 beaconPos;
		public long distance;
	}
	internal static class DayFifteen
	{
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			Regex regex = new Regex("x=(-?\\d+), y=(-?\\d+)");
			List<Sensor> sensors = new List<Sensor>();
			long largestX = long.MinValue;
			long smallestX = long.MaxValue;
			foreach (string lin in lines)
			{
				MatchCollection mc = regex.Matches(lin);
				List<Vector2> vecs = new List<Vector2>();
				for (int i = 0; i < mc.Count; i++)
				{
					Match m = mc[i];
					vecs.Add(new Vector2(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)));
				}
				Sensor s = new Sensor
				{
					pos = vecs[0],
					beaconPos = vecs[1]
				};
				sensors.Add(s);
				var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
				if (s.pos.x + distBeacon > largestX) largestX = s.pos.x + distBeacon;
				if (s.beaconPos.x + distBeacon > largestX) largestX = s.beaconPos.x + distBeacon;

				if (s.pos.x - distBeacon < smallestX) smallestX = s.pos.x - distBeacon;
				if (s.beaconPos.x - distBeacon < smallestX) smallestX = s.beaconPos.x - distBeacon;
			}
			largestX += 10;
			smallestX -= 10;
			long theYiCareAbout = 2000000;
			long maxX = int.MaxValue;
			for (long x = largestX; x > smallestX; x--)
			{
				Vector2 p = new Vector2(x, theYiCareAbout);
				var count = sensors.Sum(s =>
				{
					var distTo = Math.Abs(s.pos.x - p.x) + Math.Abs(s.pos.y - p.y);
					var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
					if (distTo <= distBeacon) return 1;
					return 0;
				});
				if (count > 0)
				{
					maxX = x;
					break;
				}
			}
			bool enable = false;
			for (long x = smallestX - 1; x <= maxX + 1; x++)
			{
				Vector2 p = new Vector2(x, theYiCareAbout);
				var count = sensors.Sum(s =>
				{
					var distTo = Math.Abs(s.pos.x - p.x) + Math.Abs(s.pos.y - p.y);
					var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
					if (distTo <= distBeacon) return 1;
					return 0;
				});
				if (count > 0 && enable)
					sum++;
				else if (count > 0) enable = true;
			}
			return sum;
		}
		internal static long Part2(string input)
		{
			string[] lines = input.Split('\n');
			int sum = 0;
			Regex regex = new Regex("x=(-?\\d+), y=(-?\\d+)");
			List<Sensor> sensors = new List<Sensor>();
			long largestX = int.MinValue;
			long smallestX = int.MaxValue;
			long shorestDist = int.MaxValue;
			foreach (string lin in lines)
			{
				sum = 0;
				MatchCollection mc = regex.Matches(lin);
				List<Vector2> vecs = new List<Vector2>();
				for (int i = 0; i < mc.Count; i++)
				{
					Match m = mc[i];
					vecs.Add(new Vector2(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)));
				}
				Sensor s = new Sensor
				{
					pos = vecs[0],
					beaconPos = vecs[1]
				};
				long distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
				s.distance = distBeacon;
				sensors.Add(s);
				if (distBeacon < shorestDist) shorestDist = distBeacon;
				if (s.pos.x > largestX) largestX = s.pos.x;
				if (s.beaconPos.x > largestX) largestX = s.beaconPos.x;

				if (s.pos.x < smallestX) smallestX = s.pos.x;
				if (s.beaconPos.x < smallestX) smallestX = s.beaconPos.x;
			}
			Dictionary<Vector2,int> edges = new Dictionary<Vector2, int>();

			foreach(Sensor s in sensors)
			{
				Vector2 p1;
				int o = 1;
				for(long i = -(s.distance+1), j = 0; i < s.distance+1; i++)
				{
					p1 = new Vector2(s.pos.x + i, s.pos.y + j);
					if(p1.x == 14 && p1.y == 11)
					{
						;
					}
					if (edges.ContainsKey(p1))
						edges[p1]++;
					else
						edges.Add(p1, 1);
					if (j != 0)
					{
						p1 = new Vector2(s.pos.x + i, s.pos.y - j);
						if (p1.x == 14 && p1.y == 11)
						{
							;
						}
						if (edges.ContainsKey(p1))
							edges[p1]++;
						else
							edges.Add(p1, 1);
					}
					j += o;
					if (i == 0) o = -o;
				}
			}

			List<Vector2> locations = edges.Where(p => p.Value >= 3).Select(p => p.Key).ToList();
			foreach(Vector2 loc in locations)
			{
				if (loc.x < 0 || loc.x > 4000000) continue;
				if (loc.y < 0 || loc.y > 4000000) continue;
				if (sensors.Any(x => x.beaconPos == loc || x.pos == loc)) continue;
				if (CheckDists(sensors, loc)) continue;
				Console.WriteLine($"{loc} {loc.x * 4000000 + loc.y}");
			}


			return -1;
		}

		private static bool CheckDists(List<Sensor> sensors, Vector2 loc)
		{
			foreach (Sensor s in sensors)
			{
				if (ManDist(s.pos, loc) <= s.distance)
				{
					return true;
				}
			}
			return false;
		}

		static bool IsInside(List<Vector2> poly, long x, long y)
		{
			int i, j;
			bool c = false;
			for (i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
			{
				if (((poly[i].y > y) != (poly[j].y > y)) &&
				 (x < (poly[j].x - poly[i].x) * (y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x))
					c = !c;
			}
			return c;
		}

		private static List<Vector2> IntersectionPoints(List<Sensor> sensors)
		{
			List<Vector2> allPts = new List<Vector2>();
			foreach (Sensor s1 in sensors)
			{
				foreach (Sensor s2 in sensors)
				{
					if (s1 == s2) continue;
					(Vector2 a, Vector2 b) = Intersect(s1, s2);
					if (a != neg) allPts.Add(a);
					if (b != neg) allPts.Add(b);
				}
			}
			return allPts.Distinct().ToList();
		}
		private static Vector2 neg = new Vector2(int.MinValue, int.MinValue);
		private static (Vector2, Vector2) Intersect(Sensor s1, Sensor s2)
		{
			Vector2 P0 = new Vector2(s1.pos.x, s1.pos.y);
			Vector2 P1 = new Vector2(s2.pos.x, s2.pos.y);

			long xd = P1.x - P0.x;
			long yd = P1.y - P0.y;
			if (Math.Abs(xd) + Math.Abs(yd) > s1.distance + s2.distance) return (neg,neg);

			long overlap = (s1.distance + s2.distance) - Math.Abs(xd) - Math.Abs(yd);

			Vector2 r1 = new Vector2(P0.x + (s1.distance - overlap)*Math.Sign(xd), P0.y + overlap * Math.Sign(yd));
			Vector2 r2 = new Vector2(P1.x - (s1.distance - overlap) * Math.Sign(xd), P1.y - overlap * Math.Sign(yd));

			return (r1, r2);
		}

		private static long ManDist(Vector2 p0, Vector2 p1)
		{
			return Math.Abs(p0.x - p1.x) + Math.Abs(p0.y - p1.y);
		}

		public static List<Vector2> Wrap(List<Vector2> points, bool loop = false)
		{
			//The points which will be returned
			var returnPoints = new List<Vector2>();

			//Find left-most point on x axis
			var currentPoint = lowestXCoord(points);

			//The endpoint to compare against
			Vector2 endpoint = new Vector2(0,0);

			while (true)
			{
				//Add current point
				returnPoints.Add(currentPoint);

				//Set endpoint back to the first point in the list of points
				endpoint = points[(points.IndexOf(currentPoint)+1)% points.Count];

				for (var j = 1; j < points.Count; j++)
				{
					//Run through points -- if the turn from this point to the other is greater, set endpoint to this
					if ((endpoint == currentPoint) || (ccw(currentPoint, endpoint, points[j]) < 0))
						endpoint = points[j];
				}

				//Set current point
				currentPoint = endpoint;

				//Break condition -- if we've looped back around then we've made a convex hull!
				if (endpoint == returnPoints[0])
					break;
				if(returnPoints.Contains(endpoint))
				{
					break;
				}
			}

			//If we want to loop, include the first vertex again
			if (loop)
				returnPoints.Add(returnPoints[0]);

			//And finally, return the points
			return returnPoints;
		}

		private static Vector2 lowestYCoord(List<Vector2> array)
		{
			return array.First(p => p.y == (array.Min(y => y.y)));
		}

		private static Vector2 lowestXCoord(List<Vector2> array)
		{
			return array.First(p => p.x == (array.Min(y => y.x)));
		}

		private static Vector2 highestYCoord(List<Vector2> array)
		{
			return array.First(p => p.y == (array.Max(y => y.y)));
		}

		private static Vector2 highestXCoord(List<Vector2> array)
		{
			return array.First(p => p.x == (array.Max(y => y.x)));
		}

		public static float ccw(Vector2 p, Vector2 q, Vector2 r)
		{
			long val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
			if (val == 0) return 0;     // colinear
			return (val > 0) ? 1 : -1;   // clock or counterclock wise
		}

		private static void swap(ref Vector2[] array, int idxA, int idxB)
		{
			//temp = a
			var temp = array[idxA];

			//a overwritten with b
			array[idxA] = array[idxB];

			//b overwritten with temp
			array[idxB] = temp;
		}
		//first result
		//x = 3428245
		//y = 1647951

		//2872552
		//2610673

		//2958180,2610673

		//5649579775995
		//7499293947496 <-- too low
		//
		//7722840655140 <-- wrong, but close?
		//
		//16000000000000 <-- too high

		//
		//2958180,2610673 = 7722840655140

		/*internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			Regex regex = new Regex("x=(-?\\d+), y=(-?\\d+)");
			List<Sensor> sensors = new List<Sensor>();
			int largestX = int.MinValue;
			int smallestX = int.MaxValue;
			int shorestDist = int.MaxValue;
			foreach (string lin in lines)
			{
				sum = 0;
				MatchCollection mc = regex.Matches(lin);
				List<Vector2> vecs = new List<Vector2>();
				for (int i = 0; i < mc.Count; i++)
				{
					Match m = mc[i];
					vecs.Add(new Vector2(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)));
				}
				Sensor s = new Sensor
				{
					pos = vecs[0],
					beaconPos = vecs[1]
				};
				sensors.Add(s);
				var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
				if (distBeacon < shorestDist) shorestDist = distBeacon;
				if (s.pos.x > largestX) largestX = s.pos.x;
				if (s.beaconPos.x > largestX) largestX = s.beaconPos.x;

				if (s.pos.x < smallestX) smallestX = s.pos.x;
				if (s.beaconPos.x < smallestX) smallestX = s.beaconPos.x;
			}
			largestX -= shorestDist;
			smallestX += shorestDist;
			for (int theYiCareAbout = shorestDist-1; theYiCareAbout <= 4000000; theYiCareAbout++)
			{
				int maxX = int.MaxValue;
				for (int x = largestX; x > smallestX; x--)
				{
					Vector2 p = new Vector2(x, theYiCareAbout);
					var count = sensors.Any(s =>
					{
						var distTo = Math.Abs(s.pos.x - p.x) + Math.Abs(s.pos.y - p.y);
						var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
						if (distTo <= distBeacon) return true;
						return false;
					});
					if (count)
					{
						maxX = x;
						break;
					}
				}
				bool enable = false;
				int firstX = int.MinValue;
				int solvedX = int.MaxValue;
				for (int x = smallestX; x <= maxX;)
				{
					Vector2 p = new Vector2(x, theYiCareAbout);
					var count = sensors.Min(s =>
					{
						var distTo = Math.Abs(s.pos.x - p.x) + Math.Abs(s.pos.y - p.y);
						var distBeacon = Math.Abs(s.pos.x - s.beaconPos.x) + Math.Abs(s.pos.y - s.beaconPos.y);
						return distBeacon - distTo;
					});
					var found = count >= 0;
					if(enable && !found)
					{
						solvedX = x;
						break;
					}
					if (found && enable)
						sum++;
					else if (found)
					{
						firstX = x;
						enable = true;
					}
					x += count + 1;
				}
				if(sum < maxX - firstX)
				{
					Console.WriteLine($"{solvedX}*{theYiCareAbout}");
					return theYiCareAbout*solvedX;
				}
			}
			return sum;
		}*/
	}
}