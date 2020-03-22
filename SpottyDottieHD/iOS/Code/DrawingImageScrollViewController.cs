using System;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace SpottyDottie
{
  public class DrawingImageScrollViewController : UIViewController
  {
    private DrawingImageScrollView mImageScrollView;
    private ColourPickerView mColourPicker;
    private UISlider mLineWidth;

    public DrawingImageScrollViewController(RectangleF bounds) : base()
    {
      Initialise();
    }

    public DrawingImageScrollViewController(IntPtr handle) : base(handle)
    {
      Initialise();
    }

    private readonly UIToolbar mToolbar = new UIToolbar();

    private void Initialise()
    {
      mImageScrollView = new DrawingImageScrollView(View.Bounds);
      mImageScrollView.OnTouchDown += OnTouchDownInternal;
      mImageScrollView.OnTouchUp += OnTouchUpInternal;
      View.AddSubview(mImageScrollView);

      // toolbar + buttons
      var imgBack = UIImage.FromFile("Menubar/Back.png");
      var itemBack = new UIBarButtonItem(imgBack, UIBarButtonItemStyle.Done, OnBackTouchUp);

      var imgSave = UIImage.FromFile("Menubar/Save.png");
      var itemSave = new UIBarButtonItem(imgSave, UIBarButtonItemStyle.Done, OnSaveTouchUp);

      var imgColour = UIImage.FromFile("Menubar/ColourPicker.png");
      var itemColour = new UIBarButtonItem(imgColour, UIBarButtonItemStyle.Done, OnColourTouchUp);

      var imgWidth = UIImage.FromFile("Menubar/Brush.png");
      var itemWidth = new UIBarButtonItem(imgWidth, UIBarButtonItemStyle.Done, OnWidthTouchUp);

      var imgUndo = UIImage.FromFile("Menubar/Undo.png");
      var itemUndo = new UIBarButtonItem(imgUndo, UIBarButtonItemStyle.Done, OnUndoTouchUp);

      var imgTrash = UIImage.FromFile("Menubar/Trash.png");
      var itemClear = new UIBarButtonItem(imgTrash, UIBarButtonItemStyle.Done, OnClearTouchUp);

      mToolbar.Items = ToolbarItems = new UIBarButtonItem[] { itemBack, itemSave, itemColour, itemWidth, itemUndo, itemClear };
      mToolbar.SizeToFit();
      View.AddSubview(mToolbar);

      // colour picker
      mColourPicker = new ColourPickerView();
      mColourPicker.Frame = new RectangleF(mColourPicker.Bounds.Left, mToolbar.Bounds.Bottom, mColourPicker.Bounds.Width, mColourPicker.Bounds.Height);
      mColourPicker.OnColourChanged += OnColourChanged;
      View.AddSubview(mColourPicker);
      View.SendSubviewToBack(mColourPicker);

      // line width
      mLineWidth = new UISlider (new RectangleF (160f, mToolbar.Bounds.Bottom + 3, 150f, 9f))
                        {
                          BackgroundColor = UIColor.Clear,
                          MinValue = 0f,
                          MaxValue = 100f,
                          Continuous = true,
                          Value = 6f
                        };
      mLineWidth.ValueChanged += OnWidthChanged;
      View.AddSubview(mLineWidth);
      View.SendSubviewToBack(mLineWidth);
    }

    #region Touch handling

    private void OnBackTouchUp(object sender, EventArgs evt)
    {
      DismissModalViewControllerAnimated (true);
    }

    private void OnSaveTouchUp(object sender, EventArgs evt)
    {
      var imgView = mImageScrollView.ViewForZoomingInDrawingImageScrollView(mImageScrollView);
      var img = PngSaver.GetPng(imgView);
      img.SaveToPhotosAlbum((image, err) =>
              {
                var alert = new UIAlertView()
                              {
                                Title = "Information",
                                Message = "Saved to Photos."
                              };
                alert.AddButton("Ok");
                alert.Show();
              });
    }

    private void OnColourTouchUp(object sender, EventArgs evt)
    {
      mColourPicker.Color = mImageScrollView.LineColor;
      View.BringSubviewToFront(mColourPicker);
    }

    private void OnColourChanged(object sender, EventArgs evt)
    {
      mImageScrollView.LineColor = mColourPicker.Color;
      View.SendSubviewToBack(mColourPicker);
    }

    private void OnWidthTouchUp(object sender, EventArgs evt)
    {
      mLineWidth.Value = mImageScrollView.LineWidth;
      View.BringSubviewToFront(mLineWidth);
    }

    private void OnWidthChanged(object sender, EventArgs evt)
    {
      mImageScrollView.LineWidth = mLineWidth.Value;
    }

    private void OnUndoTouchUp(object sender, EventArgs evt)
    {
      mImageScrollView.UndoLastSegment();
    }

    private void OnClearTouchUp(object sender, EventArgs evt)
    {
      mImageScrollView.ClearSegments();
    }

    void OnTouchDownInternal (object sender, EventArgs e)
    {
      // bit of a hack as there is no notification when user has finished with slider
      mImageScrollView.LineWidth = mLineWidth.Value;
      View.SendSubviewToBack(mLineWidth);

      View.SendSubviewToBack(mColourPicker);

      View.SendSubviewToBack(mToolbar);
    }

    void OnTouchUpInternal (object sender, EventArgs e)
    {
      View.BringSubviewToFront(mToolbar);
    }

    #endregion

    public void LoadImageByName(string imagePath)
    {
      mImageScrollView.LoadImageByName(imagePath);
    }

    #region Shake handling

    public override bool CanBecomeFirstResponder
    {
      get
      {
        return true;
      }
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      BecomeFirstResponder();
    }

    public override void ViewDidDisappear(bool animated)
    {
      base.ViewDidDisappear(animated);

      ResignFirstResponder();
    }

    public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
    {
      return false;
    }

    public override void MotionEnded(UIEventSubtype motion, UIEvent evt)
    {
      base.MotionEnded(motion, evt);

      if (evt.Subtype == UIEventSubtype.MotionShake)
      {
#if DEBUG
        Debug.WriteLine("**shake**");
#endif

        mImageScrollView.ClearSegments();
      }
    }

    #endregion

  }
}

