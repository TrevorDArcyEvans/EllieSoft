// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace SpottyDottie {
	
	
	// Base type probably should be MonoTouch.UIKit.UIViewController or subclass
	[MonoTouch.Foundation.Register("RootViewController_iPad")]
	public partial class RootViewController_iPad {
		
		private MonoTouch.UIKit.UIView __mt_view;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("view")]
		private MonoTouch.UIKit.UIView view {
			get {
				this.__mt_view = ((MonoTouch.UIKit.UIView)(this.GetNativeField("view")));
				return this.__mt_view;
			}
			set {
				this.__mt_view = value;
				this.SetNativeField("view", value);
			}
		}
	}
	
	// Base type probably should be MonoTouch.Foundation.NSObject or subclass
	[MonoTouch.Foundation.Register("AppDelegate_iPad")]
	public partial class AppDelegate_iPad {
		
		private MonoTouch.UIKit.UIWindow __mt_window;
		
		private RootViewController_iPad __mt_rootController;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Connect("window")]
		private MonoTouch.UIKit.UIWindow window {
			get {
				this.__mt_window = ((MonoTouch.UIKit.UIWindow)(this.GetNativeField("window")));
				return this.__mt_window;
			}
			set {
				this.__mt_window = value;
				this.SetNativeField("window", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("rootController")]
		private RootViewController_iPad rootController {
			get {
				this.__mt_rootController = ((RootViewController_iPad)(this.GetNativeField("rootController")));
				return this.__mt_rootController;
			}
			set {
				this.__mt_rootController = value;
				this.SetNativeField("rootController", value);
			}
		}
	}
}
