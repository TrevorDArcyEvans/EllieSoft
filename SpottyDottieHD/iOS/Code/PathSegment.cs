using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace SpottyDottie
{
  public class PathSegment
  {
    public float LineWidth;
    public CGColor LineColor;
    public PointF Start;
    private readonly List<PointF> Points = new List<PointF>(1000);
    public PointF End;

    public void AddPoint(PointF pt)
    {
      Points.Add(pt);
    }

    public void Draw(CGContext ctx, RectangleF rect)
    {
      try
      {
        ctx.SaveState();
        ctx.BeginPath();

        ctx.SetLineWidth(LineWidth);
        ctx.SetStrokeColorWithColor(LineColor);
        ctx.SetLineCap(CGLineCap.Round);

        ctx.MoveTo(Start.X, Start.Y);

        var path = new CGPath();
        path.AddLines(Points.ToArray());

        ctx.AddPath(path);
      }
      finally
      {
        ctx.StrokePath();
        ctx.RestoreState();
      }
    }
  }
}

