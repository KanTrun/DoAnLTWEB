using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using X.PagedList;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels
{
    public class DonDieuTriUserTongHopViewModel
    {
        public BenhNhan BenhNhan { get; set; }
        public IPagedList<DonDieuTriUserViewModel> DonDieuTris { get; set; }
    }
}
