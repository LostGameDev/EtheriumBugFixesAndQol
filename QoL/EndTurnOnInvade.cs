using HarmonyLib;

namespace BugFixesAndQoL
{
	[HarmonyPatch(typeof(CampaignManager))]
	public static class EndTurnOnInvade
	{
		[HarmonyPostfix]
		[HarmonyPatch("Awake")]
		public static void CampaignManager_Awake_Postfix(CampaignManager __instance, MultiplayerScript ___multiScript)
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
}