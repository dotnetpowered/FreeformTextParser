using System;
using System.Collections.Generic;
using System.IO;
using FreeformTextParser;
using Newtonsoft.Json;

namespace Parser
{
	class Program
	{
		public static void Main (string[] args)
		{
			DirectoryInfo directoryInfo = new("../../../../samples/");
			foreach (var f in directoryInfo.GetFiles("*.json"))
			{
				Console.WriteLine ($"File: {f.Name}\n-----------------------");
                using var defFile = new StreamReader(File.OpenRead(f.FullName));
				using var s = File.OpenRead(f.FullName.Replace(".json", ".txt"));
                var parser = new TextParser(new JsonTextReader(defFile));
                Console.WriteLine(parser.Parse(s));
            }
		}
	}
}
