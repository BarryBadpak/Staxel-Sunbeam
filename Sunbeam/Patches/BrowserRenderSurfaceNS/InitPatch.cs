using CefSharp;
using Harmony;
using Plukit.Base;
using Staxel.Browser;
using Sunbeam.Core;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sunbeam.Patches.BrowserRenderSurfaceNS
{
	/// <summary>
	/// Remove this._initialised = true from Init function
	/// </summary>
	[HarmonyPatch(typeof(BrowserRenderSurface), "Init")]
    class InitPatch
    {
		private static BrowserRenderSurface StartMenuSurface { get; set; }
		private static BrowserRenderSurface OverlaySurface { get; set; }
		private static List<UIReferencePair> UIPairs = new List<UIReferencePair>();

		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> TranspileInit(IEnumerable<CodeInstruction> instructions)
		{
			int targetIdx = -1;
			List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

			for (int i = 0; i < codes.Count; i++)
			{

				if (codes[i].opcode == OpCodes.Stfld && codes[i].operand.ToString() == "Boolean _initialized" 
					&& codes[i - 1].opcode == OpCodes.Ldc_I4_1)
				{
					targetIdx = i;
					continue;
				}
			}

			if (targetIdx != -1)
			{
				codes[targetIdx - 2].opcode = OpCodes.Nop; // Otherwise we're removing a jump target
				codes.RemoveRange(targetIdx - 1, 2);
			}

			return codes.AsEnumerable();
		}

		/// <summary>
		/// Patch the after init to store the BrowserRenderSurface and ChromiumWebBrowser pair
		/// and add our own callback to the browser instance
		/// 
		/// On load we can then loop through the pairs to see which one contains the right reference
		/// and pass that BrowserRenderSurface to the hook method
		/// </summary>
		/// <param name="__instance"></param>
		[HarmonyPostfix]
		static void AfterInit(BrowserRenderSurface __instance)
		{
			FieldInfo BrowserField = __instance.GetType().GetField("_browser", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
			ChromiumWebBrowser BrowserInstance = BrowserField.GetValue(__instance) as ChromiumWebBrowser;

			FieldInfo InitializedField = __instance.GetType().GetField("_initialized", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
			bool Initialized = (bool) InitializedField.GetValue(__instance);

			if (!Initialized)
			{
				InitializedField.SetValue(__instance, true);

				UIReferencePair Pair = new UIReferencePair(BrowserInstance, __instance);
				if (!InitPatch.UIPairs.Contains(Pair))
				{
					InitPatch.UIPairs.Add(Pair);
					BrowserInstance.FrameLoadEnd += InitPatch.FrameLoadEnd;
				}
			}
        }
		
        /// <summary>
        /// Callback that gets called on every frameload end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {

			for(int i = InitPatch.UIPairs.Count - 1; i >= 0; i--)
			{
				UIReferencePair UIPair = InitPatch.UIPairs[i];

				if (UIPair.Browser.Address.Contains("startmenu.html"))
				{
					InitPatch.StartMenuSurface = UIPair.Surface;
					InitPatch.UIPairs.RemoveAt(i);
					continue;
				}

				if (UIPair.Browser.Address.Contains("overlay.html"))
				{
					InitPatch.OverlaySurface = UIPair.Surface;
					InitPatch.UIPairs.RemoveAt(i);
					continue;
				}
			}


			if(e.Url.Contains("startmenu.html") && InitPatch.StartMenuSurface != null)
			{
				SunbeamController.Instance.StartMenuUILoaded(InitPatch.StartMenuSurface);
				return;
			}

			if(e.Url.Contains("overlay.html") && InitPatch.OverlaySurface != null)
			{
				SunbeamController.Instance.IngameOverlayUILoaded(InitPatch.OverlaySurface);
				return;
			}
        }
    }
}
