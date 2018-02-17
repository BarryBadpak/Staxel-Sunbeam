using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "GameContextReloadAfter")]
    class GameContextReloadAfterPatch
	{
		[HarmonyPrefix]
		static void BeforeGameContextReloadAfter()
		{
			SunbeamController.Instance.GameContextReloadAfter();
		}
    }
}
