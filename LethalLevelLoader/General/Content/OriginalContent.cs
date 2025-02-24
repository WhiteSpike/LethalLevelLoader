using DunGen.Graph;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

namespace LethalLevelLoader
{
	public static class OriginalContent
	{
		public static StartOfRound StartOfRound => Patches.StartOfRound;
		public static RoundManager RoundManager => Patches.RoundManager;
		public static Terminal Terminal => Patches.Terminal;
		public static TimeOfDay TimeOfDay => Patches.TimeOfDay;

		//Levels

		public static List<SelectableLevel> SelectableLevels { get; internal set; } = new List<SelectableLevel>();

		public static List<SelectableLevel> MoonsCatalogue { get; internal set; } = new List<SelectableLevel>();

		//Dungeons
		public static List<DungeonFlow> DungeonFlows { get; internal set; } = new List<DungeonFlow>();

		//Items
		public static List<Item> Items { get; internal set; } = new List<Item>();
		public static List<ItemGroup> ItemGroups { get; internal set; } = new List<ItemGroup>();

		//Enemies
		public static List<EnemyType> Enemies { get; internal set; } = new List<EnemyType>();

		//Spawnable Objects
		public static List<SpawnableOutsideObject> SpawnableOutsideObjects { get; internal set; } = new List<SpawnableOutsideObject>();
		public static List<GameObject> SpawnableMapObjects { get; internal set; } = new List<GameObject>();

		//Audio
		public static List<AudioMixer> AudioMixers { get; internal set; } = new List<AudioMixer>();
		public static List<AudioMixerGroup> AudioMixerGroups { get; internal set; } = new List<AudioMixerGroup>();
		public static List<AudioMixerSnapshot> AudioMixerSnapshots { get; internal set; } = new List<AudioMixerSnapshot>();
		public static List<LevelAmbienceLibrary> LevelAmbienceLibraries { get; internal set; } = new List<LevelAmbienceLibrary>();
		public static List<ReverbPreset> ReverbPresets { get; internal set; } = new List<ReverbPreset>();

		//Terminal
		public static List<TerminalKeyword> TerminalKeywords { get; internal set; } = new List<TerminalKeyword>();
		public static List<TerminalNode> TerminalNodes { get; internal set; } = new List<TerminalNode>();
	}
}
