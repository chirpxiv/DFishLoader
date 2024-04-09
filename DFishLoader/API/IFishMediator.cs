using System.Collections.Generic;

namespace DFishLoader.API;

public interface IFishMediator {
	public IEnumerable<FishLoaderInfo> GetLoaders();

	public void Load(FishLoaderInfo loader);
	public bool PostLoad(FishLoaderInfo loader);
}