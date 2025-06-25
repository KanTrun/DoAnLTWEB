namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class BenhNhanDonDieuTriViewModel
    {
        public int MaBenhNhan { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgayKhamGanNhat { get; set; }
        public string TenBenh { get; set; }
        public string LoaiChanDoan { get; set; }
        public int? MaHoSoGanNhat { get; set; }
        public bool DaCoBenh { get; set; }
        public bool DaCoChanDoan { get; set; }
        public bool DaCoDonDieuTri { get; set; }
     
        public int? MaBenhGanNhat { get; set; }
    }

}
