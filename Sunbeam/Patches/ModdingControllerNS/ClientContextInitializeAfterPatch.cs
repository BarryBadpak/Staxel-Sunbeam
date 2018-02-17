using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "ClientContextInitializeAfter")]
    class ClientContextInitializeAfterPatch
	{
		[HarmonyPrefix]
		static void BeforeClientContextInitializeAfter()
		{
			SunbeamController.Instance.ClientContextInitializeAfter();
		}
    }
}
