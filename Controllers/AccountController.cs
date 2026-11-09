using BE_TraThiDiemQuynh.Models;
using BE_TraThiDiemQuynh.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace BE_TraThiDiemQuynh.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        //post: account/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model) {
            if (ModelState.IsValid)
            {
                //kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUser=db.User.SingleOrDefault(u => u.Username == model.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại");
                    return View(model);
                }
                //nếu chưa tồn tại thì tạo bản ghi thông tin tài khoản trong bảng User
                var user = new User
                {
                    Username = model.UserName,
                    Password = model.Password, //lưu ý: nên mã hóa mật khẩu trước khi lưu
                    UserRole = "Customer"
                };
                db.User.Add(user);
                //và tạo bản ghi thông tin khách hàng trong bảng Customer
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerPhone = model.CustomerNumber,
                    CustomerAddress = model.CustomerAddress,
                    CustomerEmail = model.CustomerEmail,
                    Username = model.UserName,
                };
                db.Customer.Add(customer);
                //lưu thông tin tài khoản và thông tin khách hàng vào CSDL
                db.SaveChanges(); 
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        //GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }
        //POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                //kiểm tra thông tin đăng nhập
                var user = db.User.SingleOrDefault(u => u.Username == model.UserName && u.Password == model.Password && u.UserRole == "Customer");
                if (user != null)
                {
                    //lưu thông tin đăng nhập vào session
                    Session["UserName"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    //lưu thông tin xác thực người dùng vào cookie
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }
    }
}