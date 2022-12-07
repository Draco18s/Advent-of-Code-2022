using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2022 {
	internal static class DaySeven
	{
		static long allSize = 0;
		const int Max = 100000;

		public class FileFolder
		{
			public Dictionary<string, int> files;
			public Dictionary<string,FileFolder> folders;
			public FileFolder parent;
			public long cachedSize = -1;

			public FileFolder()
			{
				files = new Dictionary<string,int>();
				folders = new Dictionary<string, FileFolder>();
			}

			public FileFolder(FileFolder par)
			{
				files = new Dictionary<string, int>();
				folders = new Dictionary<string, FileFolder>();
				parent = par;
			}

			public long GetTotalSize()
			{
				if (cachedSize > 0) return cachedSize;
				long sum = 0;
				foreach(var f in files)
				{
					sum += f.Value;
				}
				foreach(var fold in folders)
				{
					sum += fold.Value.GetTotalSize();
				}
				cachedSize = sum;
				if(sum <= Max)
				{
					allSize += sum;
				}
				return sum;
			}

			internal List<FileFolder> FindBiggerThan(long need)
			{
				List<FileFolder> options = new List<FileFolder>();
				foreach (var fold in folders)
				{
					if(fold.Value.GetTotalSize() >= need)
					{
						options.Add(fold.Value);
					}
					options.AddRange(fold.Value.FindBiggerThan(need));
				}
				return options;
			}
		}

		internal static FileFolder root = new FileFolder();
		internal static FileFolder currentDir = root;

		internal static long Part1(string input) {
			string[] lines = input.Split('\n');
			long sum = 0;

			for(int i=0; i < lines.Length; i++)
			{
				string line = lines[i];
				//Console.WriteLine(line);
				if (line[0] == '$')
				{
					i = ParseCommand(line, lines, i);
				}
			}

			foreach (var fold in root.folders)
			{
				sum += fold.Value.GetTotalSize();
			}
			foreach (var file in root.files)
			{
				long ts = file.Value;
				sum += ts;
			}
			if (sum <= Max)
			{
				allSize += sum;
			}

			return allSize;
		}

		private static int ParseCommand(string line, string[] lines, int index)
		{
			string[] parts = line.Split(' ');
			if(parts[1] == "cd")
			{
				if(parts[2] == "/")
				{
					currentDir = root;
					return index;
				}
				if(parts[2] == "..")
				{
					if(currentDir.parent != null)
					{
						currentDir = currentDir.parent;
					}
					return index;
				}
				else
				{
					if (currentDir.folders.ContainsKey(parts[2]))
						currentDir = currentDir.folders[parts[2]];
				}
				return index;
			}
			if(parts[1] == "ls")
			{
				index++;
				while (index < lines.Length && lines[index].Length > 0 && lines[index][0] != '$')
				{
					//Console.WriteLine(lines[index]);
					string[] subParts = lines[index].Split(' ');
					switch (subParts[0])
					{
						case "dir":
							if(!currentDir.folders.ContainsKey(subParts[1]))
								currentDir.folders.Add(subParts[1], new FileFolder(currentDir));
							break;
						default:
							if (!currentDir.files.ContainsKey(subParts[1]))
								currentDir.files.Add(subParts[1], int.Parse(subParts[0]));
							break;
					}
					index++;
				}
				return index-1;
			}
			//Console.WriteLine("BBB");
			return index-1;
		}

		internal static long Part2(string input) {
			int sum = 0;
			long used = root.GetTotalSize();
			long total = 70000000;
			//need 30000000
			long free = total - used;
			long need = 30000000 - free;

			List<FileFolder> avail = root.FindBiggerThan(need);
			avail.Sort((x, y) => x.GetTotalSize().CompareTo(y.GetTotalSize()));

			return avail[0].GetTotalSize();
		}
	}
}
//8381165
//24933642

//ddbcvhqr 2195372