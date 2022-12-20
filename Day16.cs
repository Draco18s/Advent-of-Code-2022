using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventofCode2022 {
	internal static class DaySixteen {
		class ValveRoom
		{
			public string name;
			public int pressure;
			public string tunnels;
			public List<ValveRoom> connections;

			public ValveRoom(string n, int p)
			{
				name = n;
				pressure = p;
				connections = new List<ValveRoom>();
			}

			public void AddConnection(ValveRoom r)
			{
				if(!connections.Contains(r))
				connections.Add(r);
				if (!r.connections.Contains(this))
					r.connections.Add(this);
			}
		}
		class PathOption
		{
			public string roomName;
			public ValveRoom thisRoom;
			public int timeToHere;
			public List<PathOption> next;

			public long cachedReleaseTotal = -1;
			List<string> cachedPathTaken;
			List<string> cachedOpenValves;

			public PathOption()
			{
				next = new List<PathOption>();
			}

			public long GetPressureReleased(ref List<string> pathTaken, ref List<string> openValves)
			{
				if (cachedReleaseTotal >= 0)
				{
					pathTaken = cachedPathTaken;
					openValves = cachedOpenValves;
					return cachedReleaseTotal;
				}
				long ourReleaseA = 0;
				long ourReleaseB = 0;
				pathTaken.Add(this.roomName);

				long bestChild = 0;
				List<string> bestChildValves = null;
				List<string> bestChildPath = null;
				foreach (PathOption pathOption in next)
				{
					List<string> valves = new List<string>();
					List<string> path = new List<string>();
					valves.AddRange(openValves);
					path.AddRange(pathTaken);
					long child = pathOption.GetPressureReleased(ref path, ref valves);
					if (bestChild < child)
					{
						bestChildPath = path;
						bestChildValves = valves;
						bestChild = child;
					}
				}
				if (!openValves.Contains(roomName) && thisRoom.pressure > 0 && (bestChildPath?.Count + openValves.Count) < 29)
				{
					ourReleaseA = (30 - (bestChildPath.Count + openValves.Count + 1)) * thisRoom.pressure;
				}

				if (!openValves.Contains(roomName) && thisRoom.pressure > 0 && (pathTaken.Count + openValves.Count) < 29)
				{
					ourReleaseB = (30 - (pathTaken.Count + openValves.Count + 1)) * thisRoom.pressure;
				}

				long bestChild2 = 0;
				List<string> bestChildValves2 = null;
				List<string> bestChildPath2 = null;
				foreach (PathOption pathOption in next)
				{
					List<string> valves = new List<string>();
					List<string> path = new List<string>();
					valves.AddRange(openValves);
					if(ourReleaseB > 0) valves.Add(this.roomName);
					path.AddRange(pathTaken);
					long child = pathOption.GetPressureReleased(ref path, ref valves);
					if (bestChild2 < child)
					{
						bestChildPath2 = path;
						bestChildValves2 = valves;
						bestChild2 = child;
					}
				}
				cachedReleaseTotal = 0;
				if (bestChild + ourReleaseA > bestChild2 + ourReleaseB)
				{
					cachedReleaseTotal = bestChild + ourReleaseA;
					openValves.AddRange(bestChildValves);
					pathTaken = bestChildPath;
					if(ourReleaseA > 0) openValves.Add(this.roomName);
				}
				else if(bestChild2 > 0)
				{
					cachedReleaseTotal = bestChild2 + ourReleaseB;
					openValves.AddRange(bestChildValves2);
					pathTaken = bestChildPath2;
					if (ourReleaseB > 0) openValves.Add(this.roomName);
				}
				else if(thisRoom.pressure > 0 && (pathTaken.Count + openValves.Count + 1) < 30)
				{
					ourReleaseB = (30 - (pathTaken.Count + openValves.Count + 1)) * thisRoom.pressure;
					cachedReleaseTotal = ourReleaseB;
					if (ourReleaseB > 0) openValves.Add(this.roomName);
				}
				openValves = openValves.Distinct().ToList();

				cachedPathTaken = new List<string>();
				cachedPathTaken.AddRange(pathTaken);
				cachedOpenValves = new List<string>();
				cachedOpenValves.AddRange(openValves);
				return cachedReleaseTotal;
			}
		}
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long sum = 0;
			Regex pressureMatch = new Regex("rate=(\\d+);");
			Regex tunnelMatch = new Regex("valves( ([A-Z]+),?)+");
			Dictionary<string, ValveRoom> valves = new Dictionary<string, ValveRoom>();
			foreach (string lin in lines)
			{
				Match preMatch = pressureMatch.Match(lin);
				int pressure = int.Parse(preMatch.Groups[1].Value);
				string name = lin.Substring(6, 2);
				ValveRoom room = new ValveRoom(name, pressure);
				preMatch = tunnelMatch.Match(lin);
				room.tunnels = preMatch.Value.Replace("valves","");
				valves.Add(name, room);
			}
			foreach (ValveRoom r in valves.Values)
			{
				string[] conns = r.tunnels.Split(',');
				foreach (string c in conns)
				{
					if (string.IsNullOrWhiteSpace(c)) continue;
					var c2 = c.Replace(" ", "");
					r.AddConnection(valves[c2]);
				}
			}
			PathOption currentroom = new PathOption();
			currentroom.roomName = "AA";
			currentroom.thisRoom = valves["AA"];

			ParseRoomGraph(currentroom, "AA");

			List<string> pathtaken = new List<string>();
			List<string> valvesOpen = new List<string>();
			sum = currentroom.GetPressureReleased(ref pathtaken, ref valvesOpen);

			return sum;
		}

		private static void ParseRoomGraph(PathOption currentroom, string prevRoom)
		{
			if (currentroom.timeToHere >= 29) return;
			foreach (ValveRoom rn in currentroom.thisRoom.connections)
			{
				PathOption newRoom = new PathOption();
				newRoom.roomName = rn.name;
				newRoom.thisRoom = rn;
				newRoom.timeToHere = currentroom.timeToHere + 1;// + (currentroom.thisRoom.pressure > 0 ? 1 : 0);
				currentroom.next.Add(newRoom);
			}
			foreach(var nr in currentroom.next)
			{
				if (nr.roomName == prevRoom) continue;
				ParseRoomGraph(nr, currentroom.roomName);
			}
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;

			return sum;
		}
	}
}