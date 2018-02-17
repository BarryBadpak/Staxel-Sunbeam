using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "ClientContextDeinitialize")]
    class ClientContextDeinitializePatch
	{
		[HarmonyPrefix]
		static void BeforeClientContextDeinitialize()
		{
			SunbeamController.Instance.ClientContextDeinitialize();
		}
    }
}
