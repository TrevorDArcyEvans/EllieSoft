using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace SpottyDottie
{
  public class DrawingImageView : UIImageView
  {
    public event EventHandler OnTouchDown;
    public event EventHandler OnTouchUp;

    private DrawingView mDrawing;

    public float LineWidth
    {
      get
      {
        return mDrawing.LineWidth;
      }
      set
      {
        mDrawing.LineWidth = value;
      }
    }

    public CGColor LineColor
    {
      get
      {
        return mDrawing.LineColor;
      }
      set
      {
        mDrawing.LineColor = value;
      }
    }

    public DrawingImageView(UIImage img) : base(img)
    {
      UserInteractionEnabled = true;
      MultipleTouchEnabled = true;

      mDrawing = new DrawingView(new RectangleF(0f, 0f, img.Size.Width, img.Size.Height));
      mDrawing.OnTouchDown += OnTouchDownInternal;
      mDrawing.OnTouchUp += OnTouchUpInternal;
      AddSubview(mDrawing);
    }

    #region Touch handling

    private void OnTouchDownInternal (object sender, EventArgs e)
    {
      if (null != OnTouchDown)
      {
        OnTouchDown(sender, e);
      }
    }
    private void OnTouchUpInternal (object sender, EventArgs e)
    {
      if (null != OnTouchUp)
      {
        OnTouchUp(sender, e);
      }
    }

    #endregion

    #region Segment handling

    public void UndoLastSegment()
    {
      mDrawing.UndoLastSegment();
    }

    public void ClearSegments()
    {
      mDrawing.ClearSegments();
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        mDrawing.Dispose();
      }

      base.Dispose(disposing);
    }
  }
}

