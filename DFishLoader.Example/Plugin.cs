using BepInEx;

namespace DFishLoader.Example;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin {
	private void Awake() {
		var loader = new ExampleLoader();
		DFishLoader.Register(loader);
		
		this.Logger.LogInfo("Loaded example DFishLoader plugin.");
	}
}