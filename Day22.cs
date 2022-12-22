using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DayTwentytwo {
		enum Facing { PX, PY, NX, NY }
		internal static long Part1(string input) {
			string[] lines = input.Split("\n\n");
			int sum = 0;
			List<int> carry = new List<int>();
			Grid board = new Grid(lines[0], true);
			Grid map = new Grid(lines[0], true);
			int last = 0;
			Vector2 pos = GetInitialPos(board);
			Facing dir = Facing.PX;
			foreach (char c in lines[1]) {
				if(char.IsNumber(c))
				{
					last *= 10;
					last += int.Parse(c.ToString());
				}
				else
				{
					pos = MoveForward(pos, dir, last, board, map);
					dir = Rotate(dir, c);
					last = 0;
				}
			}
			if(last > 0)
				pos = MoveForward(pos, dir, last, board, map);
			//Console.WriteLine(map.ToString("char+0"));
			int col = pos.x+1;
			int row = pos.y + 1;
			int fac = (int)dir;
			return 1000*row + 4*col + fac;
		}

		private static Facing Rotate(Facing dir, char c)
		{
			if (c == 'R') return (Facing)(((int)dir + 1) % 4);
			if (c == 'L') return (Facing)(((int)dir + 3) % 4);
			return dir;
		}

		private static Vector2 MoveForward(Vector2 pos, Facing dir, int dist, Grid board, Grid map)
		{
			Vector2 step = new Vector2(dir == Facing.PX ? 1 : (dir == Facing.NX ? -1 : 0), dir == Facing.PY ? 1 : (dir == Facing.NY ? -1 : 0));
			for (int i = 0; i < dist; i++)
			{
				if ((pos + step).x >= board.MaxX)
				{
					pos = new Vector2(board.MinX, pos.y) - step;
				}
				if ((pos + step).x < board.MinX)
				{
					pos = new Vector2(board.MaxX - 1, pos.y) - step;
				}
				if ((pos + step).y >= board.MaxY)
				{
					pos = new Vector2(pos.x, board.MinY) - step;
				}
				if ((pos + step).y < board.MinY)
				{
					pos = new Vector2(pos.x, board.MaxY - 1) - step;
				}
				if (board[pos + step] == '.')
				{
					map[pos + step] += (int)dir+1;
					pos = pos + step;
					continue;
				}
				if (board[pos + step] == '#')
				{
					continue;
				}
				Vector2 oPos = pos;
				do
				{
					pos = pos + step;
					if ((pos + step).x >= board.MaxX)
					{
						pos = new Vector2(board.MinX, pos.y) - step;
					}
					if ((pos + step).x < board.MinX)
					{
						pos = new Vector2(board.MaxX - 1, pos.y) - step;
					}
					if ((pos + step).y >= board.MaxY)
					{
						pos = new Vector2(pos.x, board.MinY) - step;
					}
					if ((pos + step).y < board.MinY)
					{
						pos = new Vector2(pos.x, board.MaxY - 1) - step;
					}
				} while (board[pos + step] != '.' && board[pos + step] != '#');
				if (board[pos + step] == '.')
				{
					map[pos + step] += (int)dir + 1;
					pos = pos + step;
					continue;
				}
				else
				{
					return oPos;
				}
			}
			return pos;
		}

		private static Vector2 GetInitialPos(Grid board)
		{
			int y = board.MinY;
			for(int x = board.MinX; x < board.MaxX; x++)
			{
				if (board[x, y, true] == '.') return new Vector2(x, y);
			}
			throw new Exception("INVAID BOARD");
		}

		internal static long Part2(string input) {
			string[] lines = input.Split("\n\n");
			Grid3D cube = new Grid3D(52, 52, 52, -1,-1,-1);
			Grid3D map = new Grid3D(52, 52, 52, -1, -1, -1);
			FillCube(lines[0], ref cube);
			FillCube(lines[0], ref map);
			int last = 0;
			Vector3 pos = GetInitialPos(cube);
			CubeFacing dir = CubeFacing.PX;
			foreach (char c in lines[1])
			{
				if (char.IsNumber(c))
				{
					last *= 10;
					last += int.Parse(c.ToString());
				}
				else
				{
					(pos,dir) = MoveForward(pos, dir, last, cube, map);
					dir = Rotate(pos, dir, c);
					last = 0;
				}
			}
			if (last > 0)
				(pos,dir) = MoveForward(pos, dir, last, cube, map);
			Console.WriteLine(map.ToString("char+0"));
			Vector2 p2 = GetFinalPos(pos);
			int col = p2.x + 1;
			int row = p2.y + 1;
			int fac = GetFinalDir(pos,dir);
			return 1000 * row + 4 * col + fac;
			//110034 <-- too high
			//30288 <-- too low
		}

		private static int GetFinalDir(Vector3 pos, CubeFacing dir)
		{
			if (pos.z == 50 || pos.x == 50) return (dir == CubeFacing.PX ? 0 : (dir == CubeFacing.NY ? 1 : (dir == CubeFacing.NX ? 2 : 3)));
			if (pos.y == 50 || pos.z == -1) return (dir == CubeFacing.PX ? 0 : (dir == CubeFacing.NZ ? 1 : (dir == CubeFacing.NX ? 2 : 3)));
			if (pos.x == -1) return (dir == CubeFacing.PZ ? 2 : (dir == CubeFacing.NY ? 1 : (dir == CubeFacing.NZ ? 0 : 3)));
			if (pos.y == -1) return (dir == CubeFacing.PZ ? 2 : (dir == CubeFacing.NX ? 1 : (dir == CubeFacing.NZ ? 0 : 3)));
			throw new Exception("Fek");
		}

		private static Vector2 GetFinalPos(Vector3 pos)
		{
			if (pos.z == -1) return new Vector2(pos.x + 50, 49 - pos.y + 100);
			/* [double checked]
					Vector3 pos = new Vector3(x, 50 - y, -1);
					cube[pos] = lines[y+100][x + 50];*/
			if (pos.z == 50) return new Vector2(pos.x + 50, pos.y);
			/* [double checked]
					Vector3 pos = new Vector3(x, y, 50);
					cube[pos] = lines[y][x + 50];*/
			if (pos.x == -1) return new Vector2(49 - pos.z, 49 - pos.y + 100);
			/* [double checked]
					Vector3 pos = new Vector3(-1, 50 - y, 50 - x);
					cube[pos] = lines[y + 100][x];*/
			if (pos.x == 50) return new Vector2(49 - pos.z + 100, pos.y);
			/* [double checked]
					Vector3 pos = new Vector3(50, y, 50 - x);
					cube[pos] = lines[y][x + 100];*/
			if (pos.y == -1) return new Vector2(49 - pos.z, pos.x + 150);
			/* [double checked]
					Vector3 pos = new Vector3(y, -1, 50-x);
					cube[pos] = lines[y+150][x];*/
			if (pos.y == 50) return new Vector2(pos.x + 50, 49 - pos.z + 50);
			/* [double checked]
					Vector3 pos = new Vector3(x, 50, 50 - y);
					cube[pos] = lines[y+50][x + 50];*/

			throw new Exception("Fuck fuck fuck");
		}

		private static CubeFacing Rotate(Vector3 pos, CubeFacing dir, char c)
		{
			if(pos.z == -1) //bottom face [double checked]
			{
				switch (dir)
				{
					case CubeFacing.PX:
						return c == 'R' ? CubeFacing.NY : CubeFacing.PY;
					case CubeFacing.NX:
						return c == 'R' ? CubeFacing.PY : CubeFacing.NY;
					case CubeFacing.PY:
						return c == 'R' ? CubeFacing.PX : CubeFacing.NX;
					case CubeFacing.NY:
						return c == 'R' ? CubeFacing.NX : CubeFacing.PX;
				}
			}
			else if(pos.z == 50) { //top face [double checked]
				switch(dir)
				{
					case CubeFacing.PX:
						return c == 'L' ? CubeFacing.NY : CubeFacing.PY;
					case CubeFacing.NX:
						return c == 'L' ? CubeFacing.PY : CubeFacing.NY;
					case CubeFacing.PY:
						return c == 'L' ? CubeFacing.PX : CubeFacing.NX;
					case CubeFacing.NY:
						return c == 'L' ? CubeFacing.NX : CubeFacing.PX;
				}
			}
			else if (pos.x == -1)
			{ //left face [double checked]
				switch (dir)
				{
					case CubeFacing.PZ:
						return c == 'R' ? CubeFacing.PY : CubeFacing.NY;
					case CubeFacing.NZ:
						return c == 'R' ? CubeFacing.NY : CubeFacing.PY;
					case CubeFacing.PY:
						return c == 'R' ? CubeFacing.NZ : CubeFacing.PZ;
					case CubeFacing.NY:
						return c == 'R' ? CubeFacing.PZ : CubeFacing.NZ;
				}
			}
			else if (pos.x == 50)
			{ //right face [double checked]
				switch (dir)
				{
					case CubeFacing.PZ:
						return c == 'L' ? CubeFacing.PY : CubeFacing.NY;
					case CubeFacing.NZ:
						return c == 'L' ? CubeFacing.NY : CubeFacing.PY;
					case CubeFacing.PY:
						return c == 'L' ? CubeFacing.NZ : CubeFacing.PZ;
					case CubeFacing.NY:
						return c == 'L' ? CubeFacing.PZ : CubeFacing.NZ;
				}
			}
			else if (pos.y == 50)
			{ //front face [double checked]
				switch (dir)
				{
					case CubeFacing.PZ:
						return c == 'R' ? CubeFacing.PX : CubeFacing.NX;
					case CubeFacing.NZ:
						return c == 'R' ? CubeFacing.NX : CubeFacing.PX;
					case CubeFacing.PX:
						return c == 'R' ? CubeFacing.NZ : CubeFacing.PZ;
					case CubeFacing.NX:
						return c == 'R' ? CubeFacing.PZ : CubeFacing.NZ;
				}
			}
			else if (pos.y == -1)
			{ //back face [double checked]
				switch (dir)
				{
					case CubeFacing.PZ:
						return c == 'L' ? CubeFacing.PX : CubeFacing.NX;
					case CubeFacing.NZ:
						return c == 'L' ? CubeFacing.NX : CubeFacing.PX;
					case CubeFacing.PX:
						return c == 'L' ? CubeFacing.NZ : CubeFacing.PZ;
					case CubeFacing.NX:
						return c == 'L' ? CubeFacing.PZ : CubeFacing.NZ;
				}
			}
			throw new Exception("Fuck fuck fuck");
		}

		private static void FillCube(string input, ref Grid3D cube)
		{
			string[] lines = input.Split('\n');

			//top face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(x, y, 50);
					cube[pos] = lines[y][x + 50];
				}
			}
			//right face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(50, y, 49 - x);
					cube[pos] = lines[y][x + 100];
				}
			}
			//front face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(x, 50, 49 - y);
					cube[pos] = lines[y+50][x + 50];
				}
			}
			//bottom face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(x, 49 - y, -1);
					cube[pos] = lines[y+100][x + 50];
				}
			}
			//left face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(-1, 49 - y, 49 - x);
					cube[pos] = lines[y + 100][x];
				}
			}
			//back face [double checked]
			for (int x = 0; x < 50; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					Vector3 pos = new Vector3(y, -1, 49-x);
					char q = lines[y + 150][x];
					cube[pos] = q;
				}
			}

			for (int x = -1; x < 51; x++)
			{
				cube[x, -1, 50, true] = '|';
				cube[x, 50, -1, true] = '|';
				cube[-1, x, 50, true] = '|';
				cube[50, x, -1, true] = '|';
				cube[-1, 50, x, true] = '|';
				cube[50, -1, x, true] = '|';

				cube[x, -1, -1, true] = '|';
				cube[x, 50, 50, true] = '|';
				cube[-1, x, -1, true] = '|';
				cube[50, x, 50, true] = '|';
				cube[-1, -1, x, true] = '|';
				cube[50, 50, x, true] = '|';
			}
		}
		enum CubeFacing { PX, PY, PZ, NX, NY, NZ }

		private static Vector3 GetInitialPos(Grid3D cube)
		{
			return new Vector3(0, 0, 50);
		}

		private static (Vector3, CubeFacing) MoveForward(Vector3 pos, CubeFacing dir, int dist, Grid3D cube, Grid3D map)
		{
			Vector3 step = new Vector3(dir == CubeFacing.PX ? 1 : (dir == CubeFacing.NX ? -1 : 0), dir == CubeFacing.PY ? 1 : (dir == CubeFacing.NY ? -1 : 0), dir == CubeFacing.PZ ? 1 : (dir == CubeFacing.NZ ? -1 : 0));
			for (int i = 0; i < dist; i++)
			{
				map[pos] += 1;
				if (cube[pos + step] == '|')
				{
					CubeFacing oDir = dir;
					Vector3 oPos = pos;
					//top face always becomes NZ
					if (pos.z == 50)
					{
						dir = CubeFacing.NZ;
					}
					//bottom face always becomes PZ
					if (pos.z == -1)
					{
						dir = CubeFacing.PZ;
					}
					//left face always becomes PX
					if (pos.x == -1)
					{
						dir = CubeFacing.PX;
					}
					//right face always becomes NX
					if (pos.x == 50)
					{
						dir = CubeFacing.NX;
					}
					//back face always becomes PY
					if (pos.y == -1)
					{
						dir = CubeFacing.PY;
					}
					//front face always becomes NY
					if (pos.y == 50)
					{
						dir = CubeFacing.NY;
					}
					pos += step;
					step = new Vector3(dir == CubeFacing.PX ? 1 : (dir == CubeFacing.NX ? -1 : 0), dir == CubeFacing.PY ? 1 : (dir == CubeFacing.NY ? -1 : 0), dir == CubeFacing.PZ ? 1 : (dir == CubeFacing.NZ ? -1 : 0));
					if (cube[pos + step] == '#')
					{
						return (oPos, oDir);
					}
				}
				if (cube[pos + step] == '.')
				{
					pos = pos + step;
					continue;
				}
				else if (cube[pos + step] == '#')
				{
					return (pos,dir);
				}
				else
				{
					char c = (char)cube[pos + step];
					;
				}
			}
			return (pos,dir);
		}
	}
}