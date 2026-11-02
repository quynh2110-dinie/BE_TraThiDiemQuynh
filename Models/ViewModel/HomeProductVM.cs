using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BE_TraThiDiemQuynh.Models.ViewModel
{
    public class HomeProductVM
    {
        //tiêu chí để search theo tên, mô tả sản phẩm
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } //trang hiên tại
        public int PageSize { get; set; } //số lượng item trên 1 trang

        //danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        //danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }

    }
}