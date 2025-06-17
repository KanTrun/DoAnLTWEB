using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class BacSi
    {
        [Key]
        public int MaBacSi { get; set; }

        [Required]
        public int MaTaiKhoan { get; set; } // Đảm bảo là int

        [Required]
        public int MaKhoa { get; set; } // Đảm bảo là int

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [StringLength(50)]
        public string? ChuyenKhoa { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        // Navigation properties
        [ForeignKey("MaTaiKhoan")]
        public virtual TaiKhoanNguoiDung TaiKhoan { get; set; }

        [ForeignKey("MaKhoa")]
        public virtual Khoa Khoa { get; set; }
    }
}
