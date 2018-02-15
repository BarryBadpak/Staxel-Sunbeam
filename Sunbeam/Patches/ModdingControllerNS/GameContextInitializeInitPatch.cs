using Harmony;
using Staxel.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Sunbeam.Patches.ModdingControllerNS
{
    [HarmonyPatch(typeof(ModdingController), "GameContextInitializeBefore")]
    class GameContextInitializeInitPatch
    {
        [HarmonyPrefix]
        static void AfterGameContextInitializeBefore(ModdingController __instance)
        {
            FieldInfo Field = __instance.GetType().GetField("_modHooks", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            Object ModList = Field.GetValue(__instance);

            if (ModList is IEnumerable)
            {
                SunbeamController.Instance.EnumerateDerivedMods(ModList as IEnumerable);
            }

            if (ModList == null)
            {
                throw new Exception("ModList is null");
            }
        }
    }
}
