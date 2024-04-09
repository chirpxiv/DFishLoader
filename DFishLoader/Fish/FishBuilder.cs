using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Localization;

using DFishLoader.Data;

namespace DFishLoader.Fish;

public sealed class FishBuilder {
	private readonly FishItemData _itemData;
	private readonly string _id;

	public FishBuilder(
		string id,
		[CanBeNull] string objectName = null
	) {
		this._itemData = ScriptableObject.CreateInstance<FishItemData>();
		this._id = this._itemData.id = id;
		this._itemData.name = objectName ?? id;
		this._itemData.itemType = ItemType.GENERAL;
		this._itemData.itemSubtype = ItemSubtype.FISH;
		this._itemData.damageMode = DamageMode.DESTROY;
		this._itemData.canBeDiscardedByPlayer = true;
		this.SetAberration(false);
		this._itemData.aberrations = new List<FishItemData>();
		this._itemData.dimensions = new List<Vector2Int>();
		this._itemData.cellsExcludedFromDisplayingInfection = new List<Vector2Int>();
		this._itemData.itemOwnPrerequisites = new List<OwnedItemResearchablePrerequisite>();
		this._itemData.platformSpecificSpriteOverrides = new Dictionary<Platform, Sprite>();
		this._itemData.itemInsaneTitleKey = new LocalizedString();
		this._itemData.itemInsaneDescriptionKey = new LocalizedString();
		this._itemData.dialogueNodeSpecificDescription = new LocalizedString();
	}

	public FishBuilder SetName(string name) {
		var value = StringUtil.CreateItem($"{this._id}.name", name);
		return this.SetName(value);
	}

	public FishBuilder SetName(LocalizedString name) {
		this._itemData.itemNameKey = name;
		return this;
	}

	public FishBuilder SetDescription(string description) {
		var value = StringUtil.CreateString($"{this._id}.description", description);
		return this.SetDescription(value);
	}

	public FishBuilder SetDescription(LocalizedString description) {
		this._itemData.itemDescriptionKey = description;
		return this;
	}

	public FishBuilder SetCondition(
		FishCondition flags,
		bool canBait = true,
		bool canInfect = true,
		int minWorldPhase = 0,
		bool locationHiddenUntilCaught = false,
		bool affectedByFishingSustain = true,
		bool canBeReplacedWithResearchItem = false
	) {
		this._itemData.day = flags.HasFlag(FishCondition.IN_DAY);
		this._itemData.night = flags.HasFlag(FishCondition.IN_NIGHT);
		this._itemData.canBeCaughtByRod = flags.HasFlag(FishCondition.BY_ROD);
		this._itemData.canBeCaughtByNet = flags.HasFlag(FishCondition.BY_NET);
		this._itemData.canBeCaughtByPot = flags.HasFlag(FishCondition.BY_POT);
		this._itemData.canAppearInBaitBalls = canBait;
		this._itemData.canBeInfected = canInfect;
		this._itemData.minWorldPhaseRequired = minWorldPhase;
		this._itemData.locationHiddenUntilCaught = locationHiddenUntilCaught;
		this._itemData.affectedByFishingSustain = affectedByFishingSustain;
		this._itemData.canBeReplacedWithResearchItem = canBeReplacedWithResearchItem;
		return this;
	}
	
	public FishBuilder SetHarvest(
		HarvestPOICategory category,
		HarvestableType type,
		HarvestDifficulty difficulty,
		HarvestMinigameType minigame = HarvestMinigameType.FISHING_RADIAL,
		float weight = 1.0f,
		int perSpotMin = 1,
		int perSpotMax = 1,
		bool regenerate = true
	) {
		this._itemData.harvestPOICategory = category;
		this._itemData.harvestableType = type;
		this._itemData.harvestDifficulty = difficulty;
		this._itemData.harvestMinigameType = minigame;
		this._itemData.harvestItemWeight = weight;
		this._itemData.perSpotMin = perSpotMin;
		this._itemData.perSpotMax = perSpotMax;
		this._itemData.regenHarvestSpotOnDestroy = regenerate;
		return this;
	}

	public FishBuilder SetDimensions(
		IEnumerable<Vector2Int> dimensions,
		[CanBeNull] IEnumerable<Vector2Int> excludedFromInfection = null
	) {
		this._itemData.dimensions.Clear();
		this._itemData.dimensions.AddRange(dimensions);
		if (excludedFromInfection != null) {
			this._itemData.cellsExcludedFromDisplayingInfection.Clear();
			this._itemData.cellsExcludedFromDisplayingInfection.AddRange(excludedFromInfection);
		}
		return this;
	}

	public FishBuilder SetSizeRange(float min, float max) {
		this._itemData.minSizeCentimeters = Math.Max(Math.Min(min, max), 0.0f);
		this._itemData.maxSizeCentimeters = Math.Max(Math.Max(max, min), 0.0f);
		return this;
	}

	public FishBuilder SetAberration(bool isAberration = true) {
		this._itemData.isAberration = isAberration;
		this._itemData.itemColor = isAberration ? FishColors.AberrationItem : FishColors.NormalItem;
		this._itemData.tooltipTextColor = isAberration ? FishColors.AberrationText : FishColors.NormalText;
		this._itemData.tooltipNotesColor = FishColors.Notes;
		return this;
	}

	public FishBuilder SetZoneFoundIn(ZoneEnum zone, bool value = true) {
		if (value)
			this._itemData.zonesFoundIn |= zone;
		else
			this._itemData.zonesFoundIn &= ~zone;
		return this;
	}

	public FishBuilder SetDepthRange(DepthEnum? min, DepthEnum? max) {
		this._itemData.minDepth = min ?? DepthEnum.VERY_SHALLOW;
		this._itemData.maxDepth = max ?? DepthEnum.VERY_DEEP;
		this._itemData.hasMinDepth = min != null;
		this._itemData.hasMaxDepth = max != null;
		return this;
	}

	public FishBuilder SetSprite(Sprite sprite, Platform platform = Platform.ALL) {
		if (platform == Platform.ALL)
			this._itemData.sprite = sprite;
		else
			this._itemData.platformSpecificSpriteOverrides.Add(platform, sprite);
		return this;
	}

	public FishBuilder SetItemTypeIcon(Sprite sprite) {
		this._itemData.itemTypeIcon = sprite;
		return this;
	}

	public FishBuilder SetAdditionalNote(LocalizedString value) {
		this._itemData.additionalNoteKey = value;
		return this;
	}
	
	public FishBuilder AddAberration(FishItemData aberration) {
		this._itemData.aberrations.Add(aberration);
		aberration.nonAberrationParent ??= this._itemData;
		return this;
	}

	public FishBuilder AddAberrations(IEnumerable<FishItemData> aberrations) {
		foreach (var item in aberrations)
			this.AddAberration(item);
		return this;
	}

	public FishItemData GetResult() {
		this._itemData.hasAdditionalNote = this._itemData.additionalNoteKey != null;
		return this._itemData;
	}
}