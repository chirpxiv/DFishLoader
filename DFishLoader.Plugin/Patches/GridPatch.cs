using HarmonyLib;

namespace DFishLoader.Plugin.Patches;

[HarmonyPatch(typeof(SerializableGrid))]
public class GridPatch {
	[HarmonyPrefix]
	[HarmonyPatch("Init")]
	public static void Init(SerializableGrid __instance, GridConfiguration gridConfiguration, bool clear = false) {
		if (!clear) Plugin.Binding.HandleLoadGrid(__instance);
	}
}