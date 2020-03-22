using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using MonoTouch.AudioToolbox;

namespace SpottyDottie
{
  public class PngSaver
  {
    static public UIImage GetPng(UIView view)
    {
      UIImage retVal;

      UIGraphics.BeginImageContext(new SizeF(view.Bounds.Width, view.Bounds.Height));

      var ctx = UIGraphics.GetCurrentContext();
      view.Layer.RenderInContext(ctx);
      retVal = UIGraphics.GetImageFromCurrentImageContext();

      UIGraphics.EndImageContext();

      return retVal;
    }
  }
}

