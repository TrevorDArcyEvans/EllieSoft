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
  public class DrawingView : UIView
  {
    // draw circle to indicate we're ready to draw
    // circle must be a constant size relative to physical screen
    private const float CurrentTouchRadius = 36f;

    public event EventHandler OnTouchDown;
    public event EventHandler OnTouchUp;

    private readonly List<PathSegment> mSegments = new List<PathSegment>(100);
    private PointF mCurrTouch;

    private bool mNeedsScheduledRedraw = false;
    private NSTimer mRedrawTimer;

    #region iPhone4 performance throttling

    // There seems to be a problem with performance, or lack thereof, on the iPhone4.
    // Drawing the lines seems to lag a *lot* whilst it is perfectly usable on the iPad and 3rd gen iTouch.
    // Not sure of the reason for this but it may be related to the retina display...
    // As a workaround, we throttle the rate at which we capture the user's movement and rate of redraw.

    const int MaxPtsPerSec = 5;
    private double mLastMove = 0;
    const int MaxFps = 5;

    #endregion

    #region Properties

    private float mLineWidth = 6f;
    public float LineWidth
    {
      get
      {
        return mLineWidth;
      }
      set
      {
        mLineWidth = value;
        SaveSettings();
      }
    }

    private CGColor mLineColor = new CGColor(0.0f, 0.0f, 1.0f);
    public CGColor LineColor
    {
      get
      {
        return mLineColor;
      }
      set
      {
        mLineColor = value;
        SaveSettings();
      }
    }

    #endregion

    public DrawingView(RectangleF rect) : base(rect)
    {
      UserInteractionEnabled = true;
      BackgroundColor = UIColor.Clear;

      mRedrawTimer = NSTimer.CreateRepeatingScheduledTimer(1 / MaxFps, ScheduledRedraw);

      LoadSettings();
    }

    #region Settings persistence

    private const string LineWidthKey = "LineWidth";
    private const string LineColourRedKey = "LineColourRed";
    private const string LineColourGreenKey = "LineColourGreen";
    private const string LineColourBlueKey = "LineColourBlue";

    private void LoadSettings()
    {
      var plist = NSUserDefaults.StandardUserDefaults;
      var red = plist.FloatForKey(LineColourRedKey);
      var green = plist.FloatForKey(LineColourGreenKey);
      var blue = plist.FloatForKey(LineColourBlueKey);

      // directly manipulate backing variables as going through setter will persist to storage
      mLineColor = new CGColor(red, green, blue);
      mLineWidth = plist.FloatForKey(LineWidthKey);

      // check for first time ever
      if (mLineWidth < 0.1f)
      {
        // go through setter to persist settings
        LineWidth = 6f;
        LineColor = new CGColor(0.0f, 0.0f, 1.0f);
      }
    }

    private void SaveSettings()
    {
      var plist = NSUserDefaults.StandardUserDefaults;

      if (LineColor.ColorSpace.Model == CGColorSpaceModel.RGB)
      {
        plist.SetFloat(LineColor.Components[0], LineColourRedKey);
        plist.SetFloat(LineColor.Components[1], LineColourGreenKey);
        plist.SetFloat(LineColor.Components[2], LineColourBlueKey);
      }
      plist.SetFloat(LineWidth, LineWidthKey);

      plist.Synchronize();
    }

    #endregion

    #region Drawing

    public override void Draw(RectangleF rect)
    {
      if (mNeedsScheduledRedraw)
      {
        return;
      }

      base.Draw(rect);

      var ctx = UIGraphics.GetCurrentContext();

      DrawCurrentTouch(ctx);

      foreach (var thisSeg in mSegments)
      {
        thisSeg.Draw(ctx, rect);
      }
    }

    private void DrawCurrentTouch(CGContext ctx)
    {
      if (mCurrTouch == PointF.Empty)
      {
        return;
      }

      var parentDIV = (DrawingImageView)Superview;
      var grandParentDISV = (DrawingImageScrollView)parentDIV.Superview;
      var zoomScale = grandParentDISV.ZoomScale;

      // circle
      try
      {
        ctx.SaveState();
        ctx.BeginPath();

        ctx.SetStrokeColorWithColor(LineColor);
        ctx.SetLineWidth(9f / zoomScale);

        var rad = CurrentTouchRadius / zoomScale;

        ctx.AddArc(mCurrTouch.X, mCurrTouch.Y, rad, 0.0f, 2.0f * (float)Math.PI, true);
      }
      finally
      {
        ctx.StrokePath();
        ctx.RestoreState();
      }

      // crosshairs
      try
      {
        ctx.SaveState();
        ctx.BeginPath();

        ctx.SetStrokeColorWithColor(LineColor);
        ctx.SetLineWidth(1f / zoomScale);

        // horizontal line
        ctx.MoveTo(0f, mCurrTouch.Y);
        ctx.AddLineToPoint(Frame.Right, mCurrTouch.Y);

        // vertical line
        ctx.MoveTo(mCurrTouch.X, 0f);
        ctx.AddLineToPoint(mCurrTouch.X, Frame.Bottom);
      }
      finally
      {
        ctx.StrokePath();
        ctx.RestoreState();
      }
    }

    private void ScheduledRedraw()
    {
      if (mNeedsScheduledRedraw)
      {
        SetNeedsDisplayInRect(PointBoundingBox(Point.Empty));
      }
      mNeedsScheduledRedraw = false;
    }

    #endregion

    #region Segment handling

    /// <summary>
    /// Region of view to repaint for a touch, in view coordinates
    /// </summary>
    /// <returns>
    /// Repaint area
    /// </returns>
    /// <param name='pt'>
    /// Point.
    /// </param>
    /// <remarks>
    /// Currently returns the physical screen dimensions in view coordinates
    /// </remarks>
    private RectangleF PointBoundingBox(PointF pt)
    {
      var parentDIV = (DrawingImageView)Superview;
      var grandParentDISV = (DrawingImageScrollView)parentDIV.Superview;
      var cntOffset = grandParentDISV.ContentOffset;
      var zoomScale = grandParentDISV.ZoomScale;
      var mainScreen = UIScreen.MainScreen.Bounds;

      // physical screen dimensions in view coordinates
      var mainView = new RectangleF(cntOffset.X / zoomScale, cntOffset.Y / zoomScale, mainScreen.Width / zoomScale, mainScreen.Height / zoomScale);

      return mainView;
    }

    private void StartSegment(PointF pt)
    {
      var newSeg = new PathSegment()
                        {
                          Start = pt,
                          LineColor = LineColor,
                          LineWidth = LineWidth
                        };

      mSegments.Add(newSeg);

      mNeedsScheduledRedraw = true;
    }

    private void AddPoint(PointF pt)
    {
      if (mSegments.Count == 0)
      {
        return;
      }

      mSegments [mSegments.Count - 1].AddPoint(pt);

      mNeedsScheduledRedraw = true;
    }

    private void EndSegment(PointF pt)
    {
      if (mSegments.Count == 0)
      {
        return;
      }

      mSegments [mSegments.Count - 1].End = pt;

      mNeedsScheduledRedraw = true;
    }

    public void UndoLastSegment()
    {
      if (mSegments.Count == 0)
      {
        return;
      }

      mSegments.RemoveAt(mSegments.Count - 1);

      mNeedsScheduledRedraw = true;
    }

    public void ClearSegments()
    {
      if (mSegments.Count == 0)
      {
        return;
      }

      mSegments.Clear();

      mNeedsScheduledRedraw = true;
    }

    #endregion

    #region Touch handling

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
      base.TouchesBegan(touches, evt);

      var touch = (UITouch)touches.AnyObject;
      var pt = touch.LocationInView(this);

#if DEBUG
      Debug.WriteLine("TouchesBegan = [" + pt.X + "," + pt.Y + "]");
#endif

      if (evt.TouchesForView(this).Count == 1)
      {
        mCurrTouch = pt;

        StartSegment(pt);

        if (null != OnTouchDown)
        {
          OnTouchDown(this, null);
        }
      }
    }

    public override void TouchesMoved(NSSet touches, UIEvent evt)
    {
      base.TouchesMoved(touches, evt);

      var touch = (UITouch)touches.AnyObject;
      var pt = touch.LocationInView(this);

      if (touch.Timestamp < mLastMove + 1 / MaxPtsPerSec)
      {
        return;
      }
      mLastMove = touch.Timestamp;

#if DEBUG
      Debug.WriteLine("  TouchesMoved = [" + pt.X + "," + pt.Y + "]");
#endif

      mCurrTouch = pt;

      AddPoint(pt);
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
      base.TouchesEnded(touches, evt);

      var touch = (UITouch)touches.AnyObject;
      var pt = touch.LocationInView(this);

#if DEBUG
      Debug.WriteLine("TouchesEnded = [" + pt.X + "," + pt.Y + "]");
#endif

      mCurrTouch = PointF.Empty;

      EndSegment(pt);

      if (null != OnTouchUp)
      {
        OnTouchUp(this, null);
      }
    }

    #endregion
  }
}

