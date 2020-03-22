// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace asciiART
{
	[Register ("AboutViewController")]
	partial class AboutViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView mAboutWebView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (mAboutWebView != null) {
				mAboutWebView.Dispose ();
				mAboutWebView = null;
			}
		}
	}
}
