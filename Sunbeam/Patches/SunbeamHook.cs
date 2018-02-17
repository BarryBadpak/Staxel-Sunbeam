using Plukit.Base;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Modding;
using Staxel.Tiles;

namespace Sunbeam
{
	class SunbeamHook : IModHookV2
	{
		/// <summary>
		/// Instantiate the sunbeamcontroller somewhere within the GameContextInitializeInit method
		/// This means that sunbeam mods will lose the GameContextInitializeInit method to hook into
		/// however the constructor of sunbeam derived mods will have the same effect
		/// 
		/// I'm not convinced of instantiating mods already at the GameContext point, the most usefull to have
		/// available when the mod is instantiated is the ClientContext, now you need to instantiate ClientContext related
		/// objects in ClientContextInitializeBefore to ensure that everything within is available
		/// </summary>
		public SunbeamHook()
		{
			SunbeamController.Instance.Initialize();
		}

		/// <summary>
		/// Delegate method to SunbeamController
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="tile"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return SunbeamController.Instance.CanPlaceTile(entity, location, tile, accessFlags);
		}

		/// <summary>
		/// Delegate method to SunbeamController
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
		{
			return SunbeamController.Instance.CanRemoveTile(entity, location, accessFlags);
		}

		/// <summary>
		/// Delegate method to SunbeamController
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="location"></param>
		/// <param name="tile"></param>
		/// <param name="accessFlags"></param>
		/// <returns></returns>
		public bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return SunbeamController.Instance.CanReplaceTile(entity, location, tile, accessFlags);
		}

		public void CleanupOldSession() {}
		public void ClientContextDeinitialize() {}
		public void ClientContextInitializeAfter() {}
		public void ClientContextInitializeBefore() {}
		public void ClientContextInitializeInit() {}
		public void ClientContextReloadAfter() {}
		public void ClientContextReloadBefore() {}
		public void Dispose() {}
		public void GameContextDeinitialize() {}
		public void GameContextInitializeAfter() {}
		public void GameContextInitializeBefore() {}
		public void GameContextInitializeInit() {}
		public void GameContextReloadAfter() {}
		public void GameContextReloadBefore() {}
		public void UniverseUpdateAfter() {}
		public void UniverseUpdateBefore(Universe universe, Timestep step) {}
	}
}
