using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventofCode2022 {
	internal static class DayNineteen
	{
		public class Blueprint
		{
			public int id;
			public int oreRobotCost;
			public int clayRobotCost;
			public int obsidianRobotCost_ore;
			public int obsidianRobotCost_clay;
			public int geodeRobotCost_ore;
			public int geodeRobotCost_obsidian;

			public int GetRobotOreCost(RobotType t)
			{
				switch (t)
				{
					case RobotType.ORE:
						return oreRobotCost;
					case RobotType.CLAY:
						return clayRobotCost;
					case RobotType.OBSIDIAN:
						return obsidianRobotCost_ore;
					case RobotType.GEODE:
						return geodeRobotCost_ore;
				}
				return 0;
			}

			public int GetRobotClayCost(RobotType t)
			{
				switch (t)
				{
					case RobotType.OBSIDIAN:
						return obsidianRobotCost_clay;
				}
				return 0;
			}

			public int GetRobotObsCost(RobotType t)
			{
				switch (t)
				{
					case RobotType.GEODE:
						return geodeRobotCost_obsidian;
				}
				return 0;
			}
		}

		public enum RobotType
		{
			ORE, CLAY, OBSIDIAN, GEODE
		}

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			List<Blueprint> btList = new List<Blueprint>();
			Regex regId = new Regex("(\\d+):");
			Regex regOre = new Regex("(\\d+) ore");
			Regex regClay = new Regex("(\\d+) clay");
			Regex regObs = new Regex("(\\d+) obsidian");
			foreach (string lin in lines) {
				string[] parts = lin.Split('.');
				int id = int.Parse(regId.Match(parts[0]).Groups[1].Value);
				int ore = int.Parse(regOre.Match(parts[0]).Groups[1].Value);
				int clay = int.Parse(regOre.Match(parts[1]).Groups[1].Value);
				int obsOre = int.Parse(regOre.Match(parts[2]).Groups[1].Value);
				int obsClay = int.Parse(regClay.Match(parts[2]).Groups[1].Value);
				int geodeOre = int.Parse(regOre.Match(parts[3]).Groups[1].Value);
				int geodeObs = int.Parse(regObs.Match(parts[3]).Groups[1].Value);
				Blueprint b = new Blueprint()
				{
					id = id,
					oreRobotCost = ore,
					clayRobotCost = clay,
					obsidianRobotCost_ore = obsOre,
					obsidianRobotCost_clay = obsClay,
					geodeRobotCost_ore = geodeOre,
					geodeRobotCost_obsidian = geodeObs
				};
				btList.Add(b);
			}
			foreach (Blueprint b in btList)
			{
				int geodesHarvested = Harvest(b);
				sum += (b.id * geodesHarvested);
			}
			return sum;
		}

		private static int Harvest(Blueprint b)
		{
			Dictionary<RobotType, int> ores = new Dictionary<RobotType, int>();
			Dictionary<RobotType, int> robots = new Dictionary<RobotType, int>();
			Dictionary<RobotType, int> pending = new Dictionary<RobotType, int>();
			Dictionary<RobotType, int> desired = new Dictionary<RobotType, int>();
			Array types = Enum.GetValues(typeof(RobotType));
			List<RobotType> robotTypes = new List<RobotType>();
			foreach (RobotType e in types)
			{
				ores.Add(e, 0);
				robots.Add(e, 0);
				pending.Add(e, 0);
				robotTypes.Add(e);
				desired.Add(e, 1);
			}
			robots[RobotType.ORE] = 1;
			desired[RobotType.GEODE] = 9000;
			robotTypes.Reverse();

			int totalOreCostOfOneGeode = b.GetRobotOreCost(RobotType.GEODE) + b.GetRobotOreCost(RobotType.OBSIDIAN) + b.GetRobotOreCost(RobotType.CLAY);
			int totalClayCostOfOneGeode = b.GetRobotClayCost(RobotType.OBSIDIAN);

			int vv = Math.Max(b.GetRobotClayCost(RobotType.OBSIDIAN), b.GetRobotOreCost(RobotType.CLAY)+b.GetRobotOreCost(RobotType.OBSIDIAN))/2;
			if((24-vv) / b.GetRobotObsCost(RobotType.GEODE) > 1)
			{
				desired[RobotType.OBSIDIAN]++;
			}

			if (totalClayCostOfOneGeode / (24-b.GetRobotOreCost(RobotType.CLAY)- b.GetRobotClayCost(RobotType.OBSIDIAN)* robots[RobotType.OBSIDIAN] - b.GetRobotObsCost(RobotType.GEODE)) > 1)
			{
				desired[RobotType.CLAY]++;
				totalOreCostOfOneGeode += b.GetRobotOreCost(RobotType.CLAY);
			}
			if(totalClayCostOfOneGeode * 2 / (24 - b.GetRobotOreCost(RobotType.CLAY) - b.GetRobotClayCost(RobotType.OBSIDIAN)* robots[RobotType.OBSIDIAN] - b.GetRobotObsCost(RobotType.GEODE)) >= 1)
			{
				desired[RobotType.CLAY]++;
				totalOreCostOfOneGeode += b.GetRobotOreCost(RobotType.CLAY);
			}
			if (totalOreCostOfOneGeode / 24 > 1)
			{
				desired[RobotType.ORE]++;
			}
			if(b.GetRobotOreCost(RobotType.CLAY) < b.GetRobotOreCost(RobotType.ORE))
			{
				desired[RobotType.CLAY]*=2;
			}
			else
			{
				desired[RobotType.ORE]*=2;
			}

			for (int i=0; i < 24; i++)
			{
				//activate new robots
				foreach (RobotType t in robotTypes)
				{
					robots[t] += pending[t];
					pending[t] = 0;
				}
				//start producing robot
				foreach (RobotType t in robotTypes)
				{
					if (robots[t] < desired[t] && b.GetRobotObsCost(t) <= ores[RobotType.OBSIDIAN] && b.GetRobotClayCost(t) <= ores[RobotType.CLAY] && b.GetRobotOreCost(t) <= ores[RobotType.ORE])
					{
						ores[RobotType.OBSIDIAN] -= b.GetRobotObsCost(t);
						ores[RobotType.CLAY] -= b.GetRobotClayCost(t);
						ores[RobotType.ORE] -= b.GetRobotOreCost(t);
						pending[t]++;
					}
				}
				//harvest resources
				foreach (KeyValuePair<RobotType,int> bot in robots)
				{
					ores[bot.Key] += bot.Value;
				}
			}



			return ores[RobotType.GEODE];
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			foreach (string lin in lines)
			{

			}
			return sum;
		}
	}
}