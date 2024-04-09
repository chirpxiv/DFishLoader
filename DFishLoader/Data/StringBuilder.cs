using System;
using UnityEngine.Localization;

namespace DFishLoader.Data;

// TODO: Extend this for registering with multiple localizations.

public sealed class StringBuilder {
	private StringTableId _tableId;
	private string _key = string.Empty;
	
	public StringBuilder(
		StringTableId tableId
	) {
		this.TableId = tableId;
	}

	public StringTableId TableId
	{
		get => this._tableId;
		set => this.SetTable(value);
	}

	public StringBuilder SetTable(StringTableId tableId) {
		this._tableId = tableId;
		return this;
	}

	public StringBuilder SetKey(string key) {
		this._key = key;
		return this;
	}

	public LocalizedString Create(string text) {
		if (this._key.Length == 0)
			throw new Exception("Invalid key for string.");
		return this.Create(this._key, text);
	}

	public LocalizedString Create(string key, string text) {
		if (this._key != key) this.SetKey(key);
		return StringUtil.Create(this._tableId, key, text);
	}
}