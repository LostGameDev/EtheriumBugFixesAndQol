using HarmonyLib;

namespace BugFixesAndQoL
{
	[HarmonyPatch(typeof(Player))]
	public static class PlayerPatches
	{
		[HarmonyPrefix]
		[HarmonyPatch("buyArmy")]
		public static void Player_buyArmy_Prefix(Player __instance, TypeArmy _typeArmy, TypeArmyData _typeArmyData)
		{
			Army army = __instance.addArmy(_typeArmy, _typeArmyData, __instance.idPlayer, true);
			if (Plugin.configDebugLogging.Value)
			{
				if (army == null || _typeArmy == null || _typeArmyData == null)
				{
					Plugin.Logger.LogInfo($"[Player] [buyArmy] Info: {army}, {_typeArmy}, {_typeArmyData}");
				}
			}
		}
	}
}
