using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BE_TraThiDiemQuynh.Models.ViewModel
{
    public class CheckoutVM
    { public List<CartItem> CartItems { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name = "Tổng số tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái thanh toán")]   
        public string PaymentStatus { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Phương thức giao hàng")]
        public string DeliveryMethod { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string AddressDelivery { get; set; }

        public string UserName { get; set; }

        //các thuộc tính khác của đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}