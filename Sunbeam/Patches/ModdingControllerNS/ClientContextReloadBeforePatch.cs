using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "ClientContextReloadBefore")]
    class ClientContextReloadBeforePatch
	{
		[HarmonyPrefix]
		static void BeforeClientContextReloadBefore()
		{
			SunbeamController.Instance.ClientContextReloadBefore();
		}
    }
}
