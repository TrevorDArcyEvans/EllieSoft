using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using EllieSoft.Support;

namespace asciiART
{
  public partial class FlipsideViewController : UIViewController, IAsyncActions
  {
    private IUIInfo SelectedDestination;

    public FlipsideViewController(IntPtr handle) :
      base (handle)
    {
    }

    public FlipsideViewController() :
      base()
    {
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

      if (!string.IsNullOrEmpty(WebFilePath))
      {
        mResultsView.LoadRequest(new NSUrlRequest(new NSUrl(WebFilePath, false)));
        mResultsView.ScalesPageToFit = true;
      }
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

      if (File.Exists(WebFilePath))
      {
        File.Delete(WebFilePath);
      }
      WebFilePath = string.Empty;
    }

    #endregion

    public UIWebView WebPage
    {
      get
      {
        return mResultsView;
      }
    }

    public string WebFilePath { get; set; }

    public Action Success()
    {
      return OnUploadSuccess;
    }

    public Action Completed()
    {
      return OnUploadCompleted;
    }

    public Action Failure()
    {
      return OnUploadFailure;
    }

    private void OnUploadCompleted()
    {
    }

    private void OnUploadSuccess()
    {
    }

    private void OnUploadFailure()
    {
      new UIAlertView("asciify", "Failed to upload with " + SelectedDestination.Name, null, "OK", null).Show();
    }

    private void Share(IUIInfo selDest)
    {
      var lv = new LoadingView("Retrieving image");

      SelectedDestination = selDest;

      if (selDest is ISocialSharer)
      {
        var selSocialSharer = (ISocialSharer)selDest;
        selSocialSharer.Share(this);
        
        return;
      }
      
      // this is a mess
      if (selDest is IFileSharingUploader)
      {
        var selFileSharer = (IFileSharingUploader)selDest;
        var docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var myFilename = Helpers.GetWebFileName(docs);
        selFileSharer.Upload(WebFilePath, myFilename, this);
        
        return;
      }
      
      if (selDest is IPhotoAlbumUploader)
      {
        var selPhotoSharer = (IPhotoAlbumUploader)selDest;
        lv.Show();
        var img = Helpers.GetImageFromWebPage(WebPage, lv);
        lv.Hide();
        selPhotoSharer.Upload(img, this);
        
        return;
      }
      
      if (selDest is DropboxUploader)
      {
        var selDropBox = (DropboxUploader)selDest;
        selDropBox.Upload(WebFilePath, this);
        
        return;
      }
      
      throw new ArgumentOutOfRangeException("Unsupported sharer: " + selDest.Name);
    }

    partial void OnUploadTouchUpInside(MonoTouch.UIKit.UIButton sender)
    {
      var dests = new List<IUIInfo>(new IUIInfo[] 
                                  {
                                    new EmailSharer(this),
                                    new TwitterSharer(this),
                                    new PhotoAlbumUploader(this),
                                    new FileSharingUploader(this),
                                    new FacebookSharer(this)
                                  });
      var destsArr = from thisDest in dests select thisDest.Name;
      var actionSheet = new UIActionSheet ("Share to", null, "Cancel", null, destsArr.ToArray())
      {
        Style = UIActionSheetStyle.Default
      };
      actionSheet.Clicked += delegate (object actionSheetSender, UIButtonEventArgs args)
      {
        var index = args.ButtonIndex;
        Console.WriteLine ("Clicked on item {0}", index);
        if (index < dests.Count)
        {
          actionSheet.DismissWithClickedButtonIndex(dests.Count, true);
          Share(dests[index]);
        }
      };
      
      actionSheet.ShowInView (View);
    }
  }
}

