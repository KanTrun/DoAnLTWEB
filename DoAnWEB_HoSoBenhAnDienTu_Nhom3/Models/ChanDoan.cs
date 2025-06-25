using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ChanDoan
    {
        [Key]
        public int MaChanDoan { get; set; }

        [Required]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBenh { get; set; }

        [Required]
        public int MaBacSi { get; set; }

        [Required]
        public DateTime NgayChanDoan { get; set; }

        [StringLength(20)]
        public string LoaiChanDoan { get; set; } = "Sơ bộ";

        [StringLength(1000)]
        public string? GhiChu { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties - thêm ValidateNever để bỏ qua validation
        [ForeignKey("MaHoSo")]
        [ValidateNever]
        public virtual HoSoBenhAn? HoSo { get; set; }

        [ForeignKey("MaBenh")]
        [ValidateNever]
        public virtual Benh? Benh { get; set; }

        [ForeignKey("MaBacSi")]
        [ValidateNever]
        public virtual BacSi? BacSi { get; set; }
    }
}
