using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DataAccess;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string email, string address, string content)
        {
            var feedback = new FeedBack();
            feedback.Name = name;
            feedback.Phone = mobile;
            feedback.Email = email;
            feedback.Address = address;
            feedback.Content = content;
            feedback.CreateDate = DateTime.Now;
            var id = new ContactDataAccess().InsertFeedBack(feedback);
            if(id > 0)
            {
                return Json(new { status = true });
            }
            else
            {
                return Json(new { status = false });
            }

            
        }
    }
}