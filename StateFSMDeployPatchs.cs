using HarmonyLib;

namespace BugFixesAndQoL
{
	[HarmonyPatch(typeof(StateFSM_Deploy), "onUpdate")]
	public static class StateFSMDeployPatchs
	{
		[HarmonyPrefix]
		public static bool StateFSMDeploy_onUpdate_Prefix(StateFSM_Deploy __instance)
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

			return true; // Continue with the original method if no issues
		}
	}
}
