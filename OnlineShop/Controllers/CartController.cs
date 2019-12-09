using Models.DataAccess;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.EntityFramework;
using System.Configuration;
using Common;
using System.Web.Script.Serialization;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(long productId, int Quantity)
        {
            var product = new ProductDataAccess().ViewProductDetail(productId);
            var cart = Session[CartSession];
            //đã có sản phẩm trong giỏ hàng
            if(cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity ++;
                        }
                    }
                }
                else
                {
                    var item2 = new CartItem();
                    item2.Product = product;
                    item2.Quantity = Quantity;
                    list.Add(item2);
                }
            }
            //chưa có sp trong giỏ
            else
            {
                //Tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = Quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("index");
        }

        public JsonResult DeleteItem(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new { status = true });
        }

        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel); //data lấy từ website
            var sessionCart = (List<CartItem>)Session[CartSession];
            foreach(var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if(jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new { status = true });
        }

        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public string ListProductEmail(List<string>product)
        {
            string listProduct = "";
            foreach (var i in product)
            { 
                listProduct += i;
                listProduct += Environment.NewLine;
            }
            
            return listProduct;
        }

        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            //Lấy data, kiểu data là Order
            var order = new Order();
            order.ShipName = shipName;
            order.ShipMobile = mobile;
            order.ShipAddress = address;
            order.ShipEmail = email;
            //Insert data vừa lấy vào bảng Order
            var id = new OrderDataAccess().Insert(order);
            ////
            
            var OrderDetail = new OrderDetailDataAccess();

            //Lấy data từ session hiện tại
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            ////
            
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            decimal total = 0;
            decimal totalAll = 0;
            var listProduct = new List<string>();
            string product = "";
            //Them data vào bảng OrderDetail
            foreach (var item in list)
            {
                var detail = new OrderDetail();
                detail.ProductID = item.Product.ID;
                detail.OrderID = id;
                detail.Quantity = item.Quantity;
                detail.Price = item.Product.Price;
                OrderDetail.Insert(detail);
                total = item.Product.Price.GetValueOrDefault(0) * item.Quantity;

                product += "Sản phẩm:" + item.Product.Name + '\n' + "Số lượng:" + item.Quantity + Environment.NewLine + "Giá:" + item.Product.Price + Environment.NewLine + "Thành tiền:" + item.Product.Price * item.Quantity + Environment.NewLine;
                listProduct.Add(product);
                product = "";

                totalAll += total;
            }
            //Tạo đơn hàng email
            string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));
            content = content.Replace("{{CustomerName}}", shipName);
            content = content.Replace("{{Phone}}", mobile);
            content = content.Replace("{{Email}}", email);
            content = content.Replace("{{Address}}", address);

            string listProductEmail = ListProductEmail(listProduct);
            content = content.Replace("{{Product}}", listProductEmail);
            content = content.Replace("{{Total}}", totalAll.ToString());
            //content = content.Replace("{{Product}}", listProduct[0]);
            //gửi đến Admin
            var toEmailAdmin = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
            new MailHelper().SendMail(toEmailAdmin, "Đơn hàng mới từ BookShop", content);
            //gửi đến người mua
            var toEmailCustomer = email;
            new MailHelper().SendMail(toEmailCustomer, "Đơn hàng mới từ BookShop", content);
            ///
            return Redirect("/hoan-thanh");
        }

        public ActionResult OrderSuccess()
        {
            Session.Clear();
            Session.RemoveAll();
            return View();
        }
    }
}