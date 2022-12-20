using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayEighteen {
		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			//Grid3D lava = new Grid3D();
			List<Vector3> cubes = new List<Vector3>();
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(',');
				Vector3 p = new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
				cubes.Add(p);
			}
			int minX = cubes.Min(x => x.x);
			int minY = cubes.Min(x => x.y);
			int minZ = cubes.Min(x => x.z);
			int maxX = cubes.Max(x => x.x);
			int maxY = cubes.Max(x => x.y);
			int maxZ = cubes.Max(x => x.z);

			Grid3D lava = new Grid3D(maxX - minX + 3, maxY - minY + 3, maxZ - minZ + 3, minX-1, minY-1, minZ-1);

			foreach(Vector3 p in cubes)
			{
				lava[p.x, p.y, p.z] = 1;
			}

			for(int x=lava.MinX; x < lava.MaxX; x++)
			{
				for (int y = lava.MinY; y < lava.MaxY; y++)
				{
					for (int z = lava.MinZ; z < lava.MaxZ; z++)
					{
						sum += CountSides(lava, x, y, z);
					}
				}
			}

			return sum;
		}

		private static int CountSides(Grid3D lava, int cx, int cy, int cz)
		{
			if (lava[cx, cy, cz, true] == 0) return 0;
			int count = 0;
			if (lava[cx + 1, cy, cz] == 0) count++;
			if (lava[cx - 1, cy, cz] == 0) count++;
			if (lava[cx, cy + 1, cz] == 0) count++;
			if (lava[cx, cy - 1, cz] == 0) count++;
			if (lava[cx, cy, cz + 1] == 0) count++;
			if (lava[cx, cy, cz - 1] == 0) count++;
			return count;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			int sum = 0;
			//Grid3D lava = new Grid3D();
			List<Vector3> cubes = new List<Vector3>();
			foreach (string lin in lines)
			{
				string[] parts = lin.Split(',');
				Vector3 p = new Vector3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
				cubes.Add(p);
			}
			int minX = cubes.Min(x => x.x);
			int minY = cubes.Min(x => x.y);
			int minZ = cubes.Min(x => x.z);
			int maxX = cubes.Max(x => x.x);
			int maxY = cubes.Max(x => x.y);
			int maxZ = cubes.Max(x => x.z);

			Grid3D lava = new Grid3D(maxX - minX + 3, maxY - minY + 3, maxZ - minZ + 3, minX - 1, minY - 1, minZ - 1);

			foreach (Vector3 p in cubes)
			{
				lava[p.x, p.y, p.z] = 1;
			}

			FloodFill(lava, 0, 0, 0);

			for (int x = lava.MinX; x < lava.MaxX; x++)
			{
				for (int y = lava.MinY; y < lava.MaxY; y++)
				{
					for (int z = lava.MinZ; z < lava.MaxZ; z++)
					{
						sum += CountExternalSides(lava, x, y, z);
					}
				}
			}

			return sum;
		}

		private static void FloodFill(Grid3D lava, int x, int y, int z)
		{
			if (x < lava.MinX || x >= lava.MaxX) return;
			if (y < lava.MinY || y >= lava.MaxY) return;
			if (z < lava.MinZ || z >= lava.MaxZ) return;
			if (lava[x, y, z, true] == 0)
			{
				lava[x, y, z, true] = 2;
				FloodFill(lava, x + 1, y, z);
				FloodFill(lava, x - 1, y, z);
				FloodFill(lava, x, y + 1, z);
				FloodFill(lava, x, y - 1, z);
				FloodFill(lava, x, y, z + 1);
				FloodFill(lava, x, y, z - 1);
			}
		}

		private static int CountExternalSides(Grid3D lava, int cx, int cy, int cz)
		{
			if (lava[cx, cy, cz, true] == 0 || lava[cx, cy, cz, true] == 2) return 0;
			int count = 0;
			if (lava[cx + 1, cy, cz] == 2) count++;
			if (lava[cx - 1, cy, cz] == 2) count++;
			if (lava[cx, cy + 1, cz] == 2) count++;
			if (lava[cx, cy - 1, cz] == 2) count++;
			if (lava[cx, cy, cz + 1] == 2) count++;
			if (lava[cx, cy, cz - 1] == 2) count++;
			return count;
		}
	}
}