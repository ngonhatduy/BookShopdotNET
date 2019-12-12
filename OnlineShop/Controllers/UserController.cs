using Models.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DataAccess;
using OnlineShop.Models;
using OnlineShop.Common;
using System.Xml.Linq;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string userName, string name, string phone, string email, string address, int provinceID, int districtID, string password, string repeatPassword)
        {
            var user = new User();
            user.UserName = userName;
            user.Name = name;
            user.Phone = phone;
            user.Email = email;
            user.Address = address;
            user.ProvinceID = provinceID;
            user.DistrictID = districtID;
            password = Encryptor.MD5Hash(password);
            user.Password = password;;
            user.CreateDate = DateTime.Now;
            user.Status = true;
            var userInsert = new UserDataAccess().Insert(user);
            return RedirectToAction("Login", "User");
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var DataAccess = new UserDataAccess();
                var result = DataAccess.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = DataAccess.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoảng đang bị khóa.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập lỗi.");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult LoadProvince()
        {
            var xmlLoadData = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            var province = xmlLoadData.Elements("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<Province>();
            Province getProvince = null;
            foreach(var item in province)
            {
                getProvince = new Province();
                getProvince.ID = int.Parse(item.Attribute("id").Value);
                getProvince.Name = item.Attribute("value").Value;
                list.Add(getProvince);
            }
            return Json(
                new
                {
                    data = list,
                    status = true
                });
        }

        [HttpPost]
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlLoadData = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            //var district = xmlLoadData.Elements("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province" && x.Attribute("id").Value == provinceID);
            var district = xmlLoadData.Elements("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<District>();
            District getDistrict = null;
            foreach (var item in district.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                getDistrict = new District();
                getDistrict.ID = int.Parse(item.Attribute("id").Value);
                getDistrict.Name = item.Attribute("value").Value;
                getDistrict.Province = int.Parse(item.Attribute("id").Value);
                list.Add(getDistrict);
            }
            return Json(
                new
                {
                    data = list,
                    status = true
                });
        }

        public JsonResult LoadPrecinct(int districtID)
        {
            var xmlLoadData = XDocument.Load(Server.MapPath(@"~/assets/client/data/Provinces_Data.xml"));
            //var district = xmlLoadData.Elements("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province" && x.Attribute("id").Value == provinceID);
            var precinct = xmlLoadData.Elements("Root").Elements("Item").Elements("Item").Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == districtID);
            var list = new List<Precinct>();
            Precinct getPrecinct = null;
            foreach (var item in precinct.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                getPrecinct = new Precinct();
                getPrecinct.ID = int.Parse(item.Attribute("id").Value);
                getPrecinct.Name = item.Attribute("value").Value;
                getPrecinct.District = int.Parse(item.Attribute("id").Value);
                list.Add(getPrecinct);
            }
            return Json(
                new
                {
                    data = list,
                    status = true
                });
        }
    }
}