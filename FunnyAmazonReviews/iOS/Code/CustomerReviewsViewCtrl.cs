// This file has been autogenerated from parsing an Objective-C header file added in Xcode.
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Funny_Amazon_Reviews
{
  public partial class CustomerReviewsViewCtrl : UIViewController
  {
    private string mItemId;

    public CustomerReviewsViewCtrl(IntPtr handle) : base (handle)
    {
    }

    public void SetItemId(string itemId)
    {
      mItemId = itemId;
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      UIApplication.SharedApplication.NetworkActivityIndicatorVisible = true;
      var customerReviewUrl = GetCustomerReviewUrl(mItemId);
      var url = new NSUrl(customerReviewUrl);
      var req = new NSUrlRequest(url);
      mCustomerReviews.LoadRequest(req);
      mCustomerReviews.LoadFinished += (sender, e) => 
      {
        UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
      };
    }

    private static string GetCustomerReviewUrl(string itemId)
    {
      const string AWS_ACCESS_KEY_ID = "AKIAI36KF4DVLLEGP4GA";
      const string AWS_SECRET_KEY = "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";
      const string DESTINATION = "ecs.amazonaws.com";
      const string NAMESPACE = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";

      SignedRequestHelper helper = new SignedRequestHelper(
        AWS_ACCESS_KEY_ID,
        AWS_SECRET_KEY,
        DESTINATION
      );

      /*
       * Here is an ItemLookup example where the request is stored as a dictionary.
      */
      IDictionary<string, string> r1 = new Dictionary<string, String>();
      r1 ["Service"] = "AWSECommerceService";
      r1 ["Version"] = "2011-08-01";
      r1 ["Operation"] = "ItemLookup";
      r1 ["ItemId"] = itemId;
      r1 ["ResponseGroup"] = "Small, Reviews, EditorialReview, Images, ItemAttributes";
      r1 ["AssociateTag"] = "123";

      String url = helper.Sign(r1);

      try
      {
        WebRequest request = HttpWebRequest.Create(url);
        WebResponse response = request.GetResponse();
        XmlDocument doc = new XmlDocument();
        doc.Load(response.GetResponseStream());

        XmlNodeList errorMessageNodes = doc.GetElementsByTagName(
          "Message",
          NAMESPACE
        );
        if (errorMessageNodes != null && errorMessageNodes.Count > 0)
        {
          String message = errorMessageNodes.Item(0).InnerText;
          Console.WriteLine("Error: " + message + " (but signature worked)");

          return string.Empty;
        }

        XmlNode customerReviewsIFrameNode = doc.GetElementsByTagName(
          "IFrameURL",
          NAMESPACE
        )
          .Item(0);
        string iFrameUrl = customerReviewsIFrameNode.InnerText;

        return iFrameUrl;
      }
      catch (Exception e)
      {
        System.Console.WriteLine("Caught Exception: " + e.Message);
        System.Console.WriteLine("Stack Trace: " + e.StackTrace);
      }

      return string.Empty;
    }
  }
}
