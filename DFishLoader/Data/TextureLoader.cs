using System;
using System.IO;
using System.Reflection;

using JetBrains.Annotations;

using UnityEngine;

namespace DFishLoader.Data;

// Thanks to LunaCapra for her original implementation, which was used as reference:
// https://github.com/SlopCrew/SlopCrew/blob/cad98d0d73537e6a40b47cfdd7e061f82197e9af/SlopCrew.Plugin/UI/TextureLoader.cs

public static class TextureLoader {
	public static Texture2D LoadResourceAsTexture(
		string path,
		int width,
		int height,
		bool generateMipMaps = true,
		TextureWrapMode wrapMode = TextureWrapMode.Clamp,
		[CanBeNull] Assembly assembly = null
	) {
		assembly ??= Assembly.GetCallingAssembly();
		
		var stream = assembly.GetManifestResourceStream(path);
		if (stream == null) throw new Exception($"Could not load texture path: {path}");

		var reader = new BinaryReader(stream);
		var buffer = reader.ReadBytes((int)stream.Length);

		var texture = new Texture2D(width, height, TextureFormat.RGBA32, generateMipMaps, false) {
			wrapMode = wrapMode
		};
		texture.LoadImage(buffer);
		texture.Apply();
		return texture;
	}

	public static Sprite LoadResourceAsSprite(
		string path,
		int width,
		int height,
		float pivotX = 0.5f,
		float pivotY = 0.5f,
		[CanBeNull] Assembly assembly = null
	) {
		assembly ??= Assembly.GetCallingAssembly();
		
		var texture = LoadResourceAsTexture(path, width, height, false, assembly: assembly);
		return Sprite.Create(
			texture,
			new Rect(0.0f, 0.0f, texture.width, texture.height),
			new Vector2(pivotX, pivotY),
			100.0f
		);
	}
}