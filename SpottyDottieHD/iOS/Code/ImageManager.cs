using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenFlowSharp;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;

namespace SpottyDottie
{
  public class ImageManager
  {
    private const string ImageDirectory = "Full";
    private const string ThumbDirectory = "Thumbs";

    private List<string> mImages = new List<string>();
    private List<string> mThumbs = new List<string>();

    public ImageManager()
    {
      var allImgs = Directory.GetFiles(ImageDirectory);
      mImages.AddRange(allImgs);

      var allThumbs = Directory.GetFiles(ThumbDirectory);
      mThumbs.AddRange(allThumbs);

      Debug.Assert(mImages.Count == mThumbs.Count, "Must have a thumb for each image");
    }

    private string Random(List<string> lst)
    {
      var rand = new Random();
      var imgIndex = rand.Next(lst.Count);

      return lst[imgIndex];
    }

    public string RandomImage()
    {
      return Random(mImages);
    }

    public string RandomThumb()
    {
      return Random(mThumbs);
    }

    public int ImageCount
    {
      get
      {
        return mImages.Count;
      }
    }

    public string Thumbs(int index)
    {
      return mThumbs[index];
    }

    public string Images(int index)
    {
      return mImages[index];
    }

    public string ImageByThumb(string thumbPath)
    {
      return thumbPath.Replace(ThumbDirectory, ImageDirectory);
    }
  }
}

