// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace asciiART
{
	[Register ("MainViewController")]
	partial class MainViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView mDestinationsList { get; set; }

		[Outlet]
		asciiART.ImageSelectorView mImageSelector { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton mGoCmd { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIProgressView mProgress { get; set; }

		[Action ("OnGoTouchUpInside:")]
		partial void OnGoTouchUpInside (MonoTouch.UIKit.UIButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (mDestinationsList != null) {
				mDestinationsList.Dispose ();
				mDestinationsList = null;
			}

			if (mImageSelector != null) {
				mImageSelector.Dispose ();
				mImageSelector = null;
			}

			if (mGoCmd != null) {
				mGoCmd.Dispose ();
				mGoCmd = null;
			}

			if (mProgress != null) {
				mProgress.Dispose ();
				mProgress = null;
			}
		}
	}
}
