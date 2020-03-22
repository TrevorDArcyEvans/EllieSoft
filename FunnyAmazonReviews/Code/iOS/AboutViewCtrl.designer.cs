// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Funny_Amazon_Reviews
{
	[Register ("AboutViewCtrl")]
	partial class AboutViewCtrl
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView mAboutScreen { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (mAboutScreen != null) {
				mAboutScreen.Dispose ();
				mAboutScreen = null;
			}
		}
	}
}
