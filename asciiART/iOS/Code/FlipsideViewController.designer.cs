// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace asciiART
{
	[Register ("FlipsideViewController")]
	partial class FlipsideViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView mResultsView { get; set; }

		[Action ("OnUploadTouchUpInside:")]
		partial void OnUploadTouchUpInside (MonoTouch.UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (mResultsView != null) {
				mResultsView.Dispose ();
				mResultsView = null;
			}
		}
	}
}
