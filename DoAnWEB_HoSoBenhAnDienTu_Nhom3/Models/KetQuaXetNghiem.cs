using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class KetQuaXetNghiem
    {
        [Key]
        public int MaKetQuaXN { get; set; }

        [Required]
        public int MaChiDinh { get; set; }

        public string? GiaTriKetQua { get; set; }

        [StringLength(100)]
        public string? KhoangThamChieu { get; set; }

        [Required]
        public DateTime NgayTraKetQua { get; set; }

        // Navigation properties
        [ForeignKey("MaChiDinh")]
        public virtual ChiDinhXetNghiem ChiDinh { get; set; }
    }
}
