using System.ComponentModel.DataAnnotations;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ và tên")]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Display(Name = "Giới tính")]
        public string? GioiTinh { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(255)]
        public string? DiaChi { get; set; }

        [Display(Name = "Số điện thoại")]
        [StringLength(20)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? SoDienThoai { get; set; }

        [Display(Name = "Số bảo hiểm")]
        [StringLength(50)]
        public string? SoBaoHiem { get; set; }

        [Display(Name = "Nhóm máu")]
        [StringLength(5)]
        public string? NhomMau { get; set; }

        [Display(Name = "Dị ứng")]
        [StringLength(255)]
        public string? DiUng { get; set; }

        public bool IsExisting { get; set; } = false;
    }
}
