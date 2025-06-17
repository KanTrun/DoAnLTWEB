using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class DonDieuTri
    {
        [Key]
        public int MaDonDieuTri { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDonDieuTri { get; set; }

        [Required]
        public DateTime NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        [Required]
        public int MaBacSi { get; set; }

        // Navigation properties
        [ForeignKey("MaHoSo")]
        public virtual HoSoBenhAn HoSo { get; set; }

        [ForeignKey("MaBacSi")]
        public virtual BacSi BacSi { get; set; }
    }
}
