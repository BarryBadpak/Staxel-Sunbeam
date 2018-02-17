using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "Dispose")]
    class DisposePatch
	{
		[HarmonyPrefix]
		static void BeforeDispose()
		{
			SunbeamController.Instance.Dispose();
		}
    }
}
