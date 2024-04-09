using System.Collections.Generic;

using UnityEngine;

namespace DFishLoader.Fish;

public static class ItemDimensions {
	public readonly static List<Vector2Int> Rect1x1 = new() { new(0,0) };
	public readonly static List<Vector2Int> Rect2x1 = new() { new(0,0), new(1, 0) };
	public readonly static List<Vector2Int> Rect3x1 = new() { new(0,0), new(1, 0), new(2, 0) };
	public readonly static List<Vector2Int> Rect2x2 = new() { new(0,0), new(1, 0), new(0, 1), new(1, 1) };
	public readonly static List<Vector2Int> Rect3x2 = new() { new(0,0), new(1, 0), new(2, 0), new(0,1), new(1, 1), new(2, 1) };
	public readonly static List<Vector2Int> ShapeL = new() { new(0, 0), new(0, 1), new(1, 0) };
}