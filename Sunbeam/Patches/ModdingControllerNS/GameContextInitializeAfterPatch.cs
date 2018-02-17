using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "GameContextInitializeAfter")]
    class GameContextInitializeAfterPatch
	{
		[HarmonyPrefix]
		static void BeforeGameContextInitializeAfter()
		{
			SunbeamController.Instance.GameContextInitializeAfter();
		}
    }
}
