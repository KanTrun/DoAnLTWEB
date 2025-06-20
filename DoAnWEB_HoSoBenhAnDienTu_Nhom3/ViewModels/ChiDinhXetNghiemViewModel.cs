using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class ChiDinhXetNghiemViewModel
    {
        public int MaChiDinh { get; set; }
        public string TenXetNghiem { get; set; }
        public bool DaDanhGiaKetQua { get; set; } // TRUE nếu đã có kết quả
    }

}
