using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ThamKhamLamSang
    {
        [Key]
        public int MaThamKham { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBacSi { get; set; }

        [Required]
        public DateTime NgayThamKham { get; set; }

        [StringLength(1000)]
        public string? TrieuChung { get; set; }

        [StringLength(1000)]
        public string? KetQuaThamKham { get; set; }

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaHoSo")]
        public virtual HoSoBenhAn HoSo { get; set; }

        [ForeignKey("MaBacSi")]
        public virtual BacSi BacSi { get; set; }

        // SỬA TÊN Navigation property
        public virtual ICollection<KetQuaLamSang> KetQuas { get; set; } = new List<KetQuaLamSang>();
    }
}
