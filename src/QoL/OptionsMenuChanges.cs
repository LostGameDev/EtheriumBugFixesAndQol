using System;
using HarmonyLib;
using UnityEngine;

namespace BugFixesAndQoL
{
	/*
	//All this code should theoretically work the problem is I cant figure out how to actually modify the game's SWF files meaning that i cant make the neccessary changes in the UI itself for this to actually work, until i can figure that out this code is useless :(
	//And because i dont know how to edit the UI that will make adding new content to the game (Either in this mod in the form of Quality of life stuffs or other mods in the future) very difficult if not impossible to do effectively.
	//It might be worth trying to rewrite the entire game's UI from scratch so that it doesnt rely on Flash/Scaleform GFx anymore, but that will take time and is the last resort.
	[HarmonyPatch]
	public static class OptionsMenuChanges
	{
		//Variables and replacement methods
		private static OptionsMenuChanges.WINDOWMODE e_fullScreen = WINDOWMODE.FULLSCREEN;
		private static OptionsMenuChanges.WINDOWMODE previousFullScreen;

		public enum WINDOWMODE
		{
			WINDOWED,
			FULLSCREEN
		}

		public static OptionsMenuChanges.WINDOWMODE getFullScreen()
		{
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[OptionsMenuChanges] [getFullScreen] Returned e_fullScreen, e_fullScreen is currently {e_fullScreen}.");
			}
			return e_fullScreen;
		}

		public static void setFullScreen(OptionsMenuChanges.WINDOWMODE _e)
		{
			e_fullScreen = _e;
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[OptionsMenuChanges] [setFullScreen] Set e_fullScreen to {e_fullScreen}.");
			}
		}

		//Patches
		[HarmonyPatch(typeof(OptionManager), "DefaultParam")]
		[HarmonyPostfix]
		public static void OptionManager_DefaultParam_Postfix(OptionManager __instance)
		{
			e_fullScreen = OptionsMenuChanges.WINDOWMODE.FULLSCREEN;
			__instance.SavePref();
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[OptionManager] [DefaultParam] Set e_fullScreen to {e_fullScreen}.");
			}
		}

		[HarmonyPatch(typeof(OptionManager), "SavePref")]
		[HarmonyPostfix]
		public static void OptionManager_SavePref_Postfix()
		{
			PlayerPrefs.SetInt("fullScreen", (int)e_fullScreen);
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"[OptionManager] [SavePref] PlayerPref 'fullScreen' set to {e_fullScreen} as int ({(int)e_fullScreen}).");
			}
		}

		[HarmonyPatch(typeof(OptionManager), "LoadPref")]
		[HarmonyPostfix]
		public static void OptionManager_LoadPref_Postfix(OptionManager __instance)
		{
			if (!PlayerPrefs.HasKey("defilementCamMouse"))
			{
				__instance.DefaultParam();
			}
			else
			{
				e_fullScreen = (OptionsMenuChanges.WINDOWMODE)PlayerPrefs.GetInt("fullScreen");
				if (Plugin.configDebugLogging.Value)
				{
					Plugin.Logger.LogInfo($"[OptionManager] [LoadPref] Set e_fullScreen to PlayerPref 'fullScreen' e_fullScreen is now {e_fullScreen}");
				}
				//May need to call loadBinding() again, untested
			}
		}

		[HarmonyPatch(typeof(OptionManager), "refreshGrapicOptions")]
		[HarmonyPrefix]
		public static bool OptionManager_refreshGrapicOptions_Prefix(OptionManager __instance, bool _b_refreshResolution)
		{
			var instanceTraverse = Traverse.Create(__instance);
			OptionManager.RESOLUTION e_resolution = instanceTraverse.Field("e_resolution").GetValue<OptionManager.RESOLUTION>();
			OptionManager.LEVEL e_texture = instanceTraverse.Field("e_texture").GetValue<OptionManager.LEVEL>();
			OptionManager.LEVEL e_lod = instanceTraverse.Field("e_lod").GetValue<OptionManager.LEVEL>();
			OptionManager.LEVEL e_shadow = instanceTraverse.Field("e_shadow").GetValue<OptionManager.LEVEL>();
			bool b_canMoveWindow = instanceTraverse.Field("b_canMoveWindow").GetValue<bool>();

			if (Screen.fullScreen != Convert.ToBoolean((int)getFullScreen()))
			{
				Screen.fullScreen = Convert.ToBoolean((int)getFullScreen());
			}
			if (_b_refreshResolution)
			{
				switch (e_resolution)
				{
					case OptionManager.RESOLUTION.RESOLUTION_1024_768:
						Screen.SetResolution(1024, 768, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1280_720:
						Screen.SetResolution(1280, 720, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1280_800:
						Screen.SetResolution(1280, 800, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1280_1024:
						Screen.SetResolution(1280, 1024, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1360_768:
						Screen.SetResolution(1360, 768, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1366_768:
						Screen.SetResolution(1366, 768, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1440_900:
						Screen.SetResolution(1440, 900, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1600_900:
						Screen.SetResolution(1600, 900, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1600_1200:
						Screen.SetResolution(1600, 1200, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1680_1050:
						Screen.SetResolution(1680, 1050, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1920_1080:
						Screen.SetResolution(1920, 1080, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1920_1200:
						Screen.SetResolution(1920, 1200, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_1920_2160:
						Screen.SetResolution(1920, 2160, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_2048_1152:
						Screen.SetResolution(2048, 1152, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_2560_1080:
						Screen.SetResolution(2560, 1080, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_2560_1440:
						Screen.SetResolution(2560, 1440, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_2560_1600:
						Screen.SetResolution(2560, 1600, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_2880_1800:
						Screen.SetResolution(2880, 1800, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_3440_1440:
						Screen.SetResolution(3440, 1440, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_3840_2160:
						Screen.SetResolution(3840, 2160, Convert.ToBoolean((int)getFullScreen()));
						break;
					case OptionManager.RESOLUTION.RESOLUTION_5760_1080:
						Screen.SetResolution(5760, 1080, Convert.ToBoolean((int)getFullScreen()));
						break;
				}
				if (Game.b_isPlayingTuto && GUIManager.scaleformInGame != null && GUIManager.scaleformInGame.gfx_gameTutorial != null)
				{
					GUIManager.scaleformInGame.gfx_gameTutorial.refreshPosRappelWithReso();
				}
				instanceTraverse.Field("b_canMoveWindow").SetValue(true);
			}
			QualitySettings.vSyncCount = ((!__instance.getVSynch()) ? 0 : 1);
			switch (e_texture)
			{
				case OptionManager.LEVEL.LEVEL_LOW:
					QualitySettings.SetQualityLevel(0, true);
					break;
				case OptionManager.LEVEL.LEVEL_MEDIUM:
					QualitySettings.SetQualityLevel(1, true);
					break;
				case OptionManager.LEVEL.LEVEL_HIGHT:
					QualitySettings.SetQualityLevel(2, true);
					break;
				case OptionManager.LEVEL.LEVEL_EXTREM:
					QualitySettings.SetQualityLevel(3, true);
					break;
			}
			__instance.setAntialiasingOnCam();
			switch (e_lod)
			{
				case OptionManager.LEVEL.LEVEL_LOW:
					QualitySettings.lodBias = 0.7f;
					break;
				case OptionManager.LEVEL.LEVEL_MEDIUM:
					QualitySettings.lodBias = 1f;
					break;
				case OptionManager.LEVEL.LEVEL_HIGHT:
				case OptionManager.LEVEL.LEVEL_EXTREM:
					QualitySettings.lodBias = 2f;
					break;
			}
			switch (e_shadow)
			{
				case OptionManager.LEVEL.LEVEL_LOW:
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 0f;
					break;
				case OptionManager.LEVEL.LEVEL_MEDIUM:
					QualitySettings.shadowProjection = ShadowProjection.CloseFit;
					QualitySettings.shadowCascades = 0;
					QualitySettings.shadowDistance = 500f;
					break;
				case OptionManager.LEVEL.LEVEL_HIGHT:
					QualitySettings.shadowProjection = ShadowProjection.StableFit;
					QualitySettings.shadowCascades = 2;
					QualitySettings.shadowDistance = 500f;
					break;
				case OptionManager.LEVEL.LEVEL_EXTREM:
					QualitySettings.shadowProjection = ShadowProjection.StableFit;
					QualitySettings.shadowCascades = 4;
					QualitySettings.shadowDistance = 500f;
					break;
			}
			if (!Convert.ToBoolean((int)getFullScreen()) && b_canMoveWindow)
			{
				CoroutineManager.Instance.StartCoroutine(__instance.launchSetWindowPosition(0.3f));
				instanceTraverse.Field("b_canMoveWindow").SetValue(false);
			}
			return false;
		}

		[HarmonyPatch(typeof(GFXOption), "setVideo", new Type[0])]
		[HarmonyPrefix]
		public static bool GFXOption_setVideo_Prefix(GFXOption __instance)
		{
			Scaleform.Value gfx_movie = Traverse.Create(__instance).Field("gfx_movie").GetValue<Scaleform.Value>();

			gfx_movie.Invoke("clearArray", new object[0]);
			string[] array0 = new string[]
			{
				//TODO: Implement proper localisation
				"Fullscreen",
				"Windowed"
			};
			string[] array1 = new string[]
			{
				"1024x768",
				"1280x720",
				"1280x800",
				"1280x1024",
				"1360x768",
				"1366x768",
				"1440x900",
				"1600x900",
				"1600x1200",
				"1680x1050",
				"1920x1080",
				"1920x1200",
				"1920x2160",
				"2048x1152",
				"2560x1080",
				"2560x1440",
				"2560x1600",
				"2880x1800",
				"3440x1440",
				"3840x2160",
				"5760x1080"
			};
			string[] array2 = new string[]
			{
				Localisation.getText("MenuOption_LvL_Graphique_Low"),
				Localisation.getText("MenuOption_LvL_Graphique_Medium"),
				Localisation.getText("MenuOption_LvL_Graphique_Hight"),
				Localisation.getText("MenuOption_LvL_Graphique_Extrem")
			};
			string[] array3 = new string[]
			{
				Localisation.getText("MenuOption_Anti_Aliasing_No"),
				Localisation.getText("MenuOption_Anti_Aliasing_X2"),
				Localisation.getText("MenuOption_Anti_Aliasing_X4"),
				Localisation.getText("MenuOption_Anti_Aliasing_X8")
			};
			for (int o = 0; o < array0.Length; o++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					0,
					array0[o]
				});
			}
			for (int i = 0; i < array1.Length; i++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					1,
					array1[i]
				});
			}
			for (int j = 0; j < array2.Length; j++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					2,
					array2[j]
				});
			}
			for (int k = 0; k < array3.Length; k++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					3,
					array3[k]
				});
			}
			for (int l = 0; l < array2.Length; l++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					4,
					array2[l]
				});
			}
			for (int m = 0; m < array2.Length; m++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					5,
					array2[m]
				});
			}
			for (int n = 0; n < array2.Length; n++)
			{
				gfx_movie.Invoke("addDropText", new object[]
				{
					6,
					array2[n]
				});
			}
			gfx_movie.Invoke("setAllDropDown", new object[0]);
			gfx_movie.Invoke("setVideo", new object[]
			{
				(int)OptionsMenuChanges.getFullScreen(),
				(int)OptionManager.getInstance.getResolution(),
				OptionManager.getInstance.getVSynch(),
				(int)OptionManager.getInstance.getTexture(),
				(int)OptionManager.getInstance.getAntialiasing(),
				(int)OptionManager.getInstance.getTerrainDetail(),
				(int)OptionManager.getInstance.getLod(),
				(int)OptionManager.getInstance.getShadow()
			});
			return false;
		}

		[HarmonyPatch(typeof(GFXOption), "setVideo",
			new Type[] {
				typeof(string), typeof(string), typeof(string), typeof(string),
				typeof(string), typeof(string), typeof(string), typeof(string)
			})]
		[HarmonyPrefix]
		public static bool GFXOption_setVideo_Prefix(GFXOption __instance, string _s1, string _s2, string _s3, string _s4, string _s5, string _s6, string _s7, string _s8)
		{
			OptionsMenuChanges.setFullScreen((OptionsMenuChanges.WINDOWMODE)__instance.sToi(_s1));
			OptionManager.getInstance.setResolution((OptionManager.RESOLUTION)__instance.sToi(_s2));
			OptionManager.getInstance.setVSynch(_s3 == "true");
			OptionManager.getInstance.setTexture((OptionManager.LEVEL)__instance.sToi(_s4));
			OptionManager.getInstance.setAntialiasing((OptionManager.ANTIALIASING)__instance.sToi(_s5));
			OptionManager.getInstance.setTerrainDetail((OptionManager.LEVEL)__instance.sToi(_s6));
			OptionManager.getInstance.setLod((OptionManager.LEVEL)__instance.sToi(_s7));
			OptionManager.getInstance.setShadow((OptionManager.LEVEL)__instance.sToi(_s8));
			return false;
		}

		[HarmonyPatch(typeof(GFXOption), "clickGeneral")]
		[HarmonyPrefix]
		public static bool GFXOption_clickGeneral_Prefix(GFXOption __instance, int _i, string _s, int i_currentOption)
		{
			var instanceTraverse = Traverse.Create(__instance);
			OptionManager.RESOLUTION previousResolution = instanceTraverse.Field("previousResolution").GetValue<OptionManager.RESOLUTION>();
			Scaleform.Value gfx_movie = instanceTraverse.Field("gfx_movie").GetValue<Scaleform.Value>();
			GFXOption.CALLING_GUI callingGui = instanceTraverse.Field("callingGui").GetValue<GFXOption.CALLING_GUI>();

			switch (_i)
			{
				case 0:
					if (i_currentOption == 3)
					{
						__instance.applyChangeKeycode();
						OptionManager.getInstance.saveBinding();
					}
					else
					{
						string[] array = _s.Split('%');
						__instance.setGame(array[0], array[1], array[2]);
						__instance.setVideo(array[3], array[4], array[5], array[6], array[7], array[8], array[9], array[10]);
						__instance.setSound(array[11], array[12], array[13], array[14], array[15], array[16], array[17], array[18]);
						bool b_refreshResolution = previousResolution != OptionManager.getInstance.getResolution() || previousFullScreen != OptionsMenuChanges.getFullScreen();
						if (Game.isInGame())
						{
							OptionManager.getInstance.ApplyIG(b_refreshResolution);
							instanceTraverse.Field("previousResolution").SetValue(OptionManager.getInstance.getResolution());
						}
						else
						{
							OptionManager.getInstance.ApplyInMenu(b_refreshResolution);
							instanceTraverse.Field("previousResolution").SetValue(OptionManager.getInstance.getResolution());
						}
						OptionManager.getInstance.SavePref();
					}
					break;
				case 1:
					__instance.SetActive(false);
					if (callingGui == GFXOption.CALLING_GUI.MAIN_MENU)
					{
						GUIManager.scaleformMainMenu.getgfxMainMenu().SetActive(true);
					}
					else if (callingGui == GFXOption.CALLING_GUI.PAUSE_MENU_CAMPAIGN)
					{
						GUIManager.scaleformCampaign.gfx_gamePauseMenu.HideOptions();
					}
					else
					{
						GUIManager.scaleformInGame.gfx_gamePauseMenu.HideOptions();
					}
					break;
				case 2:
					OptionManager.getInstance.DefaultParam();
					__instance.setGame();
					__instance.setSound();
					__instance.setVideo();
					__instance.setBinding();
					EtheriumUtilities.DebugLog("refresh");
					gfx_movie.Invoke("refreshCurrentTabBinding", new object[0]);
					break;
			}
			return false;
		}

		[HarmonyPatch(typeof(GFXOption), "SetActive")]
		[HarmonyPrefix]
		public static bool GFXOption_SetActive_Prefix(GFXOption __instance, bool _b_active)
		{
			var instanceTraverse = Traverse.Create(__instance);
			instanceTraverse.Field("b_isActive").SetValue(_b_active);

			if (_b_active)
			{
				instanceTraverse.Field("previousResolution").SetValue(OptionManager.getInstance.getResolution());
				OptionsMenuChanges.previousFullScreen = OptionsMenuChanges.getFullScreen();
			}

			__instance.SetVisible(_b_active);
			return false;
		}

		[HarmonyPatch(typeof(GUIOption), "resetDataFromOptionManager")]
		[HarmonyPostfix]
		public static void GUIOption_resetDataFromOptionManager_Postfix()
		{
			e_fullScreen = OptionsMenuChanges.getFullScreen();
		}

		[HarmonyPatch(typeof(GUIOption), "applyDataToOptionManager")]
		[HarmonyPostfix]
		public static void GUIOption_applyDataToOptionManager_Postfix()
		{
			OptionsMenuChanges.setFullScreen(e_fullScreen);
			OptionManager.getInstance.SavePref();
			EtheriumAudioManager.refreshOptionVolume(true);
		}
	}
	*/
}
