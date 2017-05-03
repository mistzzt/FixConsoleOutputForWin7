using System;
using System.Linq;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using System.Management;

namespace FixConsoleOutputForWin7
{
	[ApiVersion(2, 1)]
	public class FixConsoleOutputForWin7 : TerrariaPlugin
	{
		public override string Name => GetType().Name;

		public override Version Version => GetType().Assembly.GetName().Version;

		public override string Author => "MistZZT";

		public FixConsoleOutputForWin7(Main game) : base(game)
		{
			Order = int.MinValue;
		}

		public override void Initialize()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				WriteLine("Other OS detected; do not use this plugin.", ConsoleColor.DarkCyan);
				return;
			}

			var name = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem")
							.Get()
							.Cast<ManagementObject>()
							.Select(x => x.GetPropertyValue("Caption")).FirstOrDefault()?.ToString() ?? "Unknown";

			string versionString;
			if (!name.StartsWith("Microsoft Windows") || string.IsNullOrWhiteSpace(versionString = name.Split(' ').FirstOrDefault(x => x.All(char.IsDigit))))
			{
				WriteLine("Couldn't detect version; the version string is: " + name, ConsoleColor.Red);
				return;
			}

			var windowsServer = name.Contains("Server");
			var version = int.Parse(versionString);
			if (version < 2008 && windowsServer || (version < 7 || version == 10) && !windowsServer)
			{
				WriteLine("Terraria Server on your Windows version doesn't need be fixed.", ConsoleColor.DarkCyan);
				return;
			}

			try
			{
				Console.OutputEncoding = Encoding.Default;
				WriteLine("Fixed console output successfully! OSVersion: " + name, ConsoleColor.Cyan);
			}
			catch
			{
				// ignored
			}
		}

		private static void WriteLine(string content, ConsoleColor color)
		{
			var clr = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine(content);
			Console.ForegroundColor = clr;
		}
	}
}
