using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AudioToolbox;

namespace SpottyDottie
{
  public class Application
  {
    static void Main(string[] args)
    {
      UIApplication.Main(args);
    }
  }

  // The name AppDelegate is referenced in the MainWindow.xib file.
  public partial class AppDelegate : UIApplicationDelegate
  {
    // This method is invoked when the application has loaded its UI and its ready to run
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      AudioSession.Initialize();
      AudioSession.Category = AudioSessionCategory.MediaPlayback;   
      AudioSession.SetActive(true);

      app.ApplicationSupportsShakeToEdit = false;

      // If you have defined a view, add it here:
      window.AddSubview(rootController.View);

      window.MakeKeyAndVisible();

      return true;
    }

    // This method is required in iPhoneOS 3.0
    public override void OnActivated(UIApplication application)
    {
    }
  }

  public partial class AppDelegate_iPad : AppDelegate
  {
  }
}

