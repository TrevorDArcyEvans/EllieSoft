using System;
using System.Drawing;

namespace SpottyDottie
{
  public static class Extensions
  {
    public static PointF Add(this PointF pt1, PointF pt2)
    {
      pt1.X += pt2.X;
      pt1.Y += pt2.Y;

      return pt1;
    }
  }
}

