using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class HoSoBenhAn
    {
        [Key]
        public int MaHoSo { get; set; }

        [Required]
        public int MaBenhNhan { get; set; }

        [Required]
        public int MaHinhThuc { get; set; }

        [Required]
        public DateTime NgayNhapVien { get; set; }

        public DateTime? NgayXuatVien { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "Đang điều trị";

        [StringLength(1000)]
        public string? LyDoNhapVien { get; set; }

        [StringLength(1000)]
        public string? TomTatBenhAn { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // Navigation properties (nullable, không [Required])
        // Navigation properties trong HoSoBenhAn
        [ForeignKey("MaBenhNhan")]
        [ValidateNever]
        public virtual BenhNhan? BenhNhan { get; set; }

        [ForeignKey("MaHinhThuc")]
        [ValidateNever]
        public virtual HinhThucDieuTri? HinhThucDieuTri { get; set; }

        [ValidateNever]
        public virtual ICollection<ThamKhamLamSang> ThamKhams { get; set; } = new List<ThamKhamLamSang>();

        [ValidateNever]
        public virtual ICollection<ChiDinhXetNghiem> ChiDinhs { get; set; } = new List<ChiDinhXetNghiem>();

        [ValidateNever]
        public virtual ICollection<DonDieuTri> DonDieuTris { get; set; } = new List<DonDieuTri>();

    }
}
