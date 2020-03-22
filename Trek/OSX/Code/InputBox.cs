using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace SuperStarTrek
{
  public partial class InputBox : MonoMac.AppKit.NSWindow
  {

    #region Constructors

    // Called when created from unmanaged code
    public InputBox (IntPtr handle) : base(handle)
    {
      Initialize ();
    }

    // Called when created directly from a XIB file
    [Export("initWithCoder:")]
    public InputBox (NSCoder coder) : base(coder)
    {
      Initialize ();
    }

    // Shared initialization code
    void Initialize ()
    {
    }

    #endregion
  }
}

