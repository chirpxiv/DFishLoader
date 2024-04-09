using System;
using System.Linq;
using System.Collections.Generic;

using BepInEx.Logging;

using DFishLoader.API;
using DFishLoader.Fish;

namespace DFishLoader.Plugin.Core;

public class FishBinding : IFishBinding {
	private readonly IFishMediator _mediator;
	private readonly ManualLogSource _logger;

	private readonly FishValidator _validator = new();
	
	private readonly List<FishLoaderInfo> _loaders = new();

	public FishBinding(
		ManualLogSource logger,
		IFishMediator mediator
	) {
		this._mediator = mediator;
		this._logger = logger;
	}
	
	public void Load() {
		this._logger.LogInfo("Loading fish data...");
		
		this._validator.Initialize();

		var loaders = this._mediator.GetLoaders()
			.Where(loader => !loader.IsLoaded)
			.ToList();
		loaders.ForEach(this.FirePrimaryLoad);
		this._loaders.AddRange(loaders.Where(this.FirePostLoad));
	}

	private void FirePrimaryLoad(FishLoaderInfo loader) {
		try {
			this._mediator.Load(loader);
			var ct = loader.ItemData.Count;
			this._logger.LogInfo($"Loaded {ct} item{(ct == 1 ? "" : "s")} from '{loader.Name}'");
		} catch (Exception err) {
			this._logger.LogError($"Failed to load fish from '{loader.Name}':\n{err}");
		}
	}

	private bool FirePostLoad(FishLoaderInfo loader) {
		try {
			return this._mediator.PostLoad(loader);
		} catch (Exception err) {
			this._logger.LogInfo($"Error in post-load handler from '{loader.Name}':\n{err}");
			return false;
		}
	}

	public IEnumerable<FishItemData> GetData() => this._loaders.SelectMany(this.GetData);

	private IEnumerable<FishItemData> GetData(FishLoaderInfo loader) {
		foreach (var fish in loader.ItemData) {
			if (this._validator.Validate(fish, out var status)) {
				yield return fish;
				continue;
			}

			var issues = Enum.GetValues(typeof(FishValidator.Status))
				.Cast<FishValidator.Status>()
				.Skip(1)
				.Where(value => (value != FishValidator.Status.FATAL || status == value) && status.HasFlag(value))
				.Select(this._validator.GetStatusText)
				.ToArray();

			var ct = issues.Length;
			var issuesText = string.Join("\t- ", issues);
			this._logger.LogWarning($"{loader.Name}: '{fish.id}' has {ct} validation issue{(ct == 1 ? "" : "s")}:\n\t- {issuesText}");

			if (status.HasFlag(FishValidator.Status.FATAL)) {
				this._logger.LogWarning($"{loader.Name}: Cannot load '{fish.id}' due to validation error(s), skipping.");
				continue;
			}
			
			yield return fish;
		}
	}

	public void HandleLoadGrid(SerializableGrid grid) {
		var itemManager = GameManager.Instance.ItemManager;
		if (!itemManager._hasLoadedItems) {
			this._logger.LogWarning("Items failed to load, cannot validate grid contents!");
			return;
		}

		grid.spatialItems.RemoveAll(item => {
			if (item.GetItemData<SpatialItemData>() != null)
				return false;
			this._logger.LogWarning($"Item '{item.id}' is invalid, removing from storage.");
			return true;
		});
	}
}