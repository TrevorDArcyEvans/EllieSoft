using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;

namespace SpottyDottie
{
  public class ColourPickerView : UIImageView
  {
    public CGColor Color = new CGColor(0f, 0f, 0f);

    public event EventHandler OnColourChanged;

    public ColourPickerView() : base(UIImage.FromFile("ColourSwatches.png"))
    {
      UserInteractionEnabled = true;
    }

    #region Touch handling

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
      base.TouchesEnded(touches, evt);

      var touch = (UITouch)touches.AnyObject;
      var pt = touch.LocationInView(this);

#if DEBUG
      Debug.WriteLine("OnColourChanged = [" + pt.X + "," + pt.Y + "]");
#endif

      // extract colour from where user touched
      var buffer = Marshal.AllocHGlobal ((int)(Image.Size.Width * Image.Size.Height * 4));
      var colorSpace = CGColorSpace.CreateDeviceRGB ();
      var context = new CGBitmapContext (buffer, (int)Image.Size.Width, (int)Image.Size.Height, 8, 4 * (int)Image.Size.Width, colorSpace, CGImageAlphaInfo.NoneSkipFirst);

      try
      {
        context.InterpolationQuality = CGInterpolationQuality.None;
        context.DrawImage(new Rectangle(0, 0, (int)Image.Size.Width, (int)Image.Size.Height), Image.CGImage);
  
        unsafe
        {
          var bufPtr = (byte*)((void*)buffer);
          int offset = (int)(4*(Image.Size.Width * pt.Y + pt.X));
          float alpha =  (float)bufPtr[offset] / 255f;
          float red = (float)bufPtr[offset + 1] / 255f;
          float green = (float)bufPtr[offset + 2] / 255f;
          float blue = (float)bufPtr[offset + 3] / 255f;
          
          Color = new CGColor(red, green, blue, alpha);
        }
      }
      finally
      {
        context.Dispose();
        colorSpace.Dispose();
        Marshal.FreeHGlobal(buffer);
      }

      if (null != OnColourChanged)
      {
        OnColourChanged(this, null);
      }
    }

    #endregion
  }
}

