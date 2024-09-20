using BepInEx;
using BepInEx.Logging;
using System;
using System.IO;

namespace BugFixesAndQoL;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Etherium.exe")]

public class Plugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;
    Fixes FixesInstance = new Fixes();

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        // Remove AVProWindowsMedia-x64.dll if it exists
        Logger.LogInfo("Checking for AVProWindowsMedia-x64.dll...");
        FixesInstance.CheckForAVProWindowsMediaX64Dll();
    }
}

public class Fixes
{
    private string GameRootDirectory = Directory.GetCurrentDirectory();
    public bool CheckForAVProWindowsMediaX64Dll()
    {
        if (File.Exists($"{GameRootDirectory}/Etherium_Data/Plugins/AVProWindowsMedia-x64.dll") != true)
        {
            Plugin.Logger.LogInfo("AVProWindowsMedia-x64.dll does not exist (This is a good thing! This means you wont experience the tutorial crash.)");
            return false;
        }
        try
        {
            File.Delete($"{GameRootDirectory}/Etherium_Data/Plugins/AVProWindowsMedia-x64.dll");
            Plugin.Logger.LogInfo("Sucessfully removed AVProWindowsMedia-x64.dll (This fixes the tutorial crash.)");
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogInfo($"Failed to remove AVProWindowsMedia-x64.dll due to {e} please quit the game and remove it manually!");
            return false;
        }
    }
}