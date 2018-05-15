using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppifyMiddleware.Model.Products
{
    public class ProductUploadStatus
    {
        public int Id { get; set; }
        public long ShopifyId { get; set; }
        public bool Uploaded { get; set; }
    }
    public class ProductUpload
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ProductType { get; set; }
        public string Vendor { get; set; }
        public string BodyHtml { get; set; }
        public bool Publish { get; set; }
        public long ShopifyId { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public DateTime? LastUpdatedUtc { get; set; }
        public byte UploadStatus { get; set; }
    }

    
}
