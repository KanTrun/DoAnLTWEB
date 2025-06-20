using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class KetQuaXetNghiemChonBenhNhanViewModel
    {
        public int MaBenhNhan { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgayKhamGanNhat { get; set; }
        public bool DaDanhGiaLamSang { get; set; }
        public List<ChiDinhXetNghiemViewModel> DanhSachChiDinh { get; set; } // Thêm dòng này
    }

}
