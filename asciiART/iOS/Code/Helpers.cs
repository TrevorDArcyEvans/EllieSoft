using System;
using System.IO;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;
using EllieSoft.Support;

namespace asciiART
{
  public class Helpers
  {
    public static string GetWebFileName(string rootDir)
    {
      const string RootFileName = "asciiART{0}.html";

      var i = 0;
      var filename = string.Empty;
      do
      {
        filename = Path.Combine(rootDir, string.Format(RootFileName, i++));

      }
      while (File.Exists(filename));

      return filename;
    }

    public static UIImage GetImageFromWebPage(UIWebView webPage, LoadingView lv)
    {
      var oldFrame = webPage.Frame;
      try
      {
        lv.Show();
        var frame = webPage.Frame;
        var fittingSize = webPage.SizeThatFits(new SizeF());
        frame.Size = fittingSize;
        webPage.Frame = frame;

        UIGraphics.BeginImageContext(new SizeF(webPage.Bounds.Width, webPage.Bounds.Height));
        webPage.Layer.RenderInContext(UIGraphics.GetCurrentContext());

        var viewImage = UIGraphics.GetImageFromCurrentImageContext();

        UIGraphics.EndImageContext();

        return viewImage;
      }
      finally
      {
        webPage.Frame = oldFrame;
        lv.Hide();
      }
    }
  }
}

