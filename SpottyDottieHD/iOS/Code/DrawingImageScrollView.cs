using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using MonoTouch.ObjCRuntime;
using EllieSoft.Support;

namespace SpottyDottie
{
  public class DrawingImageScrollView : UIScrollView
  {
    public event EventHandler OnTouchDown;
    public event EventHandler OnTouchUp;

    private const int ZOOM_VIEW_TAG = 100;

    private readonly ImageManager mImgMgr = new ImageManager();
    private DrawingImageView mDrawingImageView;

    public float LineWidth
    {
      get
      {
        return mDrawingImageView.LineWidth;
      }
      set
      {
        mDrawingImageView.LineWidth = value;
      }
    }

    public CGColor LineColor
    {
      get
      {
        return mDrawingImageView.LineColor;
      }
      set
      {
        mDrawingImageView.LineColor = value;
      }
    }

    public DrawingImageScrollView(RectangleF bounds) : base(bounds)
    {
      BackgroundColor = UIColor.Black;
      ViewForZoomingInScrollView = ViewForZoomingInDrawingImageScrollView;
      BouncesZoom = true;
      AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight |
                          UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin |
                          UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleTopMargin;
      MaximumZoomScale = 2.5f;
      DecelerationRate *= 0.5f;
    }

    public UIView ViewForZoomingInDrawingImageScrollView(UIScrollView scrollView)
    {
      UIView view = null;
      if (scrollView == this)
      {
        view = ViewWithTag(ZOOM_VIEW_TAG);
      }

      return view;
    }

    #region Image handling methods

    private void LoadRandomImage()
    {
      LoadImageByName(mImgMgr.RandomImage());
    }

    public void LoadImageByName(string imgPath)
    {
      // first remove previous image view, if any
      var prevImgView = ViewWithTag(ZOOM_VIEW_TAG);
      if (null != prevImgView)
      {
        prevImgView.RemoveFromSuperview();
        prevImgView.Dispose();
      }
      prevImgView = null;

      var loader = new LoadingView("Loading ...");
      try
      {
        loader.Show();

        var image = UIImage.FromFile(imgPath);
        mDrawingImageView = new DrawingImageView(image);
        mDrawingImageView.OnTouchDown += OnTouchDownInternal;
        mDrawingImageView.OnTouchUp += OnTouchUpInternal;
        mDrawingImageView.Tag = ZOOM_VIEW_TAG;
        AddSubview(mDrawingImageView);
        ContentSize = mDrawingImageView.Frame.Size;

        // choose minimum scale so image width fits screen
        var minScale = Frame.Size.Width / mDrawingImageView.Frame.Size.Width;
        MinimumZoomScale = minScale;
        ZoomScale = minScale;
        ContentOffset = PointF.Empty;
      }
      finally
      {
        loader.Hide();
      }
    }

    #endregion

    #region Touch handling

    void OnTouchDownInternal (object sender, EventArgs e)
    {
      if (null != OnTouchDown)
      {
        OnTouchDown(sender, e);
      }
    }

    void OnTouchUpInternal (object sender, EventArgs e)
    {
      if (null != OnTouchUp)
      {
        OnTouchUp(sender, e);
      }
    }

    public override bool TouchesShouldCancelInContentView(UIView view)
    {
      return false;
    }

    #endregion

    #region Line segment handling

    public void UndoLastSegment()
    {
      var drawImgView = (DrawingImageView)ViewWithTag(ZOOM_VIEW_TAG);
      if (null != drawImgView)
      {
        drawImgView.UndoLastSegment();
      }
    }

    public void ClearSegments()
    {
      var drawImgView = (DrawingImageView)ViewWithTag(ZOOM_VIEW_TAG);
      if (null != drawImgView)
      {
        drawImgView.ClearSegments();
      }
    }

    #endregion
  }
}

