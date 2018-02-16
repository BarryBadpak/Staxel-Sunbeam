using Plukit.Base;
using Staxel.Items;
using Staxel.Logic;
using Staxel.Modding;
using Staxel.Tiles;

namespace Sunbeam
{
	class SunbeamHook : IModHookV2
	{

		public SunbeamHook()
		{
			SunbeamController.Instance.Initialize();
		}

		public bool CanPlaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
		}

		public bool CanRemoveTile(Entity entity, Vector3I location, TileAccessFlags accessFlags)
		{
			return true;
		}

		public bool CanReplaceTile(Entity entity, Vector3I location, Tile tile, TileAccessFlags accessFlags)
		{
			return true;
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
