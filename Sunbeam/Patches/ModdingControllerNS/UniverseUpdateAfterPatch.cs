using Harmony;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "UniverseUpdateAfter")]
    class UniverseUpdateAfterPatch
	{
		[HarmonyPrefix]
		static void BeforeUniverseUpdateAfter()
		{
			SunbeamController.Instance.UniverseUpdateAfter();
		}
    }
}
