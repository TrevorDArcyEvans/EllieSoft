using System;

namespace Trek
{
  public partial class TrekTextView : MonoTouch.UIKit.UITextView
  {
    public delegate void TouchHandler(MonoTouch.Foundation.NSSet touches, MonoTouch.UIKit.UIEvent evt);

    public event TouchHandler OnTouchesEnded;

    public TrekTextView(IntPtr p) : base(p)
    {
    }

    public override void TouchesEnded(MonoTouch.Foundation.NSSet touches, MonoTouch.UIKit.UIEvent evt)
    {
      base.TouchesEnded(touches, evt);

      if (OnTouchesEnded != null)
      {
        OnTouchesEnded(touches, evt);
      }
    }
  }
}

