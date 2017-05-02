﻿using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable MemberCanBePrivate.Local

namespace FixConsoleOutputForWin7
{
	internal static class VersionHelper
	{
		private static bool IsWindowsVersionOrGreater(uint majorVersion, uint minorVersion, ushort servicePackMajor)
		{
			var osvi = new OsVersionInfoEx();
			osvi.OSVersionInfoSize = (uint) Marshal.SizeOf(osvi);
			osvi.MajorVersion = majorVersion;
			osvi.MinorVersion = minorVersion;
			osvi.ServicePackMajor = servicePackMajor;

			// These constants initialized with corresponding definitions in
			// winnt.h (part of Windows SDK)
			const uint VER_MINORVERSION = 0x0000001;
			const uint VER_MAJORVERSION = 0x0000002;
			const uint VER_SERVICEPACKMAJOR = 0x0000020;
			const byte VER_GREATER_EQUAL = 3;

			var versionOrGreaterMask = VerSetConditionMask(
				VerSetConditionMask(
					VerSetConditionMask(
						0, VER_MAJORVERSION, VER_GREATER_EQUAL),
					VER_MINORVERSION, VER_GREATER_EQUAL),
				VER_SERVICEPACKMAJOR, VER_GREATER_EQUAL);
			
			return VerifyVersionInfo(ref osvi, VER_MAJORVERSION | VER_MINORVERSION | VER_SERVICEPACKMAJOR, versionOrGreaterMask);
		}

		public static bool IsWindows10OrGreater()
		{
			return IsWindowsVersionOrGreater(_WIN32_WINNT_WIN10 & 0xff, (_WIN32_WINNT_WIN10 >> 8) & 0xff, 0);
		}

		private const ushort _WIN32_WINNT_WIN10 = 0x0A00;

		[DllImport("kernel32.dll")]
		private static extern ulong VerSetConditionMask(ulong dwlConditionMask, uint dwTypeBitMask, byte dwConditionMask);

		[DllImport("kernel32.dll")]
		private static extern bool VerifyVersionInfo([In] ref OsVersionInfoEx lpVersionInfo, uint dwTypeMask, ulong dwlConditionMask);

		[StructLayout(LayoutKind.Sequential)]
		private struct OsVersionInfoEx
		{
			public uint OSVersionInfoSize;
			public uint MajorVersion;
			public uint MinorVersion;
			public uint BuildNumber;
			public uint PlatformId;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string CSDVersion;
			public ushort ServicePackMajor;
			public ushort ServicePackMinor;
			public ushort SuiteMask;
			public byte ProductType;
			public byte Reserved;
		}
	}
}
