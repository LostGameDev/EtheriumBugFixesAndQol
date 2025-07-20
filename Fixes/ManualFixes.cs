using System;
using System.IO;

namespace BugFixesAndQoL
{
	public class ManualFixes
	{
		private string GameRootDirectory = Directory.GetCurrentDirectory();

		public bool CheckForAVProWindowsMediaX64Dll()
		{
			if (!File.Exists($"{GameRootDirectory}/Etherium_Data/Plugins/AVProWindowsMedia-x64.dll"))
			{
				Plugin.Logger.LogInfo("AVProWindowsMedia-x64.dll does not exist (This is a good thing! This means you shouldn't experience the tutorial crash.)");
				return false;
			}
			try
			{
				File.Delete($"{GameRootDirectory}/Etherium_Data/Plugins/AVProWindowsMedia-x64.dll");
				Plugin.Logger.LogInfo("Successfully removed AVProWindowsMedia-x64.dll (This fixes the tutorial crash.)");
				return true;
			}
			catch (Exception e)
			{
				Plugin.Logger.LogInfo($"Failed to remove AVProWindowsMedia-x64.dll. Please quit the game and remove it manually! Caused by: {e}");
				return false;
			}
		}
	}
}
