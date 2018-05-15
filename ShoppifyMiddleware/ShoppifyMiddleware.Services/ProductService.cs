using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ShoppifyMiddleware.Model;
using ShoppifyMiddleware.DataBase;
using ShoppifyMiddleware.Model.Products;
using ShoppifyMiddleware.Services.Setup;
namespace ShoppifyMiddleware.Services
{
    public interface IProductService
    {
        IList<ProductUpload> GetList(int Status = 0);
        List<ProductUploadStatus> UploadProducts();
    }
    public class ProductService : IProductService
    {
        static IGenericRepository<ShopifyProduct> _genericRepo;
        //private readonly IGenericRepository<ShopifyProduct> _genericRepo;

        //public ProductService(IGenericRepository<ShopifyProduct> genericRepo)
        //{
        //    this._genericRepo = genericRepo;
        //}

        static ProductService()
        {
            _genericRepo = new GenericRepository<ShopifyProduct>();
        }

        public virtual IList<ProductUpload> GetList(int Status = 0)
        {
            var List = new List<ProductUpload>();
            var Query = _genericRepo.FindBy(f => f.UploadStatus == Status);
            foreach (var item in Query)
            {
                var product = new ProductUpload();
                item.CopyPropertiesTo(product);
                List.Add(product);
            }
            return List;
        }

        public virtual List<ProductUploadStatus> UploadProducts()
        {
            var List = new List<ProductUploadStatus>();
            var Query = _genericRepo.FindBy(f => f.UploadStatus == (int)ProductStatus.ReadyToBeUploaded).ToList();
            foreach (var itemProduct in Query)
            {
                var status =  new ProductUploadStatus();
                status.Id = itemProduct.Id;
                var uploadedProdct = Task.Run<ShopifySharp.Product>(async () => await UploadProduct(itemProduct));
                //UploadProduct(itemProduct);
                if (uploadedProdct.Id > 0)
                {
                    status.ShopifyId = uploadedProdct.Id;
                    status.Uploaded = true;
                    itemProduct.ShopifyId = uploadedProdct.Id;
                    itemProduct.UploadStatus = (int) ProductStatus.Uploaded;
                    itemProduct.LastUpdatedUtc = DateTime.UtcNow;
                    _genericRepo.Edit(itemProduct);
                }
                List.Add(status);
            }

            return List;
        }

        private async Task<ShopifySharp.Product> UploadProduct(ShopifyProduct product)
        {
            try
            {
                var service = new ShopifySharp.ProductService("https://18749bed806ed197c1903e67b36bb221:1ea0b4f49ff131e180a2e7b7e5cba0d6@amore-by-leva.myshopify.com", "1ea0b4f49ff131e180a2e7b7e5cba0d6");
                var productToUpload = new ShopifySharp.Product()
                {
                    Title = product.Title,
                    ProductType = product.ProductType,
                    Vendor = "ASP.Net",
                    BodyHtml = "<b>Such a nice product for this price</b>",
                };
                if (product.Publish)
                {
                    productToUpload.PublishedAt = DateTimeOffset.Now;
                }
                return await service.CreateAsync(productToUpload, new ShopifySharp.ProductCreateOptions() { Published = product.Publish });
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
