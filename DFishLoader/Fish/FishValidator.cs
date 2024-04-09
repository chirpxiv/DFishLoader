using System;
using System.Collections.Generic;

using UnityEngine.Localization;

using DFishLoader.Data;

namespace DFishLoader.Fish;

public class FishValidator {
	public bool IsInitialized { get; private set; }

	private LocalizedString _invalidName = new();
	private LocalizedString _invalidDesc = new();

	public void Initialize() {
		if (this.IsInitialized) return;

		this._invalidName = StringUtil.CreateItem("dfl.invalidName", "???");
		this._invalidDesc = StringUtil.CreateItem("dfl.invalidDesc", string.Empty);

		this.IsInitialized = true;
	}

	public bool Validate(FishItemData data, out Status status) {
		if (data == null) {
			status = Status.FATAL;
			return false;
		}
		status = Status.NONE;
		if (data.itemNameKey == null) {
			status |= Status.MISSING_NAME;
			data.itemNameKey = this._invalidName;
		}
		if (data.itemDescriptionKey == null) {
			status |= Status.MISSING_DESC;
			data.itemDescriptionKey = this._invalidDesc;
		}
		if (data.sprite == null)
			status |= Status.MISSING_SPRITE;
		if (data.aberrations == null) {
			status |= Status.MISSING_FIELD;
			data.aberrations = new List<FishItemData>();
		}
		if (data.isAberration && data.nonAberrationParent == null)
			status |= Status.MISSING_ABERRATION_PARENT;
		return status == Status.NONE;
	}

	public string GetStatusText(Status status) => status switch {
		Status.MISSING_NAME => "Item has no name.",
		Status.MISSING_DESC => "Item has no description.",
		Status.MISSING_SPRITE => "Item has no sprite.",
		Status.MISSING_FIELD => "Item is missing an important field, which could cause bugs in-game.",
		Status.MISSING_ABERRATION_PARENT => "Item is an aberration but has no parent.",
		Status.FATAL => "Item is completely invalid.",
		_ => status.ToString()
	};

	[Flags]
	public enum Status {
		NONE = 0,
		MISSING_NAME = 0x001,
		MISSING_DESC = 0x002,
		MISSING_SPRITE = 0x004,
		MISSING_FIELD = 0x008,
		MISSING_ABERRATION_PARENT = 0x010,
		FATAL = 0x100
	}
}