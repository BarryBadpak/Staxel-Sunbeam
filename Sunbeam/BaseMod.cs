using Harmony;
using Plukit.Base;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Modding;
using Staxel.Tiles;
using System;

namespace Sunbeam
{
    public abstract class BaseMod : IModHookV2
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
        public const string HarmonyPrefix = "Mod.";

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
        protected BaseMod()
        {
            this.ApplyHarmonyPatches();
            this.AssetLoader = new AssetLoader(this.ModIdentifier);
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

            string identifier = BaseMod.HarmonyPrefix + this.ModIdentifier;
            try
            {
                this.HarmonyInstance = HarmonyInstance.Create(identifier);
                this.HarmonyInstance.PatchAll(GetType().Assembly);
            }
            catch (Exception e)
            {
                Logger.WriteLine("BaseModule: Failed to apply Harmony patches for " + this.ModIdentifier + ". Exception " + e);
            }
        }


        public void ClientContextInitializeInit() { }

        /// <summary>
        /// Called whenever the ClientContext.Initialize is called on game instantiation
        /// ClientContext.Initialize is called after GameContext.Initialize
        /// 
        /// Currently this method only exists within the interface
        /// </summary>
        public virtual void ClientContextInitializeInitOverride()
        {

        }

        /// <summary>
        /// Called before ClientContext.ResourceIntializations
        /// </summary>
        public void ClientContextInitializeBefore() { }

        /// <summary>
        /// Called at the end of ClientContext.ResourceIntializations
        /// </summary>
        public void ClientContextInitializeAfter() { }

        /// <summary>
        /// Called on ClientContext.Deinitialize
        /// </summary>
        public void ClientContextDeinitialize() { }

        /// <summary>
        /// Called at the start of ClientContext.Reload which is called from ClientMainLoop.AttemptActivateBundle
        /// </summary>
        public void ClientContextReloadBefore() { }

        /// <summary>
        /// Called at the end of ClientContext.Reload which is called from ClientMainLoop.AttemptActivateBundle
        /// </summary>
        public void ClientContextReloadAfter() { }

        /// <summary>
        /// Called from ClientContext.CleanupOldSession through ClientMainLoop.Dispose
        /// Gets called whenever you exit to the main menu from a game session
        /// </summary>
        public void CleanupOldSession() { }

        /// <summary>
        /// GameContext.Initialize calls the GameContextInitializeInit on the ModdingController
        /// the ModdingController will instantiate all of the Mods and after instantiating all of them
        /// call GameContextInitializeInit on the mods themselves.
        /// 
        /// GameContext.Initialize is called before ClientContext.Initialize
        /// </summary>
        public void GameContextInitializeInit() { }

        /// <summary>
        /// Called before GameContext.ResourceIntializations
        /// </summary>
        public void GameContextInitializeBefore() { }

        /// <summary>
        /// Called at the end of GameContext.ResourceIntializations
        /// </summary>
        public void GameContextInitializeAfter() { }

        /// <summary>
        /// Called on GameContext.Deinitialize
        /// </summary>
        public void GameContextDeinitialize() { }

        /// <summary>
        /// Called at the start of GameContext.Reload which is called from either ClientMainLoop.AttemptActivateBundle
        /// or ServerMainLoop.RequestReload
        /// </summary>
        public void GameContextReloadBefore() { }

        /// <summary>
        /// Called at the end of GameContext.Reload which is called from either ClientMainLoop.AttemptActivateBundle
        /// or ServerMainLoop.RequestReload
        /// </summary>
        public void GameContextReloadAfter() { }

        /// <summary>
        /// Called at the start of Universe.Update
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="step"></param>
        public void UniverseUpdateBefore(Universe universe, Timestep step) { }

        /// <summary>
        /// Called at the end of Universe.Update
        /// </summary>
        public void UniverseUpdateAfter() { }

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
        public bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
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
        public bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
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
        public bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
        {
            return true;
        }

        /// <summary>
        /// Dispose is called on application shutdown to cleanup resources
        /// This get's called through ClientContext.Deinitialize()
        /// </summary>
        public void Dispose() { }
    }
}
