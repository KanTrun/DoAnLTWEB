using System.ComponentModel.DataAnnotations;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class Benh
    {
        [Key]
        public int MaBenh { get; set; } // Đảm bảo là int

        [Required]
        [StringLength(10)]
        public string MaICD { get; set; } // Mã ICD là string

        [Required]
        [StringLength(200)]
        public string TenBenh { get; set; }

        [StringLength(1000)]
        public string? MoTa { get; set; }

        [StringLength(50)]
        public string? NhomBenh { get; set; }

        // Navigation property
        public virtual ICollection<ChanDoan> ChanDoans { get; set; } = new List<ChanDoan>();
    }
}
