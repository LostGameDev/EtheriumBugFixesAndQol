using HarmonyLib;

namespace BugFixesAndQoL
{
	[HarmonyPatch]
	public static class FSMStatePatches
	{
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
}
