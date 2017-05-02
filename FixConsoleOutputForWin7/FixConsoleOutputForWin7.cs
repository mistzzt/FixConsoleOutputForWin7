using System;
using System.Text;
using Terraria;
using TerrariaApi.Server;

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
				return;
			}

			if (Environment.OSVersion.Version >= new Version(6, 0) && VersionHelper.IsWindows10OrGreater())
			{
				WriteLine("检测到系统版本为Windows 10；不需要运行插件。", ConsoleColor.Cyan);
				return;
			}

			try
			{
				Console.OutputEncoding = Encoding.GetEncoding("GBK");
				WriteLine("控制台中文显示修复完毕！", ConsoleColor.Cyan);
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
