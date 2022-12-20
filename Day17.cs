using Draco18s.AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventofCode2022 {
	internal static class DaySeventeen
	{
		internal static long Part1(string input) {
			Grid[] pentominos = new Grid[5];
			for(int x=0; x < 5;x ++)
			{
				pentominos[x] = new Grid(4,4);
			}

			//####
			pentominos[0][0, 3] = '#';
			pentominos[0][1, 3] = '#';
			pentominos[0][2, 3] = '#';
			pentominos[0][3, 3] = '#';

			//    
			// #  
			//###
			// #  
			pentominos[1][0, 2] = '#';
			pentominos[1][1, 1] = '#';
			pentominos[1][1, 2] = '#';
			pentominos[1][1, 3] = '#';
			pentominos[1][2, 2] = '#';

			pentominos[2][0, 3] = '#';
			pentominos[2][1, 3] = '#';
			pentominos[2][2, 3] = '#';
			pentominos[2][2, 2] = '#';
			pentominos[2][2, 1] = '#';

			pentominos[3][0, 0] = '#';
			pentominos[3][0, 1] = '#';
			pentominos[3][0, 2] = '#';
			pentominos[3][0, 3] = '#';

			pentominos[4][0, 2] = '#';
			pentominos[4][0, 3] = '#';
			pentominos[4][1, 2] = '#';
			pentominos[4][1, 3] = '#';

			int pentIndex = 0;
			int jetIndex = 0;
			int jetLength = input.Length;

			int sum = 0;
			Grid board = new Grid(7, 10, 0, -4);
			for(int x = 0; x < 7*5; x++)
			{
				if(x/7 == 0)
					board[x % 7, -x / 7, true] = '#';
				else
					board[x % 7, -x / 7, true] = '.';
			}
			int totalRocks = 2022;
			int top = 0;
			while(--totalRocks >= 0)
			{
				top = GetTopOfGrid(board, top)-2;
				board.IncreaseGridBy(new Vector2(0, -4), () => '.');
				Grid piece = pentominos[pentIndex];

				int xo = 2;
				int yo = top-4;
				bool falling = true;

				//CopyPieceTo(piece, board, xo, yo);
				//Console.WriteLine($"\n{board.ToString("char+0")}\n");
				//UnCopyPieceTo(piece, board, xo, yo);

				while (falling)
				{
					char dir = input[jetIndex];
					//Console.Write(dir);
					jetIndex = (jetIndex + 1) % jetLength;
					int push = dir == '<' ? -1 : 1;
					if (!TestCollision(board, piece, xo + push, yo))
					{
						xo += push;
					}
					if (TestCollision(board, piece, xo, yo+1))
					{
						falling = false;
						//Console.Write('@');
					}
					else
					{
						yo++;
						//Console.Write('v');
					}
					//Console.Write('\n');
					//CopyPieceTo(piece, board, xo, yo);
					//Console.WriteLine($"\n{board.ToString("char+0")}\n");
					//UnCopyPieceTo(piece, board, xo, yo);
				}
				CopyPieceTo(piece, board, xo, yo);
				top = yo+4;
				pentIndex = (pentIndex + 1) % 5;
			}
			/*board.TrimGrid(() => '.');
			if (board.Width != 7)
			{
				board.IncreaseGridToInclude(new Vector2(0, board.MinY + 1), () => '.');
				board.IncreaseGridToInclude(new Vector2(6, board.MinY + 1), () => '.');
			}
			board.IncreaseGridBy(new Vector2(0, -5), () => '.');
			board.DecreaseGridBy(0, 3000);
			while (board.Height > 60)
			{
				board.DecreaseGridBy(0, 20);
			}
			Console.WriteLine(board.ToString("char+0"));*/
			top = GetTopOfGrid(board, top);
			return -(top+1);
		}

		private static void CopyPieceTo(Grid piece, Grid board, int xo, int yo)
		{
			for (int y = 0; y < 4; y++)
			{
				for (int x = 0; x < 4; x++)
				{
					if (x + xo < 0 || x + xo >= 7) continue;
					if (y + yo >= board.MaxY) continue;
					if (piece[x, y] == '#') {
						board[x + xo, y + yo, true] = '#';
					}
				}
			}
		}
		private static void UnCopyPieceTo(Grid piece, Grid board, int xo, int yo)
		{
			for (int y = 0; y < 4; y++)
			{
				for (int x = 0; x < 4; x++)
				{
					if (x + xo < 0 || x + xo >= 7) continue;
					if (piece[x, y] == '#')
					{
						board[x + xo, y + yo, true] = '.';
					}
				}
			}
		}

		private static bool TestCollision(Grid board, Grid piece, int xo, int yo)
		{
			for(int y=0; y<4;y++)
			{
				for (int x = 0; x < 4; x++)
				{
					if (x + xo < 0 || (x + xo >= 7 && piece[x, y] == '#')) return true;
					if (x + xo >= 7) continue;
					if (y + yo >= board.MaxY)
					{
						if (y + yo >= board.MaxX + 5) return true;
						continue;
					}
					if (piece[x, y] == '#' && board[x+xo, y+yo, true] == '#') return true;
				}
			}
			return false;
		}

		private static int GetTopOfGrid(Grid board, int y)
		{
			while (true)
			{
				bool empty = true;
				for (int x = 0; x < 7 && empty; x++)
				{
					if (y >= board.MaxY) empty = false;
					else if (board[x, y, true] == '#') empty = false;
				}
				if(empty)
				{
					return y;
				}
				y--;
			}
		}

		internal static long Part2(string input) {

			//File.WriteAllText(outputFile, string.Format(htmlTemplate, string.Format(mainTable, builder.ToString())));
			string path = "C:\\Users\\draco\\Documents\\Advent-of-Code-2022\\Day17_Out.txt";

			Grid[] pentominos = new Grid[5];
			for (int x = 0; x < 5; x++)
			{
				pentominos[x] = new Grid(4, 4);
			}

			//####
			pentominos[0][0, 3] = '#';
			pentominos[0][1, 3] = '#';
			pentominos[0][2, 3] = '#';
			pentominos[0][3, 3] = '#';

			//    
			// #  
			//###
			// #  
			pentominos[1][0, 2] = '#';
			pentominos[1][1, 1] = '#';
			pentominos[1][1, 2] = '#';
			pentominos[1][1, 3] = '#';
			pentominos[1][2, 2] = '#';

			pentominos[2][0, 3] = '#';
			pentominos[2][1, 3] = '#';
			pentominos[2][2, 3] = '#';
			pentominos[2][2, 2] = '#';
			pentominos[2][2, 1] = '#';

			pentominos[3][0, 0] = '#';
			pentominos[3][0, 1] = '#';
			pentominos[3][0, 2] = '#';
			pentominos[3][0, 3] = '#';

			pentominos[4][0, 2] = '#';
			pentominos[4][0, 3] = '#';
			pentominos[4][1, 2] = '#';
			pentominos[4][1, 3] = '#';

			int pentIndex = 0;
			int jetIndex = 0;
			int jetLength = input.Length;

			int sum = 0;
			Grid board = new Grid(7, 10, 0, -4);
			for (int x = 0; x < 7 * 5; x++)
			{
				if (x / 7 == 0)
					board[x % 7, -x / 7, true] = '#';
				else
					board[x % 7, -x / 7, true] = '.';
			}
			long totalRocks = 1000000000000L;
			int top = 0;
			Dictionary<int[], (int,long)> segmentLookup = new Dictionary<int[], (int, long)>();
			while (--totalRocks >= 0)
			{
				top = GetTopOfGrid(board, top) - 2;
				/*if((1000000000000-totalRocks) % (5*jetLength*10000) == 0)
				{
					//Console.WriteLine((-(top + 1)));
					double err = 2 * Math.PI * 1000000000000L / (1000000000000d - totalRocks);
					double est = ((-(top + 1))) / (1000000000000d - totalRocks) * 1000000000000L;
					if (Math.Abs((est - err) - 1514285714288) <= 10 || Math.Abs((est + err) - 1514285714288) <= 10)
					{
						Console.WriteLine($"Height: {(-(top + 1))}");
						Console.WriteLine($"Rocks: {(1000000000000 - totalRocks)}");
						;
					}
				}*/
				//rocks 1447993

				//1513837606008
				//783132499999 <-- too low
				board.IncreaseGridBy(new Vector2(0, -4), () => '.');
				if(board.Height > 150)
				{
					//Console.WriteLine($"\n{board.ToString("char+0")}\n");
					//board.DecreaseGridBy(new Vector2(0, -4));
					board.TrimGrid(() => '.');
					if (board.Width != 7)
					{
						board.IncreaseGridToInclude(new Vector2(0, board.MinY + 1), () => '.');
						board.IncreaseGridToInclude(new Vector2(6, board.MinY + 1), () => '.');
					}
					board.IncreaseGridBy(new Vector2(0, -15), () => '.');
					//Console.WriteLine($"\n{board.ToString("char+0")}\n");
					if (board.Height > 500)
					{
						board.DecreaseGridBy(0, 20);
					}

					/*if (segmentLookup.ContainsKey(segment))
					{
						Console.WriteLine($"Found duplicate segment! Prev: {segmentLookup[segment].Item1}::{segmentLookup[segment].Item2}, New: {top}::{totalRocks}");
						;
					}
					else
					{
						segmentLookup.Add(segment, (top, totalRocks));
					}*/
					//Console.WriteLine($"\n{board.ToString("char+0")}\n");
					//for (int i = board.MaxY-1; i >= board.MaxY - 20; i--)
					if(board.Height > 300){
						int i = top + 50;
						//FileStream stream = File.OpenWrite(path);
						//stream.Position = stream.Length;
						string bits = "";
						for(int x=0; x<7;x++)
						{
							char c = board[x, i, true] == '.' ? '0' : '1';
							bits += c;
						}
						int j = Convert.ToInt32(bits, 2);
						long vv = 1000000000000L - totalRocks;
						Console.WriteLine($"{((j == 117)? '!' : ' ')}-- {vv.ToString().PadLeft(14)}: {i}");
						if (j == 117)
							;
						//	Console.WriteLine($"30-- {vv.ToString().PadLeft(14)}: {i}");
						byte[] span = Encoding.ASCII.GetBytes((string.Join(' ', (Convert.ToString(j, 16)).PadLeft(2, '0').ToUpperInvariant()) + " ").ToCharArray());

						//stream.Write(span);
						//stream.Flush();
						//stream.Close();
					}
				}
				Grid piece = pentominos[pentIndex];

				int xo = 2;
				int yo = top - 4;
				bool falling = true;

				//CopyPieceTo(piece, board, xo, yo);
				//Console.WriteLine($"\n{board.ToString("char+0")}\n");
				//UnCopyPieceTo(piece, board, xo, yo);

				while (falling)
				{
					char dir = input[jetIndex];
					//Console.Write(dir);
					jetIndex = (jetIndex + 1) % jetLength;
					int push = dir == '<' ? -1 : 1;
					if (!TestCollision(board, piece, xo + push, yo))
					{
						xo += push;
					}
					if (TestCollision(board, piece, xo, yo + 1))
					{
						falling = false;
						//Console.Write('@');
					}
					else
					{
						yo++;
						//Console.Write('v');
					}
					//Console.Write('\n');
					//CopyPieceTo(piece, board, xo, yo);
					//Console.WriteLine($"\n{board.ToString("char+0")}\n");
					//UnCopyPieceTo(piece, board, xo, yo);
				}
				CopyPieceTo(piece, board, xo, yo);
				top = yo + 4;
				pentIndex = (pentIndex + 1) % 5;
			}
			/*board.TrimGrid(() => '.');
			if (board.Width != 7)
			{
				board.IncreaseGridToInclude(new Vector2(0, board.MinY + 1), () => '.');
				board.IncreaseGridToInclude(new Vector2(6, board.MinY + 1), () => '.');
			}
			board.IncreaseGridBy(new Vector2(0, -5), () => '.');
			while (board.Height > 60)
			{
				board.DecreaseGridBy(0, 20);
			}
			Console.WriteLine(board.ToString("char+0"));*/
			top = GetTopOfGrid(board, top);
			return -(top + 1);
		}
	}
}