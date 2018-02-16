using Plukit.Base;
using Staxel.Browser;
using System.Collections.Generic;
using System.Linq;
using System;
using Harmony;

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
			Logger.WriteLine("SunbeamController.IngameOverlayUILoaded called\r\n");
			for (int i = 0; i < this.Mods.Count; i++)
			{
				this.Mods[i].IngameOverlayUILoaded(surface);
			}
		}

		/// <summary>
		/// Enumerate all derived mods from a list of mods
		/// </summary>
		/// <param name="ModList"></param>
		private void EnumerateMods()
		{
			Type[] ModList = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
							  from assemblyType in domainAssembly.GetTypes()
							  where typeof(SunbeamMod).IsAssignableFrom(assemblyType)
							  && assemblyType.IsClass && !assemblyType.IsAbstract
							  select assemblyType).ToArray();

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
