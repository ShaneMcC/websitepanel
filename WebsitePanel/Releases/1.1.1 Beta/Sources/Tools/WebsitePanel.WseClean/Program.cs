using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WseClean
{
	class Program
	{
		// Methods
		private static void CleanXmlTypeDefinitions(string pathToFile)
		{
			Regex regex = new Regex(@"\[([^\]]{2,})\]");
			string[] strArray = ReadWseFileContent(pathToFile);
			StringBuilder builder = new StringBuilder();
			StringBuilder builder2 = new StringBuilder();
			for (int i = 0; i < strArray.Length; i++)
			{
				string str = strArray[i];
				builder2.AppendLine(str);
				if (!str.Contains("///"))
				{
					if (regex.IsMatch(str))
					{
						if (str.IndexOf("XmlTypeAttribute") > -1)
						{
                            int braces = 0;
                            bool started = false;
							while (!started || braces > 0)
							{
                                if (str.IndexOf("}") != -1)
                                {
                                    braces--;
                                    started = true;
                                }
                                else if (str.IndexOf("{") != -1)
                                {
                                    braces++;
                                    started = true;
                                }

								i++;
								str = strArray[i];
							}
							builder2.Remove(0, builder2.Length);
						}
					}
					else if (builder2.Length > 0)
					{
						builder.Append(builder2.ToString());
						builder2.Remove(0, builder2.Length);
					}
				}
			}
			File.WriteAllText(pathToFile, builder.ToString());
		}

		private static void Main(string[] args)
		{
			if ((args != null) && (args.Length > 0))
			{
				CleanXmlTypeDefinitions(args[0]);
			}
			else
			{
				ShowCleanerUsage();
			}
		}

		private static string[] ReadWseFileContent(string pathToFile)
		{
			if (!File.Exists(pathToFile))
			{
				throw new FileNotFoundException("Sorry but it seems that file you've specified doesn't exist");
			}
			return File.ReadAllLines(pathToFile);
		}

		private static void ShowCleanerUsage()
		{
			Console.WriteLine("WseCleaner usage guidelines...");
			Console.ReadKey();
		}
	}
}
