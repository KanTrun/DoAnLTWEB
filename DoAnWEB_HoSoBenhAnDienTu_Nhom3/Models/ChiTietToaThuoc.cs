using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ChiTietToaThuoc
    {
        [Required]
        public int MaToaThuoc { get; set; }

        [Required]
        public int MaThuoc { get; set; }

        [Required]
        [StringLength(100)]
        public string LieuLuong { get; set; }

        [Required]
        [StringLength(100)]
        public string SoLanUong { get; set; }

        public int? SoLuong { get; set; }

        // Navigation properties
        [ForeignKey("MaToaThuoc")]
        public virtual ToaThuoc ToaThuoc { get; set; }

        [ForeignKey("MaThuoc")]
        public virtual Thuoc Thuoc { get; set; }
    }
}
