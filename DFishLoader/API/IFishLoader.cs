using System.Collections.Generic;

namespace DFishLoader.API;

public interface IFishLoader {
	public string Name { get; }

	public IEnumerable<FishItemData> Load();
}