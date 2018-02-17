using Harmony;
using Staxel.Browser;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "CleanupOldSession")]
    class CleanupOldSessionPatch
	{
		[HarmonyPrefix]
		static void BeforeCleanupOldSession()
		{
			SunbeamController.Instance.CleanupOldSession();
		}
    }
}
