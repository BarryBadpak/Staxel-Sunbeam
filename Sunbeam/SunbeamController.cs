using Plukit.Base;
using Staxel.Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Sunbeam
{
    public class SunbeamController
    {
        private static SunbeamController _instance;
        public static SunbeamController Instance
        {
            get { return _instance ?? (_instance = new SunbeamController()); }
        }

        private readonly List<BaseMod> DerivedMods = new List<BaseMod>();

        /// <summary>
        /// Initialize the SunbeamController
        /// Should only be called once, at least after the ModdingController.GameContextInitializeInit was called
        /// </summary>
        public void Initialize(IEnumerable ModList)
        {

            this.EnumerateDerivedMods(ModList);
        }

        /// <summary>
        /// Enumerate all derived mods
        /// </summary>
        /// <param name="ModList"></param>
        public void EnumerateDerivedMods(IEnumerable ModList)
        {
            foreach (var ModHook in ModList as IEnumerable)
            {
                FieldInfo Field2 = ModHook.GetType().GetField("ModHookV2", BindingFlags.Public | BindingFlags.Instance);
                IModHookV2 ModInstance = Field2.GetValue(ModHook) as IModHookV2;

                if (ModInstance is BaseMod)
                {
                    if (this.DerivedMods.Find(m => m.ModIdentifier == (ModInstance as BaseMod).ModIdentifier) != null)
                    {
                        Logger.WriteLine("SunBeamController.EnumerateDerivedMods: Skipped mod" + (ModInstance as BaseMod).ModIdentifier + " already added.\r\n");
                        continue;
                    }

                    SunbeamController.Instance.DerivedMods.Add(ModInstance as BaseMod);
                    Logger.WriteLine("SunBeamController.EnumerateDerivedMods: Added mod" + (ModInstance as BaseMod).ModIdentifier + ".\r\n");
                }
            }
        }
    }
}
