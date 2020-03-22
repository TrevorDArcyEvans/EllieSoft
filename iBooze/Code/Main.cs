
using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace iBooze
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
      tabBarItemiBooze.Image = UIImage.FromFile("drunken_duck_Beer_2_32x32.png");
      tabBarItemInfo.Image = UIImage.FromFile("missiridia_Universal_information_symbol_32x32.png");
      tabBarItemAbout.Image = UIImage.FromFile("Copyright.png");

      var aboutUrl = new NSUrlRequest(NSUrl.FromFilename("copyright.html"));
      AboutPage.LoadRequest(aboutUrl);

      var infoUrl = new NSUrlRequest(NSUrl.FromFilename("info.html"));
      InfoPage.LoadRequest(infoUrl);

      // If you have defined a view, add it here:
      window.AddSubview (tabBarController.View);

      window.MakeKeyAndVisible();
      
      return true;
    }

    // This method is required in iPhoneOS 3.0
    public override void OnActivated(UIApplication application)
    {
      Recalculate();
    }

    private void Recalculate()
    {
      try
      {
        txtUnitCost.Text = string.Empty;
        
        var CartonCost = float.Parse(txtCartonCost.Text);
        var ABV = float.Parse(txtABV.Text);
        var UnitsPerCarton = float.Parse(txtUnitsPerCarton.Text);
        var UnitVolume = float.Parse(txtUnitVolume.Text);
        
        var UnitCost = CartonCost / (ABV / 100.0 * UnitsPerCarton * UnitVolume / 1000.0);
        txtUnitCost.Text = String.Format("{0:0.00}", UnitCost);
      }
      catch (Exception)
      {
        // swallow number parse exceptions
      }
    }

    partial void DidEndOnExit(MonoTouch.UIKit.UITextField sender)
    {
      Recalculate();

      // hide keyboard
      sender.ResignFirstResponder();
    }
  }
}

