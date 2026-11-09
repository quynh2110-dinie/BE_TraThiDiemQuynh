using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BE_TraThiDiemQuynh.Models.ViewModel
{
    public class RegisterVM /*Luuw thông tin form đăng ký tài khoản khách hàng*/
    {
        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Họ và tên")]   
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerNumber { get; set; }

        [Required]
        [Display(Name = "Địa chỉ")]
        public string CustomerAddress { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }
    }
}