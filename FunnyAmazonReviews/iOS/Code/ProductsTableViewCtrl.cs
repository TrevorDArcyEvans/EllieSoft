// This file has been autogenerated from parsing an Objective-C header file added in Xcode.
using System;
using System.Collections.Generic;
using System.IO;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Vici.CoolStorage;

namespace Funny_Amazon_Reviews
{
    public partial class ProductsTableViewCtrl : UITableViewController
    {
        private const string DataDirectory = "Data";
        private const string DataFile = "Amazon.db3";

        public ProductsTableViewCtrl(IntPtr handle) :
          base (handle)
        {
            string dbDirectory = Path.Combine(
                NSBundle.MainBundle.BundlePath,
                DataDirectory
            );
            string dbPath = Path.Combine(dbDirectory, DataFile);
            CSConfig.SetDB(dbPath, SqliteOption.CreateIfNotExists, () =>
            {
                CSDatabase.ExecuteNonQuery(AmazonProductInfo.CreateDbQuery);
            }
            );

            TableView.Source = new TableSource(this, AmazonProductInfo.All());
        }
    }
}
