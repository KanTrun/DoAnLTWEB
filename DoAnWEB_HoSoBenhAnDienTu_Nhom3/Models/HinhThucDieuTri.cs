using System.ComponentModel.DataAnnotations;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class HinhThucDieuTri
    {
        [Key]
        public int MaHinhThuc { get; set; }

        [Required]
        [StringLength(50)]
        public string TenHinhThuc { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        // Navigation property - SỬA TÊN
        public virtual ICollection<HoSoBenhAn> HoSoBenhAns { get; set; } = new List<HoSoBenhAn>();
    }
}
