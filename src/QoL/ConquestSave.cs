using HarmonyLib;
using Scaleform;
using System.Reflection;
using EtheriumLib.Debug;

namespace BugFixesAndQoL
{
    [HarmonyPatch]
    public static class ConquestSave
    {
        /*
        public static GFXGameLoad gfx_gameLoad;

        [HarmonyPatch(typeof(GUIScaleformInGame), "CreateGFXs")]
        [HarmonyPrefix]
        public static bool GUIScaleformInGame_CreateGFXs_Prefix(GUIScaleformInGame __instance)
        {
            InspectorDebugUtils.PrintHierarchy();

            MultiplayerScript multiplayerScript = new MultiplayerScript();
            var sfMgrField = typeof(SFCamera).GetField("SFMgr", BindingFlags.NonPublic | BindingFlags.Static);
            var sfMgr = sfMgrField.GetValue(null);
            var instanceTraverse = Traverse.Create(__instance);
            bool b_init = instanceTraverse.Field("b_init").GetValue<bool>();

            EtheriumUtilities.DebugLog("b_init ? " + b_init);
            if (!b_init)
            {
                EtheriumUtilities.DebugLog("Create GFX InGamePauseMenu");
                SFMovieCreationParams sfmovieCreationParams = SFCamera.CreateMovieCreationParams("InGamePauseMenu.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                if (!multiplayerScript.b_iWasPlayingCampaign)
                {
                    if (Plugin.configDebugLogging.Value)
                    {
                        Plugin.Logger.LogInfo("[ConquestSave] [CreateGFXs] Not Multiplayer, showing save/load button.");
                    }
                    __instance.gfx_gamePauseMenu = new GFXGamePauseMenu((SFManager)sfMgr, sfmovieCreationParams, true);
                }
                else
                {
                    if (Plugin.configDebugLogging.Value)
                    {
                        Plugin.Logger.LogInfo("[ConquestSave] [CreateGFXs] Multiplayer, not showing save/load button.");
                    }
                    __instance.gfx_gamePauseMenu = new GFXGamePauseMenu((SFManager)sfMgr, sfmovieCreationParams, false);
                }

                EtheriumUtilities.DebugLog("Create GFX InGameForces");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("InGameForces.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                __instance.gfx_gameForces = new GFXGameForces((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Create GFX InGameRessources");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("InGameRessources.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                __instance.gfx_gameRessources = new GFXGameRessources((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Create GFX InGameMinimap");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("InGameMinimap.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                __instance.gfx_gameMinimap = new GFXGameMinimap((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Create GFX MenuOtpions");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("MenuOtpions.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                __instance.gfx_option = new GFXOption((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Create GFX NewAlerteManager");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("NewAlerteManager.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                __instance.gfx_alertManager = new GFXAlertManager((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Create GFX load_campaign");
                sfmovieCreationParams = SFCamera.CreateMovieCreationParams("load_campaign.swf", 1);
                sfmovieCreationParams.TheScaleModeType = ScaleModeType.SM_ExactFit;
                sfmovieCreationParams.IsInitFirstFrame = true;
                //Note: Depth line may not be needed, untested 
                //sfmovieCreationParams.Depth = (int)GUIScaleformCampaign.GFXS.GFX_GAMELOAD;
                gfx_gameLoad = new GFXGameLoad((SFManager)sfMgr, sfmovieCreationParams);
                EtheriumUtilities.DebugLog("Finish to create GFX!!");
                b_init = true;
            }
            return false;
        }

        [HarmonyPatch(typeof(GUIScaleformInGame), "DestroyGFXs")]
        [HarmonyPostfix]
        public static void GUIScaleformInGame_DestroyGFXs_Postfix(GUIScaleformInGame __instance)
        {
            EtheriumUtilities.DebugLog("Destroy GFX gfx_gameLoad?");
            if (gfx_gameLoad != null)
            {
                EtheriumUtilities.DebugLog("Destroy GFX gfx_gameLoad!");
                gfx_gameLoad.Destroy();
                gfx_gameLoad = null;
            }
        }

        [HarmonyPatch(typeof(GUIScaleformInGame), "isAllGUIDestroyed")]
        [HarmonyPostfix]
        public static void GUIScaleformInGame_isAllGUIDestroyed_Postfix(GUIScaleformInGame __instance, ref bool __result)
        {
            __result = __instance.gfx_gameForces == null && __instance.gfx_gameRessources == null && __instance.gfx_gameMinimap == null && __instance.gfx_gamePauseMenu == null && __instance.gfx_option == null && __instance.gfx_gameTutorial == null && __instance.gfx_alertManager == null && __instance.gfx_migratingLoading == null && __instance.gfx_loading == null && gfx_gameLoad == null;
        }
        */
    }
}
