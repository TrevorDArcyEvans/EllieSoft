using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Vici.CoolStorage;

using EllieSoft.Support;

namespace Funny_Amazon_Reviews
{
  public class TableSource : UITableViewSource
  {
    private const string CellIdentifier = "TableCell";

    private readonly CSList<AmazonProductInfo> mTableItems;
    private readonly UIViewController mParentViewController;

    public TableSource(UIViewController ctrl, CSList<AmazonProductInfo> items)
    {
      mTableItems = items;
      mParentViewController = ctrl;
    }
 
    /// <summary>
    /// Called by the TableView to determine how many cells to create for that particular section.
    /// </summary>
    public override int RowsInSection(UITableView tableview, int section)
    {
      return mTableItems.Count;
    }
   
    /// <summary>
    /// Called when a row is touched
    /// </summary>
    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
      tableView.DeselectRow(indexPath, true);

      if (!Reachability.IsHostReachable("www.Amazon.com"))
      {
        new UIAlertView(Utils.GetStringFromMainBundle("CFBundleDisplayName"), "Cannot connect to Amazon", null, "OK", null).Show();

        return;
      }
     
      // show customer review
      var detail = mParentViewController.Storyboard.InstantiateViewController("CustomerReviews") as CustomerReviewsViewCtrl;
      detail.Title = mTableItems [indexPath.Row].Title;
      detail.SetItemId(mTableItems [indexPath.Row].Id);
      mParentViewController.NavigationController.PushViewController(
                detail,
                true
      );
    }
   
    /// <summary>
    /// Called by the TableView to get the actual UITableViewCell to render for the particular row
    /// </summary>
    public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
    {
      // request a recycled cell to save memory
      UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);

      var cellStyle = UITableViewCellStyle.Default;

      // if there are no cells to reuse, create a new one
      if (cell == null)
      {
        cell = new UITableViewCell(cellStyle, CellIdentifier);
      }
      cell.TextLabel.Lines = 0;
      cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

      cell.TextLabel.Text = mTableItems [indexPath.Row].Title;
      cell.ImageView.Image = UIImage.FromFile("Data/" + mTableItems [indexPath.Row].Id + ".jpg");
     
      return cell;
    }
  }
}