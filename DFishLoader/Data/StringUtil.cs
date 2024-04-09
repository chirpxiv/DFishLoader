using JetBrains.Annotations;

using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace DFishLoader.Data;

public static class StringUtil {
	public static TableReference ResolveTableReference(StringTableId tableId) => tableId switch {
		StringTableId.YARN => LanguageManager.YARN_TABLE,
		StringTableId.CHARACTER => LanguageManager.CHARACTER_TABLE,
		StringTableId.ITEM => LanguageManager.ITEM_TABLE,
		_ => LanguageManager.STRING_TABLE
	};
	
	public static StringTable ResolveTable(
		StringTableId tableId,
		[CanBeNull] Locale locale = null
	) => LocalizationSettings.StringDatabase.GetTable(
		ResolveTableReference(tableId),
		locale
	);

	public static StringTableEntry ResolveTableEntry(
		StringTableId tableId,
		string key,
		[CanBeNull] Locale locale = null
	) => ResolveTable(tableId, locale).GetEntry(key);

	public static LocalizedString Create(
		StringTableId tableId,
		string key,
		string value,
		[CanBeNull] Locale locale = null
	) {
		var tableRef = ResolveTableReference(tableId);
		var table = LocalizationSettings.StringDatabase.GetTable(tableRef, locale);
		table.AddEntry(key, value);
		return new LocalizedString(tableRef, key);
	}
	
	public static LocalizedString CreateString(
		string key,
		string value,
		[CanBeNull] Locale locale = null
	) => Create(StringTableId.DEFAULT, key, value, locale);

	public static LocalizedString CreateItem(
		string key,
		string value,
		[CanBeNull] Locale locale = null
	) => Create(StringTableId.ITEM, key, value, locale);
	
	public static LocalizedString CreateYarn(
		string key,
		string value,
		[CanBeNull] Locale locale = null
	) => Create(StringTableId.YARN, key, value, locale);
	
	public static LocalizedString CreateChara(
		string key,
		string value,
		[CanBeNull] Locale locale = null
	) => Create(StringTableId.CHARACTER, key, value, locale);
}