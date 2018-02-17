using Plukit.Base;
using Staxel.Browser;
using System.Collections.Generic;
using System.Linq;
using System;
using Harmony;
using Staxel.Logic;
using Staxel.Tiles;
using Staxel.Items;
using System.IO;
using System.Reflection;

namespace Sunbeam
{
	public class SunbeamController
	{
		private const string HarmonyIdentifier = "Mod.Sunbeam";

		private static SunbeamController _instance;
		public static SunbeamController Instance
		{
			get { return _instance ?? (_instance = new SunbeamController()); }
		}

		private readonly List<SunbeamMod> Mods = new List<SunbeamMod>();

		internal HarmonyInstance HarmonyInstance { get; private set; }

		/// <summary>
		/// Initialize the SunbeamController
		/// Should only be called once, at least after the ModdingController.GameContextInitializeInit was called
		/// </summary>
		public void Initialize()
		{
			Logger.WriteLine("SunbeamController Initialize");
			this.ApplyHarmonyPatches();
			this.EnumerateMods();
		}

		private void ApplyHarmonyPatches()
		{
			this.HarmonyInstance = HarmonyInstance.Create(SunbeamController.HarmonyIdentifier);
			this.HarmonyInstance.PatchAll(typeof(SunbeamController).Assembly);
		}

		/// <summary>
		/// Called whenever the browser instance finished (re)loading the start menu
		/// </summary>
		/// <param name="surface"></param>
		public void StartMenuUILoaded(BrowserRenderSurface surface)
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].StartMenuUILoaded(surface);
			}
		}

		/// <summary>
		/// Called whenever the browser instance finished (re)loading the ingame overlay
		/// </summary>
		public void IngameOverlayUILoaded(BrowserRenderSurface surface)
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].IngameOverlayUILoaded(surface);
			}
		}

		/// <summary>
		/// GameContextInitializeBefore, called before Staxel's ModdingController
		/// </summary>
		public void GameContextInitializeBefore()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].GameContextInitializeBefore();
			}
		}

		/// <summary>
		/// GameContextInitializeAfter, called before Staxel's ModdingController
		/// </summary>
		public void GameContextInitializeAfter()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].GameContextInitializeAfter();
			}
		}

		/// <summary>
		/// GameContextDeinitialize, called before Staxel's ModdingController
		/// </summary>
		public void GameContextDeinitialize()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].GameContextDeinitialize();
			}
		}

		/// <summary>
		/// GameContextReloadBefore, called before Staxel's ModdingController
		/// </summary>
		public void GameContextReloadBefore()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].GameContextReloadBefore();
			}
		}

		/// <summary>
		/// GameContextReloadAfter, called before Staxel's ModdingController
		/// </summary>
		public void GameContextReloadAfter()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].GameContextReloadAfter();
			}
		}

		/// <summary>
		/// Dispose, called before Staxel's ModdingController
		/// </summary>
		public void Dispose()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].Dispose();
			}
		}

		/// <summary>
		/// ClientContextInitializeBefore, called before Staxel's ModdingController
		/// </summary>
		public void ClientContextInitializeBefore()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].ClientContextInitializeBefore();
			}
		}

		/// <summary>
		/// ClientContextInitializeAfter, called before Staxel's ModdingController
		/// </summary>
		public void ClientContextInitializeAfter()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].ClientContextInitializeAfter();
			}
		}

		/// <summary>
		/// ClientContextDeinitialize, called before Staxel's ModdingController
		/// </summary>
		public void ClientContextDeinitialize()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].ClientContextDeinitialize();
			}
		}

		/// <summary>
		/// ClientContextReloadBefore, called before Staxel's ModdingController
		/// </summary>
		public void ClientContextReloadBefore()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].ClientContextReloadBefore();
			}
		}

		/// <summary>
		/// ClientContextReloadAfter, called before Staxel's ModdingController
		/// </summary>
		public void ClientContextReloadAfter()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].ClientContextReloadAfter();
			}
		}

		/// <summary>
		/// CleanupOldSession, called before Staxel's ModdingController
		/// </summary>
		public void CleanupOldSession()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].CleanupOldSession();
			}
		}

		/// <summary>
		/// UniverseUpdateBefore, called before Staxel's ModdingController
		/// </summary>
		public void UniverseUpdateBefore(Universe universe, Timestep step)
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].UniverseUpdateBefore(universe, step);
			}
		}

		/// <summary>
		/// UniverseUpdateAfter, called before Staxel's ModdingController
		/// </summary>
		public void UniverseUpdateAfter()
		{
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].UniverseUpdateAfter();
			}
		}

		/// <summary>
		/// CanPlaceTile, called through SunbeamHook: IModhookV2
		/// </summary>
		public bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			bool result = true;
			for (int i = 0; i < this.Mods.Count; i++)
			{
				result &= this.Mods[i].CanPlaceTile(entity, location, tile, accessFlags);
			}
			return result;
		}

		/// <summary>
		/// CanReplaceTile, called through SunbeamHook: IModhookV2
		/// </summary>
		public bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			bool result = true;
			for (int i = 0; i < this.Mods.Count; i++)
			{
				result &= this.Mods[i].CanReplaceTile(entity, location, tile, accessFlags);
			}
			return result;
		}

		/// <summary>
		/// CanRemoveTile, called through SunbeamHook: IModhookV2
		/// </summary>
		public bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
		{
			bool result = true;
			for (int i = 0; i < this.Mods.Count; i++)
			{
				result &= this.Mods[i].CanRemoveTile(entity, location, accessFlags);
			}
			return result;
		}

		/// <summary>
		/// Enumerate all derived mods from a list of mods
		/// </summary>
		/// <param name="ModList"></param>
		private void EnumerateMods()
		{
			DirectoryInfo location = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
			HashSet<string> suppress = AtomicFile.ReadStream(Path.Combine(location.FullName, "loader.suppress"), false).ReadAllText().Split('\n', '\r', ' ', '\t')
				.ToSet();
			suppress.RemoveAll((string x) => x.Trim().Length == 0);
			List<Type> ModList = new List<Type>();

			foreach (FileInfo AssemblyDll in location.GetFiles("*.dll").Concat(location.GetFiles("*.exe")))
			{
				HashSet<string> source = suppress;
				Func<string, bool> predicate = (string suppression) => AssemblyDll.FullName.IndexOf(suppression, StringComparison.OrdinalIgnoreCase) != -1;
				if (!source.Any(predicate) && File.Exists(Path.ChangeExtension(AssemblyDll.FullName, ".mod")))
				{
					try
					{
						foreach (Type AssemblyType in Assembly.LoadFrom(AssemblyDll.FullName).GetTypes())
						{
							if (!AssemblyType.IsAbstract && AssemblyType.IsClass && typeof(SunbeamMod).IsAssignableFrom(AssemblyType))
							{
								ModList.Add(AssemblyType);
							}
						}
					}
					catch (ReflectionTypeLoadException e)
					{}
				}
			}

			Logger.WriteLine("SunbeamController.EnumerateMods: Found " + ModList.Count + " mods.");

			foreach (Type ModType in ModList)
			{
				try
				{
					SunbeamMod Mod = (SunbeamMod)Activator.CreateInstance(ModType);
					Mod.ApplyHarmonyPatches();
					Mod.Initialize();

					Logger.WriteLine("SunbeamController.EnumerateMods: Loaded mod '" + Mod.ModIdentifier + "'");
					this.Mods.Add(Mod);
				}
				catch (Exception e)
				{
					Logger.WriteLine("SunbeamController.EnumerateMods: Exception thrown - " + e.ToString());
				}
			}

		}
	}
}
