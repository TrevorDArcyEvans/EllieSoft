using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using OpenFlowSharp;
using MonoTouch.Dialog;

namespace SpottyDottie
{
  public partial class RootViewController : UIViewController
  {
    private UIWebView mInstructions;
    private UIButton mCmdOK;
    private FlowViewController mFlowViewCtlr;

    public RootViewController(IntPtr handle) : base(handle)
    {
    }

    private void Initialise()
    {
      const float BtnWidth = 200f;
      const float BtnHeight = 40f;
      const float BtnVertMargin = 10f;

      mFlowViewCtlr = new FlowViewController();

      // instructions - allow for OK button at bottom
      var instrUrl = NSUrl.FromFilename("Instructions/index.html");
      var instrUrlReq = NSUrlRequest.FromUrl(instrUrl);
      var instrBounds = new RectangleF(0f, 0f, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - BtnHeight - 2f * BtnVertMargin);
      mInstructions = new UIWebView(instrBounds);
      mInstructions.LoadRequest(instrUrlReq);
      View.AddSubview(mInstructions);

      // centre OK button at bottom of screen
      var btnBounds = new RectangleF((UIScreen.MainScreen.Bounds.Width - BtnWidth) / 2, UIScreen.MainScreen.Bounds.Height - BtnHeight - BtnVertMargin, BtnWidth, BtnHeight);

      mCmdOK = new GlassButton(btnBounds)
        {
          Font = UIFont.BoldSystemFontOfSize (22),
          NormalColor = UIColor.Blue,
          HighlightedColor = UIColor.Cyan
        };
      mCmdOK.SetTitle("OK", UIControlState.Normal);
      mCmdOK.TouchUpInside += OnCmdOKTouchUpInside;

      View.AddSubview(mCmdOK);
    }

    public override void LoadView()
    {
      base.LoadView();

      Initialise();
    }

    void OnCmdOKTouchUpInside(object sender, EventArgs e)
    {
      PresentModalViewController(mFlowViewCtlr, true);
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      Initialise();
    }
  }

  public partial class RootViewController_iPad : RootViewController
  {
    public RootViewController_iPad(IntPtr handle) : base(handle)
    {
    }
  }
}

