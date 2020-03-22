
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Trek
{
  public partial class TrekViewController : UITabBarController
  {
    public TrekViewController(IntPtr p) : base(p)
    {
    }

    public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
    {
      return true;
    }
  }
}

