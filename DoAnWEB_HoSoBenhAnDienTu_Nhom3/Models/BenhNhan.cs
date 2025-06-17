using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class BenhNhan
    {
        [Key]
        public int MaBenhNhan { get; set; }

        [Required]
        public int MaTaiKhoan { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required]
        public DateTime NgaySinh { get; set; }

        [StringLength(10)]
        public string? GioiTinh { get; set; }

        [StringLength(255)]
        public string? DiaChi { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(50)]
        public string? SoBaoHiem { get; set; }

        [StringLength(5)]
        public string? NhomMau { get; set; }

        [StringLength(255)]
        public string? DiUng { get; set; }

        // Computed property
        [NotMapped]
        public int Tuoi => DateTime.Now.Year - NgaySinh.Year;

        // Navigation properties
        [ForeignKey("MaTaiKhoan")]
        public virtual TaiKhoanNguoiDung TaiKhoan { get; set; }

        public virtual ICollection<HoSoBenhAn> HoSoBenhAn { get; set; } = new List<HoSoBenhAn>();

        // Navigation property cho KetQuaLamSang
        public virtual ICollection<KetQuaLamSang> KetQuaLamSang { get; set; } = new List<KetQuaLamSang>();
    }
}
