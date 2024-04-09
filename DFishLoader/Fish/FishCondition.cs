using System;

namespace DFishLoader.Fish;

[Flags]
public enum FishCondition {
	NONE = 0x00,
	
	IN_DAY = 0x01,
	IN_NIGHT = 0x02,
	ALL_TIMES = IN_DAY | IN_NIGHT,
	
	BY_ROD = 0x04,
	BY_NET = 0x08,
	BY_POT = 0x10,
	ALL_METHODS = BY_ROD | BY_NET | BY_POT,
	
	ALL = ALL_TIMES | ALL_METHODS
}