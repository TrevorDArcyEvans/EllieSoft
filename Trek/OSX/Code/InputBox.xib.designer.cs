// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace SuperStarTrek {
	
	
	// Should subclass MonoMac.AppKit.NSWindow
	[MonoMac.Foundation.Register("InputBox")]
	public partial class InputBox {
	}
	
	// Should subclass MonoMac.AppKit.NSWindowController
	[MonoMac.Foundation.Register("InputBoxController")]
	public partial class InputBoxController {
		
		private global::MonoMac.AppKit.NSTextField __mt_Value;
		
		#pragma warning disable 0169
		[MonoMac.Foundation.Export("BtnCancel:")]
		partial void BtnCancel (MonoMac.AppKit.NSButton sender);

		[MonoMac.Foundation.Export("BtnOk:")]
		partial void BtnOk (MonoMac.AppKit.NSButton sender);

		[MonoMac.Foundation.Connect("Value")]
		private global::MonoMac.AppKit.NSTextField Value {
			get {
				this.__mt_Value = ((global::MonoMac.AppKit.NSTextField)(this.GetNativeField("Value")));
				return this.__mt_Value;
			}
			set {
				this.__mt_Value = value;
				this.SetNativeField("Value", value);
			}
		}
	}
}
