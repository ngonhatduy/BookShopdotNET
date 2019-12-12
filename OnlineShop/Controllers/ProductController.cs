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
            var product = new ProductDataAccess().ViewProductDetail(id);
            var relatedProduct = new ProductDataAccess().ListRelatedProduct(id);
            ViewBag.RelatedProduct = relatedProduct;
            return View(product);
        }
        public ActionResult Search (string keyword, int page = 1, int pageSize = 1)
        {
            int totalRecord = 0;
            var model = new ProductDataAccess().Search(keyword, ref totalRecord, pageSize);
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;
            ViewBag.Keyword = keyword;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize));
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(model);
        }

        public JsonResult ListName(string q)
        {
           var data = new ProductDataAccess().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}