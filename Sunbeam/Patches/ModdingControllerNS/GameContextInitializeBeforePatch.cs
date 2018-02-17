using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "GameContextInitializeBefore")]
    class GameContextInitializeBeforePatch
    {
		[HarmonyPrefix]
		static void BeforeGameContextInitializeBefore()
		{
			SunbeamController.Instance.GameContextInitializeBefore();
		}
    }
}
