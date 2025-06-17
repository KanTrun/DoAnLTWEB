using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class Thuoc
    {
        [Key]
        public int MaThuoc { get; set; }

        [Required]
        [StringLength(200)]
        public string TenThuoc { get; set; }

        [StringLength(100)]
        public string? HoatChat { get; set; }

        [StringLength(50)]
        public string? DonVi { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Gia { get; set; }

        [StringLength(1000)]
        public string? CongDung { get; set; }

        [StringLength(1000)]
        public string? CachDung { get; set; }

        public int? SoLuongTon { get; set; }

        public DateTime? HanSuDung { get; set; }

        // SỬA TÊN Navigation property
        public virtual ICollection<ChiTietToaThuoc> ChiTietToaThuocs { get; set; } = new List<ChiTietToaThuoc>();
    }
}
