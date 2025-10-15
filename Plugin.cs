using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EtheriumLib.UI;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BugFixesAndQoL;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Etherium.exe")]
[BepInDependency("EtheriumLib")]
public class Plugin : BaseUnityPlugin
{
	public static new ManualLogSource Logger;

	// Assets Folder
	public static string AssetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

	// SWF Filepaths
	public static string CustomMainMenuSWF = Path.Combine(AssetsFolderPath, "CustomMainMenu.swf");

	// Config Values
	public static ConfigEntry<bool> configEndTurnOnInvade;
	public static ConfigEntry<string> configNatFacilitatorIP;
	public static ConfigEntry<int> configNatFacilitatorPort;
	public static ConfigEntry<bool> configDebugLogging;

	[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members")]
	private void Awake()
	{
		// Plugin startup logic
		Logger = base.Logger;
		Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! Version: {MyPluginInfo.PLUGIN_VERSION}");
		CreateConfigs();

		// Change Nat Facilitator IP and Port
		Network.natFacilitatorIP = configNatFacilitatorIP.Value;
		Network.natFacilitatorPort = configNatFacilitatorPort.Value;

		// Initialize Harmony
		Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
		harmony.PatchAll();

		// Remove AVProWindowsMedia-x64.dll if it exists
		Logger.LogInfo("Checking for AVProWindowsMedia-x64.dll...");
		ManualFixes.CheckForAVProWindowsMediaX64Dll();

		// Check if the game is running in DirectX 9
		CheckGraphicsAPI();

		// Load Custom UI
		LoadCustomUI();
	}

	private void CreateConfigs()
	{
		if (!Config.TryGetEntry("General", "EndTurnOnInvade", out configEndTurnOnInvade))
		{
			configEndTurnOnInvade = Config.Bind("General", "EndTurnOnInvade", true,
												"Ends turn after invasion.");
		}

		if (!Config.TryGetEntry("Multiplayer", "NatFacilitatorIP", out configNatFacilitatorIP))
		{
			configNatFacilitatorIP = Config.Bind("Multiplayer", "NatFacilitatorIP", "127.0.0.1",
												 "IP for Nat Facilitator Server.");
		}

		if (!Config.TryGetEntry("Multiplayer", "NatFacilitatorPort", out configNatFacilitatorPort))
		{
			configNatFacilitatorPort = Config.Bind("Multiplayer", "NatFacilitatorPort", 50005,
												   "Port for Nat Facilitator Server.");
		}

		if (!Config.TryGetEntry("Debug", "EnableDebugLogging", out configDebugLogging))
		{
			configDebugLogging = Config.Bind("Debug", "EnableDebugLogging", false,
												"Enable Debug Logging for the mod (Will cause a LOT of messages in console!).");
		}
	}

	private void CheckGraphicsAPI()
	{
		string graphicsAPI = SystemInfo.graphicsDeviceVersion;
		Plugin.Logger.LogInfo($"Detected graphics API: {graphicsAPI}");

		// Check if the string contains exactly "Direct3D 9"
		if (graphicsAPI.Contains("Direct3D 9"))
		{
			Logger.LogInfo("Game is running in DirectX 9.");
		}
		else if (graphicsAPI.Contains("Direct3D 11"))
		{
			Logger.LogWarning($"Warning: Game is running in DirectX 11 (or a higher feature level) and not DirectX 9. Detected API: {graphicsAPI}. Please put '-force-d3d9' in your game's launch arguments on Steam to launch the game in DirectX 9 mode. This helps fix a severe crashing issue.");
		}
		else
		{
			Logger.LogWarning($"Warning: Game is not running in DirectX 9! Detected API: {graphicsAPI}. Please put '-force-d3d9' in your game's launch arguments on Steam to launch the game in DirectX 9 mode. This helps fix a severe crashing issue.");
		}
	}

	private void LoadCustomUI()
	{
		ScaleformGFxUtils.RegisterOverride(this, "MainMenu.swf", CustomMainMenuSWF);
	}
}