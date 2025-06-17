using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ToaThuoc
    {
        [Key]
        public int MaToaThuoc { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBacSi { get; set; }

        [Required]
        public DateTime NgayKeDon { get; set; }

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "Chờ cấp phát";

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaHoSo")]
        public virtual HoSoBenhAn HoSo { get; set; }

        [ForeignKey("MaBacSi")]
        public virtual BacSi BacSi { get; set; }

        // SỬA TÊN Navigation property
        public virtual ICollection<ChiTietToaThuoc> ChiTietToaThuocs { get; set; } = new List<ChiTietToaThuoc>();
    }
}
