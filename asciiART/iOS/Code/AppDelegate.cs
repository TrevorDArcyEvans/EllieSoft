using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.FacebookConnect;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using EllieSoft.Support;

namespace asciiART
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the
  // User Interface of the application, as well as listening (and optionally responding) to
  // application events from iOS.
  [Register ("AppDelegate")]
  public partial class AppDelegate : UIApplicationDelegate, IFacebookApp
  {
    // class-level declarations

    public Facebook Facebook { get; set; }

    public override UIWindow Window
    {
      get;
      set;
    }

    // This method is invoked when the application is about to move from active to inactive state.
    // OpenGL applications should use this method to pause.
    public override void OnResignActivation(UIApplication application)
    {
    }

    // This method should be used to release shared resources and it should store the application state.
    // If your application supports background exection this method is called instead of WillTerminate
    // when the user quits.
    public override void DidEnterBackground(UIApplication application)
    {
    }

    // This method is called as part of the transiton from background to active state.
    public override void WillEnterForeground(UIApplication application)
    {
    }

    // This method is called when the application is about to terminate. Save data, if needed.
    public override void WillTerminate(UIApplication application)
    {
    }

    // Pre-4.2 callback
    // This method is called back when Facebook has authenticated us in the Web UI
    public override bool HandleOpenURL(UIApplication application, NSUrl url)
    {
      return Facebook.HandleOpenURL(url);
    }

    // Post 4.2 callback
    public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
    {
      return Facebook.HandleOpenURL(url);
    }
  }
}

