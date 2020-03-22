using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace asciiART
{
  public partial class ImageSelectorView : UIImageView
  {
    private readonly UIImagePickerController mImagePicker = new UIImagePickerController();

    public MainViewController NavigationController { get; set; }

    public ImageSelectorView(IntPtr handle) :
      base (handle)
    {
    }

    public override void TouchesEnded(NSSet touches, UIEvent evt)
    {
      if (touches == null)
      {
        throw new ArgumentNullException("touches");
      }
      if (evt == null)
      {
        throw new ArgumentNullException("evt");
      }
      if (touches.Count != 1)
      {
        return;
      }

      mImagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
      mImagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

      mImagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
      mImagePicker.Canceled += Handle_Canceled;

      NavigationController.PresentModalViewController(mImagePicker, true);
    }

    private void Handle_Canceled(object sender, EventArgs e)
    {
      mImagePicker.DismissModalViewControllerAnimated(true);
    }

    private void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
    {
      // determine what was selected, video or image
      bool isImage = false;
      switch (e.Info [UIImagePickerController.MediaType].ToString())
      {
        case "public.image":
          isImage = true;
          break;

        case "public.video":
          break;
      }

      // get common info (shared between images and video)
      NSUrl referenceURL = e.Info [new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
           
      // if it was an image, get the other image info
      if (isImage)
      {
        // get the original image
        UIImage originalImage = e.Info [UIImagePickerController.OriginalImage] as UIImage;
        if (originalImage != null)
        {
          // display the image
          this.Image = originalImage;

          NavigationController.HasPickedImage();
        }
      }
      else
      {
        // if it's a video
        // get video url
        NSUrl mediaURL = e.Info [UIImagePickerController.MediaURL] as NSUrl;
      }

      // dismiss the picker
      mImagePicker.DismissModalViewControllerAnimated(true);
    }
  }
}
