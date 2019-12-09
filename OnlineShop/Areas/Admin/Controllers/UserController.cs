using Models.DataAccess;
using Models.EntityFramework;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var DataAccess = new UserDataAccess();
            var model = DataAccess.ListAllPaging(page, pageSize);
            return View(model);
        }

        
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create (User user)
        {
            if(ModelState.IsValid)
            {
                var DataAccess = new UserDataAccess();
                var encrypteMd5 = Encryptor.MD5Hash(user.Password);
                user.Password = encrypteMd5;
                long id = DataAccess.Insert(user);
                if (id > 0)
                {
                    SetAlert("Thêm user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var DataAccess = new UserDataAccess().ViewDetail(id);

            return View(DataAccess);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var DataAccess = new UserDataAccess();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encrypteMd5 = Encryptor.MD5Hash(user.Password);
                    user.Password = encrypteMd5;
                }
                var result = DataAccess.Update(user);
                if (result)
                {
                    SetAlert("Sửa user thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var DataAccess = new UserDataAccess().ViewDetail(id);

            return View(DataAccess);
        }
        [HttpPost]
        public ActionResult Delete(User user)
        {
            if (ModelState.IsValid)
            {
                var DataAccess = new UserDataAccess();
                long id = DataAccess.Delete(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user thành công");
                }
            }
            return View("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDataAccess().ChangeStatus(id);

            return Json(new
            {
                Status = result
            });
        }
    }
}