using System.ComponentModel.DataAnnotations;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class KetQuaLamSangViewModel
    {
        public int MaKetQua { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bệnh nhân")]
        [Display(Name = "Bệnh nhân")]
        public int MaBenhNhan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày khám")]
        [Display(Name = "Ngày khám")]
        [DataType(DataType.Date)]
        public DateTime NgayKham { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Vui lòng nhập tên bác sĩ")]
        [Display(Name = "Bác sĩ khám")]
        [StringLength(100)]
        public string BacSiKham { get; set; }

        [Display(Name = "Nhiệt độ (°C)")]
        [Range(35, 45, ErrorMessage = "Nhiệt độ phải từ 35-45°C")]
        public decimal? NhietDo { get; set; }

        [Display(Name = "Huyết áp")]
        [StringLength(20)]
        public string? HuyetAp { get; set; }

        [Display(Name = "Nhịp tim (lần/phút)")]
        [Range(40, 200, ErrorMessage = "Nhịp tim phải từ 40-200 lần/phút")]
        public int? NhipTim { get; set; }

        [Display(Name = "Nhịp thở (lần/phút)")]
        [Range(10, 40, ErrorMessage = "Nhịp thở phải từ 10-40 lần/phút")]
        public int? NhipTho { get; set; }

        [Display(Name = "Chẩn đoán")]
        [StringLength(500)]
        public string? ChanDoan { get; set; }

        [Display(Name = "Kết quả khám")]
        [StringLength(1000)]
        public string? KetQua { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(1000)]
        public string? GhiChu { get; set; }

        // Thông tin bệnh nhân để hiển thị
        public string? TenBenhNhan { get; set; }
        public DateTime? NgaySinhBenhNhan { get; set; }
        public string? GioiTinhBenhNhan { get; set; }
    }

    public class ChonBenhNhanViewModel
    {
        public List<BenhNhanSelectViewModel> DanhSachBenhNhan { get; set; } = new List<BenhNhanSelectViewModel>();
        public string? TimKiem { get; set; }
    }

    public class BenhNhanSelectViewModel
    {
        public int MaBenhNhan { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string? GioiTinh { get; set; }
        public string? SoDienThoai { get; set; }
        public int Tuoi => DateTime.Now.Year - NgaySinh.Year;
        public DateTime? NgayKhamGanNhat { get; set; }
    }
}
