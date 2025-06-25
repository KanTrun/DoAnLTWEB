using X.PagedList;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class UserToaThuocViewModel
    {
        public BenhNhan BenhNhan { get; set; }
        public IPagedList<ToaThuoc> ToaThuocs { get; set; } // Đổi thành IPagedList
        public List<ChiTietToaThuoc> ChiTietToaThuocs { get; set; } = new List<ChiTietToaThuoc>();
    }
}
