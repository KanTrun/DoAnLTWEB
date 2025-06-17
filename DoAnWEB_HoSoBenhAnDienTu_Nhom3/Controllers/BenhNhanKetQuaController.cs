using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize(Roles = "User")]
    public class BenhNhanKetQuaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BenhNhanKetQuaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BenhNhanKetQua
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin bệnh nhân hiện tại từ User đã đăng nhập
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Tìm tài khoản người dùng
            var taiKhoan = await _context.TaiKhoanNguoiDung
                .FirstOrDefaultAsync(t => t.TenDangNhap == userEmail);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy thông tin tài khoản.");
            }

            // Tìm thông tin bệnh nhân
            var benhNhan = await _context.BenhNhan
                .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

            if (benhNhan == null)
            {
                return NotFound("Không tìm thấy thông tin bệnh nhân.");
            }

            // Lấy tất cả kết quả lâm sàng của bệnh nhân này
            var ketQuaLamSang = await _context.KetQuaLamSang
                .Where(k => k.MaBenhNhan == benhNhan.MaBenhNhan)
                .OrderByDescending(k => k.NgayKham)
                .ToListAsync();

            ViewBag.TenBenhNhan = benhNhan.HoTen;
            return View(ketQuaLamSang);
        }

        // GET: BenhNhanKetQua/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kiểm tra quyền truy cập - chỉ cho phép xem kết quả của chính mình
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var taiKhoan = await _context.TaiKhoanNguoiDung
                .FirstOrDefaultAsync(t => t.TenDangNhap == userEmail);

            if (taiKhoan == null)
            {
                return NotFound();
            }

            var benhNhan = await _context.BenhNhan
                .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

            if (benhNhan == null)
            {
                return NotFound();
            }

            var ketQuaLamSang = await _context.KetQuaLamSang
                .Include(k => k.BenhNhan)
                .FirstOrDefaultAsync(m => m.MaKetQua == id && m.MaBenhNhan == benhNhan.MaBenhNhan);

            if (ketQuaLamSang == null)
            {
                return NotFound();
            }

            return View(ketQuaLamSang);
        }
    }
}
