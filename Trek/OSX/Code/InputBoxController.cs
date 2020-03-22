
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace SuperStarTrek
{
  public partial class InputBoxController : MonoMac.AppKit.NSWindowController
  {
    #region Constructors

    // Called when created from unmanaged code
    public InputBoxController (IntPtr handle) : base(handle)
    {
      Initialize ();
    }

    // Called when created directly from a XIB file
    [Export("initWithCoder:")]
    public InputBoxController (NSCoder coder) : base(coder)
    {
      Initialize ();
    }

    // Call to load from the XIB/NIB file
    public InputBoxController() : base("InputBox")
    {
      Initialize();
    }

    public InputBoxController(string title) : this()
    {
      Window.Title = title;
    }

    // Shared initialization code
    void Initialize()
    {
    }

    #endregion

    //strongly typed window accessor
    public new InputBox Window
    {
      get { return (InputBox)base.Window; }
    }

    partial void BtnOk (MonoMac.AppKit.NSButton sender)
    {
      Close ();
      NSApplication.SharedApplication.StopModalWithCode (1);
    }

    partial void BtnCancel (MonoMac.AppKit.NSButton sender)
    {
      Close ();
      NSApplication.SharedApplication.StopModalWithCode (0);
    }

    public global::MonoMac.AppKit.NSTextField RealValue
    {
      get { return Value; }
    }
    
  }
}

