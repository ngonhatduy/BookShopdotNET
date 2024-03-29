﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DataAccess;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                var DataAccess = new UserDataAccess();
                var result = DataAccess.Login(model.UserName, Encryptor.MD5Hash(model.Password), true);
                if(result == 1)
                {
                    var user = DataAccess.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if(result == -1)
                {
                    ModelState.AddModelError("", "Tài khoảng đang bị khóa.");
                }
                else if(result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập vào trang quản trị."); //Do user không phải là admin
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập lỗi.");
                }
            }
            return View("Index");
        }
    }
}