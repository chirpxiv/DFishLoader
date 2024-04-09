using System;
using System.Linq;
using System.Collections.Generic;

using FluffyUnderware.DevTools.Extensions;

using DFishLoader.API;

namespace DFishLoader;

public static class DFishLoader {
	private readonly static List<FishLoaderInfo> _clients = new();
	private readonly static IFishMediator _mediator = new Mediator();

	public static FishLoaderInfo Register(
		IFishLoader client,
		int priority = 0
	) {
		var name = client.Name;
		if (_clients.Any(cl => cl.Name == name))
			throw new Exception($"Attempted to register name already in use: '{client.Name}'");
		
		var info = new FishLoaderInfo(client, name, priority) { IsEnabled = true };
		_clients.Add(info);
		return info;
	}
	
	public static T Bind<T>(
		params object[] param
	) where T : IFishBinding {
		return (T)Activator.CreateInstance(typeof(T), param.Add(_mediator));
	}

	private class Mediator : IFishMediator {
		public IEnumerable<FishLoaderInfo> GetLoaders() => _clients.AsReadOnly()
			.OrderBy(cl => cl.Priority);

		public void Load(FishLoaderInfo loader) {
			if (loader.IsLoaded) return;
			var items = loader.Client.Load();
			loader.ItemData.AddRange(items);
			loader.IsLoaded = true;
		}

		public bool PostLoad(FishLoaderInfo loader) {
			try {
				if (loader.IsLoaded)
					loader.InvokeLoaded();
				return loader.IsLoaded;
			} catch {
				loader.IsLoaded = false;
				throw;
			}
		}
	}
}