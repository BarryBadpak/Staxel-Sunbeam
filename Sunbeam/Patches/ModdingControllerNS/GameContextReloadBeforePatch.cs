using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "GameContextReloadBefore")]
    class GameContextReloadBeforePatch
	{
		[HarmonyPrefix]
		static void BeforeGameContextReloadBefore()
		{
			SunbeamController.Instance.GameContextReloadBefore();
		}
    }
}
