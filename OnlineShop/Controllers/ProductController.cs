using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDataAccess();
            var productcategory = model.ListAll();
            return PartialView(productcategory);
        }

        public ActionResult Category (long id)
        {
            var category = new ProductDataAccess().ViewDetail(id);
            return View(category);
        }

        public ActionResult Detail (long id)
        {
            var product = new ProductDataAccess().ViewDetail(id);
            return View(product);
        }
    }
}