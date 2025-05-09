using HarmonyLib;
using System.Collections.Generic;

namespace BugFixesAndQoL
{
	[HarmonyPatch]
	public static class MoviePatches
	{
		// Dictionary to track destroyed state for each MovieID
		private static Dictionary<long, bool> destroyedMovies = new Dictionary<long, bool>();

		[HarmonyPatch(typeof(Scaleform.Movie), "Destroy")]
		[HarmonyPrefix]
		public static bool Movie_Destroy_Prefix(Scaleform.Movie __instance)
		{
			long movieID = __instance.GetID();

			// Check if this movie has already been destroyed
			if (destroyedMovies.TryGetValue(movieID, out bool isDestroyed) && isDestroyed)
			{
				// Skip the method if already destroyed
				if (Plugin.configDebugLogging.Value)
				{
					Plugin.Logger.LogInfo($"Movie {movieID} is already destroyed, skipping.");
				}
				destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue
				return false; // Skip original method
			}

			// Mark this movie as destroyed
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"Patching Movie Destroy for ID = {movieID}");
			}
			destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue
			return true; // Continue with original method
		}

		[HarmonyPatch(typeof(Scaleform.Movie), "Finalize")]
		[HarmonyPrefix]
		public static bool Movie_Finalize_Prefix(Scaleform.Movie __instance)
		{
			long movieID = __instance.GetID();

			// Check if this movie has already been finalized
			if (destroyedMovies.TryGetValue(movieID, out bool isDestroyed) && isDestroyed)
			{
				if (Plugin.configDebugLogging.Value)
				{
					Plugin.Logger.LogInfo($"Movie {movieID} has already been finalized, skipping.");
				}
				return false; // Skip original method
			}

			// Mark this movie as destroyed/finalized
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"Patching Movie Finalize for ID = {movieID}");
			}
			destroyedMovies[movieID] = true; // Mark as destroyed to prevent double-destroy issue

			// Clean up the dictionary by removing the movie ID entry
			destroyedMovies.Remove(movieID);
			if (Plugin.configDebugLogging.Value)
			{
				Plugin.Logger.LogInfo($"Movie {movieID} entry removed from destroyedMovies dictionary (finalized).");
			}
			return true; // Continue with original method
		}
	}
}
