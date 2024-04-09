using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

using JetBrains.Annotations;

using HarmonyLib;

namespace DFishLoader.Plugin.Patches;

[HarmonyPatch(typeof(ItemManager))]
public class ItemManagerPatch {
	[HarmonyPrefix]
	[HarmonyPatch("Load")]
	public static void Load(ItemManager __instance) {
		Plugin.Binding.Load();
	}

	[HarmonyPostfix]
	[HarmonyPatch("OnItemDataAddressablesLoaded")]
	public static void OnItemDataAddressablesLoaded(
		ItemManager __instance,
		AsyncOperationHandle<IList<ItemData>> handle
	) {
		GetIconsFromData(handle.Result, out var fishIcon, out var junkIcon, out var trinketIcon);
        
		var data = Plugin.Binding.GetData().ToList();
		foreach (var item in data.Where(item => item.itemTypeIcon == null)) {
			item.itemTypeIcon = item.itemSubtype switch {
				ItemSubtype.FISH => fishIcon,
				ItemSubtype.DREDGE => junkIcon,
				ItemSubtype.TRINKET => trinketIcon,
				_ => null
			};
		}
		__instance.allItems.AddRange(data);
	}

	private static void GetIconsFromData(
		IEnumerable<ItemData> data,
		[CanBeNull] out Sprite fishIcon,
		[CanBeNull] out Sprite junkIcon,
		[CanBeNull] out Sprite trinketIcon
	) {
		fishIcon = null;
		junkIcon = null;
		trinketIcon = null;
		foreach (var icon in data.Where(item => item.itemTypeIcon != null).Select(item => item.itemTypeIcon)) {
			if (fishIcon == null && icon.name == "FishIcon")
				fishIcon = icon;
			if (junkIcon != null && icon.name == "JunkIcon")
				junkIcon = icon;
			if (trinketIcon != null && icon.name == "TrinketIcon")
				trinketIcon = icon;
			if (fishIcon != null && junkIcon != null && trinketIcon != null)
				break;
		}
	}
}
