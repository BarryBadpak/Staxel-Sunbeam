using Staxel.Browser;

namespace Sunbeam.Core
{
	class UIReferencePair
	{
		public ChromiumWebBrowser Browser { get; private set; }
		public BrowserRenderSurface Surface { get; private set; }

		public UIReferencePair(ChromiumWebBrowser browser, BrowserRenderSurface surface)
		{
			this.Browser = browser;
			this.Surface = surface;
		}
	}
}