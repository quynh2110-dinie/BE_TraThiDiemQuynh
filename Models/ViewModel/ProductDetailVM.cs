using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using PagedList.Mvc;

namespace BE_TraThiDiemQuynh.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product product { get; set; }
        public int Quantity { get; set; } = 1;
        //tính giá trị tàm thời
        public decimal estimatedValue { get; set; } /*=quanlity*product.ProductPrice;*/

        //các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 3;

        //danh sách 8 sản phẩm cùng danh mục
        public List<Product> RelatedProducts { get; set; }

        //danh sách 8 sản phẩm bán chạy nahast cùng danh mục
        public PagedList.IPagedList<Product> TopProducts { get; set; }
    }
}