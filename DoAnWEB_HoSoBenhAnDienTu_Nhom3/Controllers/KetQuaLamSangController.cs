using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class KetQuaLamSangController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public KetQuaLamSangController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ĐIỂM VÀO CHUNG CHO CẢ ADMIN VÀ USER
        public async Task<IActionResult> Index(int? page)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("ChonBenhNhan");
            }
            else if (User.IsInRole("User"))
            {
                var user = await _userManager.GetUserAsync(User);
                var taiKhoan = await _context.TaiKhoanNguoiDung.FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);
                if (taiKhoan == null) return NotFound();
                var benhNhan = await _context.BenhNhan.FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);
                if (benhNhan == null) return NotFound();

                // Phân trang thủ công
                var pageNumber = page ?? 1;
                var pageSize = 1;
                var ketQuaQuery = _context.KetQuaLamSang
                    .Where(k => k.MaBenhNhan == benhNhan.MaBenhNhan)
                    .OrderByDescending(k => k.NgayKham);

                var totalItemCount = await ketQuaQuery.CountAsync();
                var items = await ketQuaQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedList = new StaticPagedList<KetQuaLamSang>(
                    items, pageNumber, pageSize, totalItemCount
                );

                return View("IndexUser", pagedList);
            }
            else
            {
                return Forbid();
            }
        }

        // ADMIN: Xem danh sách bệnh nhân để nhập kết quả lâm sàng
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChonBenhNhan()
        {
            var dsBenhNhan = await _context.BenhNhan
        .Select(bn => new DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels.BenhNhanSelectViewModel
        {
            MaBenhNhan = bn.MaBenhNhan,
            HoTen = bn.HoTen,
            NgaySinh = bn.NgaySinh,
            GioiTinh = bn.GioiTinh,
            SoDienThoai = bn.SoDienThoai,
            NgayKhamGanNhat = _context.KetQuaLamSang
                .Where(k => k.MaBenhNhan == bn.MaBenhNhan)
                .OrderByDescending(k => k.NgayKham)
                .Select(k => (DateTime?)k.NgayKham)
                .FirstOrDefault()
        })
        .ToListAsync();

            return View(dsBenhNhan);
        }

        // ADMIN: Nhập kết quả lâm sàng cho bệnh nhân
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(int maBenhNhan)
        {
            var benhNhan = await _context.BenhNhan.FindAsync(maBenhNhan);
            if (benhNhan == null) return NotFound();
            ViewBag.BenhNhan = benhNhan;

            // Lấy danh sách bác sĩ
            var bacSiList = await _context.BacSi
                .Select(bs => bs.HoTen)
                .ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList);

            return View(new KetQuaLamSang { MaBenhNhan = maBenhNhan, NgayKham = DateTime.Now });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KetQuaLamSang model)
        {
            ModelState.Remove("BenhNhan");
            // Lấy lại danh sách bác sĩ nếu có lỗi
            var bacSiList = await _context.BacSi
                .Select(bs => bs.HoTen)
                .ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList);

            if (!ModelState.IsValid)
            {
                ViewBag.BenhNhan = await _context.BenhNhan.FindAsync(model.MaBenhNhan);
                return View(model);
            }
            model.NgayTao = DateTime.Now;
            _context.KetQuaLamSang.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("ChonBenhNhan");
        }
    }
}
