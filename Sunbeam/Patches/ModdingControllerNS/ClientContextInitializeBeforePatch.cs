using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "ClientContextInitializeBefore")]
    class ClientContextInitializeBeforePatch
	{
		[HarmonyPrefix]
		static void BeforeClientContextInitializeBefore()
		{
			SunbeamController.Instance.ClientContextInitializeBefore();
		}
    }
}
