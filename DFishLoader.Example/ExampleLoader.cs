using System.Collections.Generic;

using DFishLoader.API;
using DFishLoader.Data;
using DFishLoader.Fish;

namespace DFishLoader.Example;

public class ExampleLoader : IFishLoader {
	public string Name { get; } = "Maxwell";

	public IEnumerable<FishItemData> Load() {
		yield return CreateMaxwell();
	}

	private static FishItemData CreateMaxwell() {
		const string spritePath = "DFishLoader.Example.Resource.maxwell.png";
		
		return new FishBuilder("maxwell")
			.SetName("Maxwell")
			.SetDescription("meow")
			.SetSprite(TextureLoader.LoadResourceAsSprite(spritePath, 400, 400))
			.SetCondition(FishCondition.ALL)
			.SetZoneFoundIn(ZoneEnum.ALL)
			.SetHarvest(HarvestPOICategory.FISH_LARGE, HarvestableType.ABYSSAL, HarvestDifficulty.VERY_HARD)
			.SetDepthRange(DepthEnum.VERY_SHALLOW, DepthEnum.VERY_DEEP)
			.SetDimensions(ItemDimensions.Rect2x2)
			.SetSizeRange(0f, 1_000_000f)
			.GetResult();
		
	}
}