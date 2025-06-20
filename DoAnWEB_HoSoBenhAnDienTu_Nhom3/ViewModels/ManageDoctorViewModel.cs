using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class ManageDoctorViewModel
    {
        public bool IsExisting { get; set; }
        public int? MaBacSi { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string HoTen { get; set; }

        [StringLength(50)]
        [Display(Name = "Chuyên khoa")]
        public string? ChuyenKhoa { get; set; }

        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Khoa là bắt buộc")]
        [Display(Name = "Khoa")]
        public int? MaKhoa { get; set; }

        public IEnumerable<SelectListItem>? KhoaList { get; set; }
    }
}
