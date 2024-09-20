using BepInEx;
using BepInEx.Logging;
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

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Initialize Harmony
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        // Remove AVProWindowsMedia-x64.dll if it exists
        Logger.LogInfo("Checking for AVProWindowsMedia-x64.dll...");
        ManualFixesInstance.CheckForAVProWindowsMediaX64Dll();

        // Check if the game is running in DirectX 9
        CheckGraphicsAPI();
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
            Plugin.Logger.LogInfo("AVProWindowsMedia-x64.dll does not exist (This is a good thing! This means you won't experience the tutorial crash.)");
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

// Harmony patches for the Movie class
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
            //Plugin.Logger.LogInfo($"Movie {movieID} is already destroyed, skipping.");
            return false; // Skip original method
        }

        // Mark this movie as destroyed
        Plugin.Logger.LogInfo($"Patching Movie Destroy for ID = {movieID}");
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
            //Plugin.Logger.LogInfo($"Movie {movieID} has already been finalized, skipping.");
            return false; // Skip original method
        }

        // Mark this movie as destroyed/finalized
        //Plugin.Logger.LogInfo($"Patching Movie Finalize for ID = {movieID}");
        destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue

        // Clean up the dictionary by removing the movie ID entry
        destroyedMovies.Remove(movieID);
        //Plugin.Logger.LogInfo($"Movie {movieID} entry removed from destroyedMovies dictionary (finalized).");

        return true; // Continue with original method
    }
}