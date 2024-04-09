using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace DFishLoader.API;

public record FishLoaderInfo {
	internal readonly IFishLoader Client;

	public readonly string Name;
	public readonly int Priority;
	
	public bool IsEnabled { get; set; }
	public bool IsLoaded { get; internal set; }
	
	public readonly List<FishItemData> ItemData = new();

	[CanBeNull] public Action<FishLoaderInfo> Loaded;

	internal FishLoaderInfo(IFishLoader client, string name, int priority = 0) {
		this.Client = client;
		this.Name = name;
		this.Priority = priority;
	}

	internal void InvokeLoaded() => this.Loaded?.Invoke(this);
}