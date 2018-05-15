using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ShoppifyMiddleware.Services;
using ShoppifyMiddleware.Model.Products;
namespace ShoppifyMiddleware.Controllers
{
    public class ShopifyProductController : ApiController
    {
        static IProductService _productService;

        static ShopifyProductController() { _productService = new ProductService(); }

        //public ShopifyProductController(IProductService productService)
        //{
        //    _productService = productService;
        //}

        [HttpGet]
        public List<ProductUpload> GetProducts(int ProductStatus = 0)
        {
            return _productService.GetList(ProductStatus).ToList();
        }

        [HttpGet]
        public List<ProductUploadStatus> UploadProducts()
        {
            return _productService.UploadProducts();
        }

        //[HttpPost]
        //public async Task<Product> SaveProduct()
        //{
        //    var service = new ProductService("https://18749bed806ed197c1903e67b36bb221:1ea0b4f49ff131e180a2e7b7e5cba0d6@amore-by-leva.myshopify.com", "1ea0b4f49ff131e180a2e7b7e5cba0d6");
        //    return await service.CreateAsync(new Product()
        //    {
        //        Title = "ASP.Net Subscription",
        //        ProductType = "",
        //        PublishedAt = DateTime.UtcNow,
        //        Vendor = "ASP.Net",
        //        BodyHtml = "<b>Such a nice product for this price</b>",
        //    }, new ProductCreateOptions() { Published = true });
        //}
        //public async Task<IEnumerable<Product>> GetProduct()
        //{
        //    try
        //    {
        //        var service = new ProductService("https://18749bed806ed197c1903e67b36bb221:1ea0b4f49ff131e180a2e7b7e5cba0d6@amore-by-leva.myshopify.com", "1ea0b4f49ff131e180a2e7b7e5cba0d6");
        //        var list = await service.ListAsync(new ProductFilter() { Limit = 5 });
        //        return list;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}

        //public async Task<Shop> GetShop()
        //{
        //    try
        //    {
        //        var service = new ShopService("https://18749bed806ed197c1903e67b36bb221:1ea0b4f49ff131e180a2e7b7e5cba0d6@amore-by-leva.myshopify.com", "1ea0b4f49ff131e180a2e7b7e5cba0d6");
        //        return await service.GetAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
    }
}
