// WARNING
//
// This file has been generated automatically by MonoDevelop to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace Funny_Amazon_Reviews
{
	[Register ("CustomerReviewsViewCtrl")]
	partial class CustomerReviewsViewCtrl
	{
		[Outlet]
		MonoTouch.UIKit.UIWebView mCustomerReviews { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (mCustomerReviews != null) {
				mCustomerReviews.Dispose ();
				mCustomerReviews = null;
			}
		}
	}
}
