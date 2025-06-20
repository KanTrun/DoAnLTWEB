using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;
using System.Linq;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            var account = _context.TaiKhoanNguoiDung
                .Include(t => t.BacSi)
                    .ThenInclude(b => b.Khoa)
                .FirstOrDefault(t => t.TenDangNhap == username);

            ViewBag.VaiTro = account?.VaiTro;

            if (account?.VaiTro == "BacSi" || account?.VaiTro == "Admin")
            {
                ViewBag.BacSi = account.BacSi;

                // Thống kê số lượng bệnh nhân và bác sĩ
                var soLuongBenhNhan = _context.BenhNhan.Count();
                var soLuongBacSi = _context.BacSi.Count();
                ViewBag.SoLuongBenhNhan = soLuongBenhNhan;
                ViewBag.SoLuongBacSi = soLuongBacSi;

                // --- Biểu đồ cột: kiểm tra lâm sàng & xét nghiệm ---
                // Số bệnh nhân đã kiểm tra lâm sàng (có bác sĩ khám)
                var maBenhNhanKiemTraLamSang = _context.KetQuaLamSang
                    .Where(k => !string.IsNullOrEmpty(k.BacSiKham))
                    .Select(k => k.MaBenhNhan)
                    .Distinct()
                    .ToList();

                // Số bệnh nhân đã ĐÁNH GIÁ KẾT QUẢ XÉT NGHIỆM (đã có KetQuaXetNghiem)
                var maBenhNhanKiemTraXetNghiem = _context.KetQuaXetNghiem
                    .Include(kq => kq.ChiDinh)
                        .ThenInclude(cd => cd.HoSo)
                    .Where(kq => kq.ChiDinh != null && kq.ChiDinh.HoSo != null)
                    .Select(kq => kq.ChiDinh.HoSo.MaBenhNhan)
                    .Distinct()
                    .ToList();

                var soUserKiemTraLamSang = _context.BenhNhan
                    .Count(b => maBenhNhanKiemTraLamSang.Contains(b.MaBenhNhan));
                var soUserKiemTraXetNghiem = _context.BenhNhan
                    .Count(b => maBenhNhanKiemTraXetNghiem.Contains(b.MaBenhNhan));

                ViewBag.SoUserKiemTraLamSang = soUserKiemTraLamSang;
                ViewBag.SoUserKiemTraXetNghiem = soUserKiemTraXetNghiem;
            }
            else if (account?.VaiTro == "BenhNhan" || account?.VaiTro == "User" || account?.VaiTro == "Bệnh nhân")
            {
                // Lấy thông tin bệnh nhân
                var benhNhan = _context.BenhNhan.FirstOrDefault(b => b.MaTaiKhoan == account.MaTaiKhoan);
                ViewBag.BenhNhan = benhNhan;

                // Lấy danh sách hồ sơ bệnh án của bệnh nhân này
                var hoSoList = _context.HoSoBenhAn
                    .Where(h => h.MaBenhNhan == benhNhan.MaBenhNhan)
                    .OrderByDescending(h => h.NgayNhapVien)
                    .ToList();
                ViewBag.HoSoBenhAnList = hoSoList;

                // Lấy danh sách thăm khám lâm sàng của bệnh nhân này (bao gồm tên bác sĩ)
                var hoSoIds = hoSoList.Select(hs => hs.MaHoSo).ToList();
                var danhSachThamKham = _context.ThamKhamLamSang
                    .Where(tk => hoSoIds.Contains(tk.MaHoSo))
                    .Include(tk => tk.BacSi)
                    .OrderByDescending(tk => tk.NgayThamKham)
                    .ToList();
                ViewBag.DanhSachThamKham = danhSachThamKham;
            }
        }
        return View();
    }
}
