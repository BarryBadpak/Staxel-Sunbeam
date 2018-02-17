using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "ClientContextReloadAfter")]
    class ClientContextReloadAfterPatch
	{
		[HarmonyPrefix]
		static void BeforeClientContextReloadAfter()
		{
			SunbeamController.Instance.ClientContextReloadAfter();
		}
    }
}
