using Vici.CoolStorage;

namespace Funny_Amazon_Reviews
{
    [MapTo("AmazonProductInfo")]
    public class AmazonProductInfo : CSObject<AmazonProductInfo, string>
    {
            [MapTo("Id")]
        public string Id
        {
            get
            {
                return (string)GetField("Id");
            }
            set
            {
                SetField("Id", value);
            }
        }

        // ItemLookupRequestIdType
        [MapTo("IdType")]
        public string IdType
        {
            get
            {
                return (string)GetField("IdType");
            }
            set
            {
                SetField("IdType", value);
            }
        }

        // Item.ItemAttributes.Title
        [MapTo("Title")]
        public string Title
        {
            get
            {
                return (string)GetField("Title");
            }
            set
            {
                SetField("Title", value);
            }
        }

        // Item.DetailPageURL
        [MapTo("DetailsUrl")]
        public string DetailsUrl
        {
            get
            {
                return (string)GetField("DetailsUrl");
            } 
            set
            {
                SetField("DetailsUrl", value);
            }
        }

        // Item.CustomerReviews
        [MapTo("CustomerReviewsUrl")]
        public string CustomerReviewsUrl
        {
            get
            {
                return (string)GetField("CustomerReviewsUrl");
            } 
            set
            {
                SetField("CustomerReviewsUrl", value);
            }
        }

        public static string CreateDbQuery
        {
            get
            {
                return @"CREATE TABLE AmazonProductInfo 
                        (Id TEXT PRIMARY KEY NOT NULL UNIQUE,
                        IdType TEXT NOT NULL,
                        Title TEXT NOT NULL,
                        DetailsUrl TEXT,
                        CustomerReviewsUrl TEXT)";
            }
        }
    }
}
