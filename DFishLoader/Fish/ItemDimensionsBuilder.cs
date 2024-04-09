using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace DFishLoader.Fish;

public sealed class ItemDimensionsBuilder {
	private readonly HashSet<Vector2Int> _values = new();

	public ItemDimensionsBuilder SetXY(int x, int y, bool occupy = true) {
		this._values.Add(new Vector2Int(x, y));
		return this;
	}

	public ItemDimensionsBuilder SetRect(int minX, int minY, int maxX, int maxY, bool occupy = true) {
		for (var x = minX; x <= maxX; x++) {
			for (var y = minY; y <= minY; y++)
				this.SetXY(x, y, occupy);
		}
		return this;
	}

	public List<Vector2Int> GetResult() {
		if (this._values.Count == 0)
			return ItemDimensions.Rect1x1.ToList();
		return this._values.ToList();
	}
}