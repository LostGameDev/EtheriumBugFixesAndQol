using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BugFixesAndQoL;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Etherium.exe")]
public class Plugin : BaseUnityPlugin
{
	public static new ManualLogSource Logger;
	ManualFixes ManualFixesInstance = new ManualFixes();

	//Config Values
	public static ConfigEntry<bool> configEndTurnOnInvade;
	public static ConfigEntry<string> configNatFacilitatorIP;
    public static ConfigEntry<int> configNatFacilitatorPort;
	public static ConfigEntry<bool> configDebugLogging;


	private void Awake()
	{
		// Plugin startup logic
		Logger = base.Logger;
		Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded! Version: {MyPluginInfo.PLUGIN_VERSION}.");
		CreateConfigs();

		// Change Nat Facilitator IP and Port
		Network.natFacilitatorIP = configNatFacilitatorIP.Value;
		Network.natFacilitatorPort = configNatFacilitatorPort.Value;

		// Initialize Harmony
		Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
		harmony.PatchAll();

		// Remove AVProWindowsMedia-x64.dll if it exists
		Logger.LogInfo("Checking for AVProWindowsMedia-x64.dll...");
		ManualFixesInstance.CheckForAVProWindowsMediaX64Dll();

		// Check if the game is running in DirectX 9
		CheckGraphicsAPI();
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
}

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
			Plugin.Logger.LogInfo($"Failed to remove AVProWindowsMedia-x64.dll due to {e}. Please quit the game and remove it manually!");
			return false;
		}
	}
}

[HarmonyPatch]
public static class MoviePatches
{
	// Dictionary to track destroyed state for each MovieID
	private static Dictionary<long, bool> destroyedMovies = new Dictionary<long, bool>();

	// Patch for the Movie.Destroy method
	[HarmonyPatch(typeof(Scaleform.Movie), "Destroy")]
	[HarmonyPrefix]
	public static bool Destroy_Prefix(Scaleform.Movie __instance)
	{
		long movieID = __instance.GetID();

		// Check if this movie has already been destroyed
		if (destroyedMovies.TryGetValue(movieID, out bool isDestroyed) && isDestroyed)
		{
			// Skip the method if already destroyed
			if (Plugin.configDebugLogging.Value)
            {
				Plugin.Logger.LogInfo($"Movie {movieID} is already destroyed, skipping.");
			}
			return false; // Skip original method
		}

		// Mark this movie as destroyed
		if (Plugin.configDebugLogging.Value)
        {
			Plugin.Logger.LogInfo($"Patching Movie Destroy for ID = {movieID}");
        }
		destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue
		return true; // Continue with original method
	}

	// Patch for the Movie.Finalize method
	[HarmonyPatch(typeof(Scaleform.Movie), "Finalize")]
	[HarmonyPrefix]
	public static bool Finalize_Prefix(Scaleform.Movie __instance)
	{
		long movieID = __instance.GetID();

		// Check if this movie has already been finalized
		if (destroyedMovies.TryGetValue(movieID, out bool isDestroyed) && isDestroyed)
		{
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"Movie {movieID} has already been finalized, skipping.");
			}
			return false; // Skip original method
		}

		// Mark this movie as destroyed/finalized
		if (Plugin.configDebugLogging.Value)
		{
			Plugin.Logger.LogInfo($"Patching Movie Finalize for ID = {movieID}");
		}
		destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue

		// Clean up the dictionary by removing the movie ID entry
		destroyedMovies.Remove(movieID);
		if (Plugin.configDebugLogging.Value)
        {
			Plugin.Logger.LogInfo($"Movie {movieID} entry removed from destroyedMovies dictionary (finalized).");
		}		
		return true; // Continue with original method
	}
}

