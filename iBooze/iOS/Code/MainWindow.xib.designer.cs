// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace iBooze {
	
	
	// Base type probably should be MonoTouch.Foundation.NSObject or subclass
	[MonoTouch.Foundation.Register("AppDelegate")]
	public partial class AppDelegate {
		
		private MonoTouch.UIKit.UIWindow __mt_window;
		
		private MonoTouch.UIKit.UITextField __mt_txtABV;
		
		private MonoTouch.UIKit.UITextField __mt_txtCartonCost;
		
		private MonoTouch.UIKit.UITextField __mt_txtUnitCost;
		
		private MonoTouch.UIKit.UITextField __mt_txtUnitsPerCarton;
		
		private MonoTouch.UIKit.UITextField __mt_txtUnitVolume;
		
		private MonoTouch.UIKit.UITabBarController __mt_tabBarController;
		
		private MonoTouch.UIKit.UITabBarItem __mt_tabBarItemiBooze;
		
		private MonoTouch.UIKit.UITabBarItem __mt_tabBarItemAbout;
		
		private MonoTouch.UIKit.UITabBarItem __mt_tabBarItemInfo;
		
		private MonoTouch.UIKit.UIWebView __mt_AboutPage;
		
		private MonoTouch.UIKit.UIWebView __mt_InfoPage;
		
		#pragma warning disable 0169
		[MonoTouch.Foundation.Export("DidEndOnExit:")]
		partial void DidEndOnExit (MonoTouch.UIKit.UITextField sender);

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
		
		[MonoTouch.Foundation.Connect("txtABV")]
		private MonoTouch.UIKit.UITextField txtABV {
			get {
				this.__mt_txtABV = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("txtABV")));
				return this.__mt_txtABV;
			}
			set {
				this.__mt_txtABV = value;
				this.SetNativeField("txtABV", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("txtCartonCost")]
		private MonoTouch.UIKit.UITextField txtCartonCost {
			get {
				this.__mt_txtCartonCost = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("txtCartonCost")));
				return this.__mt_txtCartonCost;
			}
			set {
				this.__mt_txtCartonCost = value;
				this.SetNativeField("txtCartonCost", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("txtUnitCost")]
		private MonoTouch.UIKit.UITextField txtUnitCost {
			get {
				this.__mt_txtUnitCost = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("txtUnitCost")));
				return this.__mt_txtUnitCost;
			}
			set {
				this.__mt_txtUnitCost = value;
				this.SetNativeField("txtUnitCost", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("txtUnitsPerCarton")]
		private MonoTouch.UIKit.UITextField txtUnitsPerCarton {
			get {
				this.__mt_txtUnitsPerCarton = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("txtUnitsPerCarton")));
				return this.__mt_txtUnitsPerCarton;
			}
			set {
				this.__mt_txtUnitsPerCarton = value;
				this.SetNativeField("txtUnitsPerCarton", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("txtUnitVolume")]
		private MonoTouch.UIKit.UITextField txtUnitVolume {
			get {
				this.__mt_txtUnitVolume = ((MonoTouch.UIKit.UITextField)(this.GetNativeField("txtUnitVolume")));
				return this.__mt_txtUnitVolume;
			}
			set {
				this.__mt_txtUnitVolume = value;
				this.SetNativeField("txtUnitVolume", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("tabBarController")]
		private MonoTouch.UIKit.UITabBarController tabBarController {
			get {
				this.__mt_tabBarController = ((MonoTouch.UIKit.UITabBarController)(this.GetNativeField("tabBarController")));
				return this.__mt_tabBarController;
			}
			set {
				this.__mt_tabBarController = value;
				this.SetNativeField("tabBarController", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("tabBarItemiBooze")]
		private MonoTouch.UIKit.UITabBarItem tabBarItemiBooze {
			get {
				this.__mt_tabBarItemiBooze = ((MonoTouch.UIKit.UITabBarItem)(this.GetNativeField("tabBarItemiBooze")));
				return this.__mt_tabBarItemiBooze;
			}
			set {
				this.__mt_tabBarItemiBooze = value;
				this.SetNativeField("tabBarItemiBooze", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("tabBarItemAbout")]
		private MonoTouch.UIKit.UITabBarItem tabBarItemAbout {
			get {
				this.__mt_tabBarItemAbout = ((MonoTouch.UIKit.UITabBarItem)(this.GetNativeField("tabBarItemAbout")));
				return this.__mt_tabBarItemAbout;
			}
			set {
				this.__mt_tabBarItemAbout = value;
				this.SetNativeField("tabBarItemAbout", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("tabBarItemInfo")]
		private MonoTouch.UIKit.UITabBarItem tabBarItemInfo {
			get {
				this.__mt_tabBarItemInfo = ((MonoTouch.UIKit.UITabBarItem)(this.GetNativeField("tabBarItemInfo")));
				return this.__mt_tabBarItemInfo;
			}
			set {
				this.__mt_tabBarItemInfo = value;
				this.SetNativeField("tabBarItemInfo", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("AboutPage")]
		private MonoTouch.UIKit.UIWebView AboutPage {
			get {
				this.__mt_AboutPage = ((MonoTouch.UIKit.UIWebView)(this.GetNativeField("AboutPage")));
				return this.__mt_AboutPage;
			}
			set {
				this.__mt_AboutPage = value;
				this.SetNativeField("AboutPage", value);
			}
		}
		
		[MonoTouch.Foundation.Connect("InfoPage")]
		private MonoTouch.UIKit.UIWebView InfoPage {
			get {
				this.__mt_InfoPage = ((MonoTouch.UIKit.UIWebView)(this.GetNativeField("InfoPage")));
				return this.__mt_InfoPage;
			}
			set {
				this.__mt_InfoPage = value;
				this.SetNativeField("InfoPage", value);
			}
		}
	}
}