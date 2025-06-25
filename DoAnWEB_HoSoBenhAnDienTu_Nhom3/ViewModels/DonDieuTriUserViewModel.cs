using X.PagedList;


namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class DonDieuTriUserViewModel
    {
        public int MaDonDieuTri { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string TenBenh { get; set; }
        public string LoaiChanDoan { get; set; }
        public string TenDonDieuTri { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public string BacSi { get; set; }
    }

}
