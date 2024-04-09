using System;
using System.Reflection;

using BepInEx;

using HarmonyLib;

using DFishLoader.API;
using DFishLoader.Plugin.Core;

namespace DFishLoader.Plugin;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
	internal static IFishBinding Binding;
	
	private void Awake() {
		Binding = DFishLoader.Bind<FishBinding>(this.Logger);
		this.CreatePatches();
	}

	private void CreatePatches() {
		try {
			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
		} catch (Exception err) {
			this.Logger.LogError($"Failed to patch assembly:\n{err}");
		}
	}
}