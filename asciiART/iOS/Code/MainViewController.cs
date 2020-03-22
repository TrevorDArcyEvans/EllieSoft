using MonoTouch.UIKit;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using MonoTouch.Foundation;
using StaticDust;
using EllieSoft.Support;

namespace asciiART
{
  public partial class MainViewController : UIViewController
  {
    public MainViewController(IntPtr handle) :
      base (handle)
    {
      // Custom initialization
    }

    public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
    {
      // Return true for supported orientations
      return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
    }

    public override void DidReceiveMemoryWarning()
    {
      // Releases the view if it doesn't have a superview.
      base.DidReceiveMemoryWarning();

      // Release any cached data, images, etc that aren't in use.
    }

    #region View lifecycle

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // clean out previous web page files
      var tempFiles = Directory.EnumerateFiles(Path.GetTempPath());
      foreach (var thisTempFile in tempFiles)
      {
        File.Delete(thisTempFile);
      }

      // disable Go button until user picks image
      mGoCmd.Enabled = mGoCmd.UserInteractionEnabled = false;

      mImageSelector.NavigationController = this;

      mImageSelector.Image = UIImage.FromFile("LoadImagePrompt.png");
    }

    public override void ViewDidUnload()
    {
      base.ViewDidUnload();

      // Clear any references to subviews of the main view in order to
      // allow the Garbage Collector to collect them sooner.
      //
      // e.g. myOutlet.Dispose (); myOutlet = null;

      ReleaseDesignerOutlets();
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);
    }

    public override void ViewWillDisappear(bool animated)
    {
      base.ViewWillDisappear(animated);
    }

    public override void ViewDidDisappear(bool animated)
    {
      base.ViewDidDisappear(animated);
    }

    #endregion

    private void CreateAsciiArt(AsciiPageSize selectedPageSize)
    {
      // calculate block size
      const int FudgeFactor = 5;
      var blockWidth = mImageSelector.Image.Size.Width / selectedPageSize.Width;
      var blockHeight = mImageSelector.Image.Size.Height / selectedPageSize.Height;
      var blockSize = FudgeFactor * Math.Max(blockWidth, blockHeight);
      blockSize = Math.Max(1f, blockSize);
      
      // show progress bar
      var progress = new CancellableProgressingView("Converting ...");
      progress.Show();
      
      // convert to ascii
      var filename = Helpers.GetWebFileName(Path.GetTempPath());
      using (var fs = new StreamWriter(filename))
      {
        AsciiArt.ConvertImage(mImageSelector.Image, fs, (int)blockSize, 5, false, true, progress);
      }
      progress.Hide();
      
      if (progress.Cancel)
      {
        return;
      }
      
      // save to file
      //File.WriteAllText(filename, browserStr);
      
      var results = (FlipsideViewController)this.Storyboard.InstantiateViewController("FlipsideViewController");
      results.WebFilePath = filename;
      
      PresentModalViewController(results, true);
    }

    partial void OnGoTouchUpInside(MonoTouch.UIKit.UIButton sender)
    {
      List<AsciiPageSize> sizes = new List<AsciiPageSize>
                                        {
                                          new AsciiPageSize("micro", 320, 240),
                                          new AsciiPageSize("tiny", 480, 360),
                                          new AsciiPageSize("small", 640, 480),
                                          new AsciiPageSize("medium", 800, 600),
                                          new AsciiPageSize("large", 1024, 768),
                                          new AsciiPageSize("extra large", 1280, 1024),
                                          new AsciiPageSize("super large", 1600, 1200)
                                        };
      var sizesArr = from thisSize in sizes select thisSize.Name;
      var actionSheet = new UIActionSheet ("Select size", null, "Cancel", null, sizesArr.ToArray())
                              {
                                Style = UIActionSheetStyle.Default
                              };
      actionSheet.Clicked += delegate (object actionSheetSender, UIButtonEventArgs args)
                              {
                                var index = args.ButtonIndex;
                                Console.WriteLine ("Clicked on item {0}", index);
                                if (index < sizes.Count)
                                {
                                  actionSheet.DismissWithClickedButtonIndex(sizes.Count, true);
                                  CreateAsciiArt(sizes[index]);
                                }
                              };
      
      actionSheet.ShowInView (View);
    }

    public void HasPickedImage()
    {
      mGoCmd.Enabled = mGoCmd.UserInteractionEnabled = true;
    }
  }
}

