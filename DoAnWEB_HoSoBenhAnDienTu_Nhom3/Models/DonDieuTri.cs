using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class DonDieuTri
    {
        [Key]
        public int MaDonDieuTri { get; set; }

        [Required(ErrorMessage = "Mã hồ sơ là bắt buộc")]
        public int MaHoSo { get; set; }

        [Required(ErrorMessage = "Tên đơn điều trị là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên đơn điều trị không được quá 100 ký tự")]
        public string TenDonDieuTri { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        [Required(ErrorMessage = "Mã bác sĩ là bắt buộc")]
        public int MaBacSi { get; set; }

        // SỬA: Bỏ [Required] cho Navigation properties và thêm [ValidateNever]
        [ForeignKey("MaHoSo")]
        [ValidateNever]
        public virtual HoSoBenhAn? HoSo { get; set; }

        [ForeignKey("MaBacSi")]
        [ValidateNever]
        public virtual BacSi? BacSi { get; set; }
    }
}
