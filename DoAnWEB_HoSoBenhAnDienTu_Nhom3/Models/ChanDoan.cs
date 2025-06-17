using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ChanDoan
    {
        [Key]
        public int MaChanDoan { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBenh { get; set; } // SỬA: Đổi từ string sang int

        [Required]
        public int MaBacSi { get; set; }

        [Required]
        public DateTime NgayChanDoan { get; set; }

        [StringLength(20)]
        public string LoaiChanDoan { get; set; } = "Sơ bộ"; // Sơ bộ, Xác định, Phân biệt

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaHoSo")]
        public virtual HoSoBenhAn HoSo { get; set; }

        [ForeignKey("MaBenh")]
        public virtual Benh Benh { get; set; }

        [ForeignKey("MaBacSi")]
        public virtual BacSi BacSi { get; set; }
    }
}
