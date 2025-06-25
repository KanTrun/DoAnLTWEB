using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ChiTietToaThuoc
    {
        // THÊM: Primary Key bắt buộc cho EF Core
        [Key]
        public int MaChiTiet { get; set; }

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

        // SỬA: Bỏ [Required] cho Navigation properties
        [ForeignKey("MaToaThuoc")]
        [ValidateNever]
        public virtual ToaThuoc? ToaThuoc { get; set; }

        [ForeignKey("MaThuoc")]
        [ValidateNever]
        public virtual Thuoc? Thuoc { get; set; }
    }
}
