using HarmonyLib;
using EtheriumLib.Debug;
using UnityEngine;

namespace BugFixesAndQoL
{
	[HarmonyPatch]
	public static class ConquestSave
	{
        [HarmonyPatch(typeof(Game), "startTheGame")]
        [HarmonyPrefix]
        public static void Game_startTheGame_Prefix()
        {
            //InspectorDebugUtils.PrintHierarchy();
            InspectorDebugUtils.PrintComponents(GameObject.Find("MultiplayerManager"));
        }
    }
}
