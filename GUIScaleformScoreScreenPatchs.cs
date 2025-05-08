using HarmonyLib;

namespace BugFixesAndQoL
{
	[HarmonyPatch(typeof(GUIScaleformScoreScreen))]
	public static class GUIScaleformScoreScreenPatchs
	{
		[HarmonyPrefix]
		[HarmonyPatch("Start")]
		public static bool GUIScaleformScoreScreen_Start_Prefix(GUIScaleformScoreScreen __instance)
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
		public static bool GUIScaleformScoreScreen_Update_Prefix(GUIScaleformScoreScreen __instance, bool ___b_init, bool ___b_isStarted)
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
}
