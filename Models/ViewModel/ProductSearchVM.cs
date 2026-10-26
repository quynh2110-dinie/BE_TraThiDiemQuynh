using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace BE_TraThiDiemQuynh.Models.ViewModel
{
    public class ProductSearchVM
    {
        //tiêu chí để search theo tên, mô tả sp
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }
        //các tiêu chí để search theo giá sản phẩm
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        //thứ tự sắp xếp
        public string SortOrder { get; set; }

        //các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //trang hiên tại
        public int PageSize { get; set; } //số lượng item trên 1 trang

        //danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }

        //danh sách sản phẩm thỏa điều kiện tìm kiếm
        //public List<Product> Products { get; set; }
    }
}