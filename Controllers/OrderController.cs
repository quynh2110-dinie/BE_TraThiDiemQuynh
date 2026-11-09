using BE_TraThiDiemQuynh.Models;
using BE_TraThiDiemQuynh.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_TraThiDiemQuynh.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        //GET: Order/Checkout
        public ActionResult Checkout()
        {
            //kiểm tra giỏ hàng trong sesion
            //nếu giỏ hàng trống thì chuyển hướng về trang chủ
            var cart = Session["Cart"] as List<CartItem>;
            if(cart == null || cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            //xác thực người dùng đã đăng nhập chưa, nếu chưa thì chuyển hướng tới trang đăng nhập
            var user=db.User.SingleOrDefault(u => u.Username == User.Identity.Name);
            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            //lấy thông tin khách hàng từ CSDL, nếu chưa có thì chuyển hướng tới trang đăng nhập
            //nếu có rồi thì lấy địa chỉ khách hàng và gán vào DeliveryAddress của CheckoutVM
            var customer = db.Customer.SingleOrDefault(c => c.Username == user.Username);
            if(customer == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new CheckoutVM
            {
                CartItems = cart,
                TotalAmount=cart.Sum(item => item.TotalPrice), //tổng giá trị của các mặt hàng giỏ
                OrderDate=DateTime.Now,//mặc định lấy bằng thời điểm đặt hàng
                AddressDelivery=customer.CustomerAddress,//lấy đại chỉ mặc định của bảng Customer
                CustomerID=customer.CustomerID,//lấy mã khách hàng từ bảng Customer
                UserName=customer.Username //lấy tên đăng nhập từ bảng Customer
            };
            return View(model);
        }

        //POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                //lấy thông tin giỏ hàng từ session
                var cart = Session["Cart"] as List<CartItem>;

                //nếu giỏ hàng rỗng sẽ điều hướng tới trang Home
                if(cart == null || !cart.Any())
                {
                    return RedirectToAction("Index", "Home");
                }
                //nếu người dùng chưa đăng nhập sẽ điều hướng tới trang Login
                var user = db.User.SingleOrDefault(u => u.Username == User.Identity.Name);
                if(user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                //nếu khách hàng không khớp với tên đăng nhập, sẽ điều hướng tới trang Login
                var customer = db.Customer.SingleOrDefault(c => c.Username == user.Username);
                if(customer == null || customer.CustomerID != model.CustomerID)
                {
                    return RedirectToAction("Login", "Account");
                }
                //nếu người dùng chọn thanh toán bằng Paypal, sẽ điều hướng tới trang PaymentWWithPaypal
                if(model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Payment", model);
                }

                //thiết lập PaymentStatus dựa trên PaymentMethod
                string paymentStatus = "Chưa thanh toán";
                switch (model.PaymentMethod)
                {
                    case "Tiền mặt":
                        paymentStatus = "Thanh toán tiền mặt";
                        break;
                    case "Paypal":
                        paymentStatus = "Thanh toán qua Paypal";
                        break;
                    case "Mua trước trả sau":
                        paymentStatus = "Mua trước trả sau";
                        break;
                    default:
                        paymentStatus = "Chưa thanh toán";
                        break;
                }

                //tạo đơn hàng và chi tiết đơn hàng liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = model.DeliveryMethod,
                    AddressDelivery = model.AddressDelivery,
                    OrderDetail = cart.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice =item.TotalPrice
                    }).ToList()
                };
                db.Order.Add(order);
                db.SaveChanges();
                
                //xóa giỏ hàng khỏi session sau khi đặt hàng thành công
                Session["Cart"] = null;
                return RedirectToAction("OrderSuccess", new {id=order.OrderID});
            }
            return View(model);
        }
        public ActionResult OrderSuccess(int id)
        {
            var order = db.Order.Include("OrderDetails").SingleOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }
}