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
using MonoTouch.AudioToolbox;
using OpenFlowSharp;

namespace SpottyDottie
{
  public class FlowViewController : UIViewController, IOpenFlowDataSource
  {
    private readonly ImageManager mImgMgr = new ImageManager();
    private readonly AutoResetEvent mSignal = new AutoResetEvent(false);
    private readonly Queue<NSAction> mTasks = new Queue<NSAction>();
    private readonly  UIActivityIndicatorView mWaitCursor = new UIActivityIndicatorView ();
    private readonly SystemSound mDrawStartSound = SystemSound.FromFile (new NSUrl ("4911__NoiseCollector__barkloud.caf"));

    private OpenFlowView mFlowView;
    private DrawingImageScrollViewController mDISVC;
    private bool mPlaySounds;

    public FlowViewController() : base()
    {
      Initialise();
    }

    public FlowViewController(IntPtr handle) : base(handle)
    {
      Initialise();
    }

    private void Initialise()
    {
      var prefs = NSUserDefaults.StandardUserDefaults;
      mPlaySounds = prefs.BoolForKey("PlaySounds");

      mDISVC = new DrawingImageScrollViewController(UIScreen.MainScreen.Bounds);

      mFlowView = new OpenFlowView(UIScreen.MainScreen.Bounds, this);

#if DEBUG
      mFlowView.Changed += delegate(object sender, EventArgs e)
      {
        Debug.WriteLine("Changed to " + mFlowView.Selected);
      };
#endif

      mFlowView.OnSingleTap += OnSingleTap;

      // Load images on demand on a worker thread
      mFlowView.NumberOfImages = mImgMgr.ImageCount;

      View = mFlowView;

      mWaitCursor.Frame = new RectangleF (0f, 0f, 50f, 50f);
      mWaitCursor.Center = View.Center;
      mWaitCursor.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
      mWaitCursor.HidesWhenStopped = true;
      View.AddSubview (mWaitCursor);

      // Start our thread queue system
      new Thread(Worker).Start();
      mSignal.Set();
    }

    private void OnSingleTap(object sender, EventArgs e)
    {
#if DEBUG
      Debug.WriteLine("OnSingleTap to " + mFlowView.Selected);
#endif

      mWaitCursor.StartAnimating();
      if (mPlaySounds)
      {
        mDrawStartSound.PlaySystemSound();
      }
      
      var thumbPath = mImgMgr.Thumbs(mFlowView.Selected);
      var imgPath = mImgMgr.ImageByThumb(thumbPath);
      mDISVC.LoadImageByName(imgPath);

      mWaitCursor.StopAnimating();

      PresentModalViewController (mDISVC, true);
    }

    #region IOpenFlowDataSource implementation

    void IOpenFlowDataSource.RequestImage(OpenFlowView view, int index)
    {
      NSAction task = delegate
      {
        var img = UIImage.FromFile(mImgMgr.Thumbs(index));
        InvokeOnMainThread(delegate
        {
          mFlowView [index] = img;
        });
      };

      lock (mTasks)
      {
        mTasks.Enqueue(task);
      }
      mSignal.Set();
    }

    UIImage IOpenFlowDataSource.GetDefaultImage()
    {
      return UIImage.FromFile("Icon@2x.png");
    }

    #endregion

    // Dispatches the tasks queued in the tasks queue
    private void Worker()
    {
      // Create the NSAutoreleasePool so that any NSObjects that
      // the ObjC runtime creates are disposed using it, otherwise
      // ObjC just leaks them.
      using (var releasePool = new NSAutoreleasePool())
      {
        while (mSignal.WaitOne())
        {
          while (true)
          {
            NSAction task;

            lock (mTasks)
            {
              if (mTasks.Count > 0)
              {
                task = mTasks.Dequeue();
              }
              else
              {
                break;
              }
            }

            task();
          }
        }
      }
    }
  }
}