[HarmonyPatch(typeof(StateFSM_Deploy), "onUpdate")]
public static class StateFSM_Deploy_onUpdate_Patch
{
	[HarmonyPrefix]
	public static bool Prefix(StateFSM_Deploy __instance)
	{
		// Cache the agent field to avoid calling reflection multiple times
		ArmyAgent agent = Traverse.Create(__instance).Field("agent").GetValue<ArmyAgent>();
		if (agent == null)
		{
			Plugin.Logger.LogWarning("Error: Agent is null in StateFSM_Deploy.onUpdate.");
			return false; // Skip original method to prevent potential crashes
		}

		// Check if getAttackLieutenant or mission is null before proceeding
		var lieutenant = agent.getAttackLieutenant();
		if (lieutenant == null)
		{
			Plugin.Logger.LogWarning("Error: getAttackLieutenant is null in StateFSM_Deploy.onUpdate.");
			agent.findNewLieutenant(); // Attempt to find a new lieutenant if none exists
			lieutenant = agent.getAttackLieutenant();

			if (lieutenant == null)
			{
				Plugin.Logger.LogError("Failed to find a new lieutenant. Skipping onUpdate.");
				return false; // Skip if still null after attempting to find a new one
			}
		}

		if (lieutenant.mission == null)
		{
			Plugin.Logger.LogWarning("Error: mission is null in StateFSM_Deploy.onUpdate.");
			return false; // Skip if still null after attempting to find a new one
		}

		Colony landingColony = lieutenant.mission.landingColony;
		if (landingColony == null || landingColony.isIsolated())
		{
			lieutenant.mission.searchBestLandingColony();
		}

		// Additional logic remains unchanged
		return true; // Continue with the original method if no issues
	}
}

[HarmonyPatch]
public static class FSMStatePatches
{
	// Patch for HFSMState.onUpdate
	[HarmonyPatch(typeof(HFSMState), "onUpdate")]
	[HarmonyPrefix]
	public static bool HFSMState_onUpdate_Prefix(HFSMState __instance)
	{
		if (__instance.currentState == null)
		{
			Plugin.Logger.LogError("HFSMState.onUpdate: currentState is null, skipping update.");
			return false; // Skip the original method to avoid the error
		}
		return true; // Continue with the original method if currentState is valid
	}

	// Patch for FiniteStateMachine.update
	[HarmonyPatch(typeof(FiniteStateMachine), "update")]
	[HarmonyPrefix]
	public static bool FiniteStateMachine_update_Prefix(FiniteStateMachine __instance)
	{
		if (__instance.currentState == null)
		{
			Plugin.Logger.LogError("FiniteStateMachine.update: currentState is null, skipping update.");
			return false; // Skip the original method to avoid the error
		}
		return true; // Continue with the original method if currentState is valid
	}
}

[HarmonyPatch(typeof(GUIScaleformScoreScreen))]
public static class GUIScaleformScoreScreen_Patch
{
	[HarmonyPrefix]
	[HarmonyPatch("Start")]
	public static bool Start_Prefix(GUIScaleformScoreScreen __instance)
	{
		if (Plugin.configDebugLogging.Value)
        {
			Plugin.Logger.LogInfo("[GUIScaleformScoreScreen] [Start] Prefix patch initiated.");
		}

		// Ensure InitParams is properly initialized
		if (__instance.InitParams == null)
		{
			Plugin.Logger.LogError("[GUIScaleformScoreScreen] InitParams is null during Start. Skipping Start.");
			return false; // Skip the original method to prevent crashes
		}

		if (Plugin.configDebugLogging.Value)
        {
			Plugin.Logger.LogInfo("[GUIScaleformScoreScreen] InitParams is valid. Proceeding with Start.");
		}
		return true; // Continue with the original method
	}

	[HarmonyPrefix]
	[HarmonyPatch("Update")]
	public static bool Update_Prefix(GUIScaleformScoreScreen __instance, bool ___b_init, bool ___b_isStarted)
	{
		if (Plugin.configDebugLogging.Value)
        {
			// Log the current initialization status
			Plugin.Logger.LogInfo($"[GUIScaleformScoreScreen] Update - b_init: {___b_init}, b_isStarted: {___b_isStarted}");
		}			

		// Ensure that b_init and b_isStarted are true before calling Update
		if (!___b_init || !___b_isStarted)
		{
			Plugin.Logger.LogWarning("[GUIScaleformScoreScreen] Skipping Update due to uninitialized state.");
			return false; // Skip the original method to avoid issues
		}

		if (Plugin.configDebugLogging.Value)
        {
			Plugin.Logger.LogInfo("[GUIScaleformScoreScreen] Update proceeding normally.");
		}
		return true; // Continue with the original method
	}
}

[HarmonyPatch(typeof(CampaignManager))]
public static class EndTurnOnInvadePatch
{

	[HarmonyPostfix]
	[HarmonyPatch("Awake")]
	public static void Awake_Postfix(CampaignManager __instance, MultiplayerScript ___multiScript)
	{
		if (Plugin.configEndTurnOnInvade.Value)
		{
			// Ensure we only trigger when going back to the campaign
			if (___multiScript.b_iAmPlayingCampaign)
			{
				__instance.nextPlayer();
			}
		}
	}
} 