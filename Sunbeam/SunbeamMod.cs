using Harmony;
using Plukit.Base;
using Staxel.Browser;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Tiles;
using Sunbeam.Core;
using System;

namespace Sunbeam
{
	public abstract class SunbeamMod
	{
		/// <summary>
		/// The mod identifier should be the same as the mod's name/folder name
		/// For now only used in junction with the HarmonyPrefix as a identifier and used
		/// for the AssetLoader to locate the mod directory
		/// </summary>
		public abstract string ModIdentifier { get; }

		/// <summary>
		/// Placeholder for the mod settings from the .mod file
		/// </summary>
		protected Blob Settings { get; private set; }

		/// <summary>
		/// The prefix for the Harmony patching next to the assembly name
		/// </summary>
		public const string HarmonyPrefix = "Mod.Sunbeam.";

		/// <summary>
		/// Override this field and return false to prevent a HarmonyInstance being created and automatically patching
		/// your assembly
		/// </summary>
		protected virtual bool HarmonyAutoPatch { get { return true; } }

		/// <summary>
		/// The reference to the instantiated HarmonyInstance that applies the patches within the assembly
		/// </summary>
		protected HarmonyInstance HarmonyInstance { get; set; }

		/// <summary>
		/// AssetLoader instance
		/// </summary>
		protected AssetLoader AssetLoader { get; set; }

		/// <summary>
		/// Instantiate a new BaseMod
		/// </summary>
		protected SunbeamMod()
		{
			this.AssetLoader = new AssetLoader(this.ModIdentifier);
		}

		/// <summary>
		/// Use this for mod initialization
		/// </summary>
		public virtual void Initialize() { }

		/// <summary>
		/// Called whenever the browser instance finished (re)loading the start menu
		/// </summary>
		/// <param name="surface"></param>
		public virtual void StartMenuUILoaded(BrowserRenderSurface surface) { }

		/// <summary>
		/// Called whenever the browser instance finished (re)loading the ingame overlay
		/// </summary>
		/// <param name="surface"></param>
		public virtual void IngameOverlayUILoaded(BrowserRenderSurface surface) { }

		/// <summary>
		/// Called before GameContext.ResourceIntializations
		/// </summary>
		public virtual void GameContextInitializeBefore() { }

		/// <summary>
		/// Called at the end of GameContext.ResourceIntializations
		/// </summary>
		public virtual void GameContextInitializeAfter() { }

		/// <summary>
		/// Called on GameContext.Deinitialize
		/// </summary>
		public virtual void GameContextDeinitialize() { }

		/// <summary>
		/// Called at the start of GameContext.Reload which is called from either ClientMainLoop.AttemptActivateBundle
		/// or ServerMainLoop.RequestReload
		/// </summary>
		public virtual void GameContextReloadBefore() { }

		/// <summary>
		/// Called at the end of GameContext.Reload which is called from either ClientMainLoop.AttemptActivateBundle
		/// or ServerMainLoop.RequestReload
		/// </summary>
		public virtual void GameContextReloadAfter() { }

		/// <summary>
		/// Dispose is called on application shutdown to cleanup resources
		/// This get's called through ClientContext.Deinitialize()
		/// </summary>
		public virtual void Dispose() { }

		/// <summary>
		/// Called before ClientContext.ResourceIntializations
		/// </summary>
		public virtual void ClientContextInitializeBefore() { }

		/// <summary>
		/// Called at the end of ClientContext.ResourceIntializations
		/// </summary>
		public virtual void ClientContextInitializeAfter() { }

		/// <summary>
		/// Called on ClientContext.Deinitialize
		/// </summary>
		public virtual void ClientContextDeinitialize() { }


		/// <summary>
		/// Called at the start of ClientContext.Reload which is called from ClientMainLoop.AttemptActivateBundle
		/// </summary>
		public virtual void ClientContextReloadBefore() { }

		/// <summary>
		/// Called at the end of ClientContext.Reload which is called from ClientMainLoop.AttemptActivateBundle
		/// </summary>
		public virtual void ClientContextReloadAfter() { }

		/// <summary>
		/// Called from ClientContext.CleanupOldSession through ClientMainLoop.Dispose
		/// Gets called whenever you exit to the main menu from a game session
		/// </summary>
		public virtual void CleanupOldSession() { }

		/// <summary>
		/// Called at the start of Universe.Update
		/// </summary>
		/// <param name="universe"></param>
		/// <param name="step"></param>
		public virtual void UniverseUpdateBefore(Universe universe, Timestep step) { }

		/// <summary>
		/// Called at the end of Universe.Update
		/// </summary>
		public virtual void UniverseUpdateAfter() { }

		/// <summary>
		/// Called from Universe.CanPlaceTile 
		/// This function only get's called if the method body prior to this call has not returned false. If the core logic
		/// determines a tile cannot be placed this method won't be executed
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="tile"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public virtual bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
		}

		/// <summary>
		/// Called from Universe.CanReplaceTile 
		/// This function only get's called if the method body prior to this call has not returned false. If the core logic
		/// determines a tile cannot be replaced this method won't be executed
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="tile"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public virtual bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
		}

		/// <summary>
		/// Called from Universe.CanRemoveTile 
		/// This function only get's called if the method body prior to this call has not returned false. If the core logic
		/// determines a tile cannot be removed this method won't be executed
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public virtual bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
		{
			return true;
		}

		/// <summary>
		/// Applies the Harmony patches if the HarmonyAutoPatch is set to true
		/// </summary>
		internal void ApplyHarmonyPatches()
		{
			if (!this.HarmonyAutoPatch)
			{
				return;
			}

			string identifier = SunbeamMod.HarmonyPrefix + this.ModIdentifier;
			try
			{
				this.HarmonyInstance = HarmonyInstance.Create(identifier);
				this.HarmonyInstance.PatchAll(GetType().Assembly);
			}
			catch (Exception e)
			{
				Logger.WriteLine("SunbeamMod: Failed to apply Harmony patches for " + this.ModIdentifier + ". Exception " + e);
			}
		}
	}
}
