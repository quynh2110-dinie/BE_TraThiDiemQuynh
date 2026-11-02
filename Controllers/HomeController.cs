using BE_TraThiDiemQuynh.Models;
using BE_TraThiDiemQuynh.Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_TraThiDiemQuynh.Controllers
{
    public class HomeController : Controller
    {
        private MyStoreEntities db=new MyStoreEntities();

        //get: admin/product
        public ActionResult Index(string searchTerm, int? page)
        {
            var model = new HomeProductVM();
            var products=db.Product.AsQueryable();

            //tìm kiếm sản phẩm dựa trên từ khóa
            if(!string.IsNullOrEmpty(searchTerm) )
            {
                model.SearchTerm = searchTerm;
                products=products.Where(p=>p.ProductName.Contains(searchTerm)||
                                        p.ProductDecription.Contains(searchTerm)||
                                        p.Category.CategoryName.Contains(searchTerm));
            }

            //đoạn code liên quan đến phân trang
            //lấy số trang hiện tại(mặc định là 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = 6;//số sản phẩm mỗi trang

            //lấy top 10 sản phẩm bán chạy nhất
            model.FeaturedProducts=products.OrderByDescending(p=>p.OrderDetail.Count()).Take(10).ToList();

            //lấy 20 sản phẩm bán ế nhất và phân trang
            model.NewProducts = products.OrderBy(p => p.OrderDetail.Count()).Take(20).ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public ActionResult ProductDetail(int? id, int?quanlity, int?page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var product = db.Product.Find(id);
            if(product==null)
            {
                return HttpNotFound();
            }
            var products=db.Product.Where(p=>p.CategoryID==product.CategoryID && p.ProductID!=product.ProductID).Take(8).ToList();
            ProductDetailVM model = new ProductDetailVM();
            model.product=product;
            model.RelatedProducts=db.Product.Where(p=>p.CategoryID==product.CategoryID && p.ProductID!=product.ProductID).Take(8).ToList();
            model.TopProducts=db.Product.Where(p=>p.CategoryID==product.CategoryID).OrderByDescending(p=>p.OrderDetail.Count()).Take(8).ToPagedList(1,8);
            if(quanlity.HasValue)
            {
                model.Quantity=quanlity.Value;
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}