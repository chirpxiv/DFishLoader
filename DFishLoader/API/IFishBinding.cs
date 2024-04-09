using System.Collections.Generic;

namespace DFishLoader.API;

public interface IFishBinding {
	public void Load();

	public IEnumerable<FishItemData> GetData();
}