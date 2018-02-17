using Harmony;
using Plukit.Base;
using Staxel.Browser;
using Staxel.Logic;
using Staxel.Modding;

namespace Sunbeam.Patches.ModdingControllerNS
{
	[HarmonyPatch(typeof(ModdingController), "UniverseUpdateBefore")]
    class UniverseUpdateBeforePatch
	{
		[HarmonyPrefix]
		static void BeforeUniverseUpdateBefore(Universe universe, Timestep step)
		{
			SunbeamController.Instance.UniverseUpdateBefore(universe, step);
		}
    }
}
