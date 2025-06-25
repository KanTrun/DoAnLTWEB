using System.ComponentModel.DataAnnotations;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class KeThuocViewModel
    {
        // Thuốc mới
        [ValidateNever]
        public Thuoc ThuocMoi { get; set; } = new Thuoc();

        // Chi tiết toa thuốc  
        [ValidateNever]
        public ChiTietToaThuoc ChiTietToaThuoc { get; set; } = new ChiTietToaThuoc();

        // Danh sách thuốc có sẵn
        [ValidateNever]
        public List<Thuoc> DanhSachThuocCoSan { get; set; } = new List<Thuoc>();

        // Chọn loại thuốc
        public bool SuDungThuocMoi { get; set; } = false;

        // Mã thuốc có sẵn (chỉ validate khi không dùng thuốc mới)
        public int MaThuocCoSan { get; set; }

        // Các trường riêng cho validation (thay vì dùng nested object)
        [Required(ErrorMessage = "Liều lượng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Liều lượng không được quá 100 ký tự")]
        public string LieuLuong { get; set; } = "";

        [Required(ErrorMessage = "Số lần uống là bắt buộc")]
        [StringLength(100, ErrorMessage = "Số lần uống không được quá 100 ký tự")]
        public string SoLanUong { get; set; } = "";

        public int? SoLuong { get; set; }

        // Thuốc mới - các trường riêng
        [StringLength(200, ErrorMessage = "Tên thuốc không được quá 200 ký tự")]
        public string TenThuocMoi { get; set; } = "";

        [StringLength(100)]
        public string? HoatChatMoi { get; set; }

        [StringLength(50)]
        public string? DonViMoi { get; set; }

        public decimal? GiaMoi { get; set; }

        [StringLength(1000)]
        public string? CongDungMoi { get; set; }

        [StringLength(1000)]
        public string? CachDungMoi { get; set; }

        public int? SoLuongTonMoi { get; set; }

        public DateTime? HanSuDungMoi { get; set; }
    }
}
