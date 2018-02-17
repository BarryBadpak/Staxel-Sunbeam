using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "GameContextDeinitialize")]
    class GameContextDeinitializePatch
	{
		[HarmonyPrefix]
		static void BeforeGameContextDeinitialize()
		{
			SunbeamController.Instance.GameContextDeinitialize();
		}
    }
}
