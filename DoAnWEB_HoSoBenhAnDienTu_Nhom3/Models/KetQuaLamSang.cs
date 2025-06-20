using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class KetQuaLamSang
    {
        [Key]
        public int MaKetQua { get; set; }

        [Required]
        public int MaBenhNhan { get; set; }

        [Required]
        public DateTime NgayKham { get; set; }

        [Required]
        [StringLength(100)]
        public string BacSiKham { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? NhietDo { get; set; }

        [StringLength(20)]
        public string? HuyetAp { get; set; }

        public int? NhipTim { get; set; }

        public int? NhipTho { get; set; }

        [StringLength(500)]
        public string? ChanDoan { get; set; }

        [StringLength(1000)]
        public string? KetQua { get; set; }

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaBenhNhan")]
        public virtual BenhNhan? BenhNhan { get; set; }
    }
}
