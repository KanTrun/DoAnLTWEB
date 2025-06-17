using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ChiDinhXetNghiem
    {
        [Key]
        public int MaChiDinh { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBacSi { get; set; }

        [Required]
        public DateTime NgayChiDinh { get; set; }

        [Required]
        [StringLength(200)]
        public string TenXetNghiem { get; set; }

        [StringLength(1000)]
        public string? MoTa { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "Chờ thực hiện";

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("MaHoSo")]
        public virtual HoSoBenhAn HoSo { get; set; }

        [ForeignKey("MaBacSi")]
        public virtual BacSi BacSi { get; set; }

        // SỬA TÊN Navigation property
        public virtual ICollection<KetQuaXetNghiem> KetQuas { get; set; } = new List<KetQuaXetNghiem>();
    }
}
