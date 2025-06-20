using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ThamKhamLamSangController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThamKhamLamSangController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ThamKhamLamSang/Create?maBenhNhan=123
        public async Task<IActionResult> Create(int maBenhNhan)
        {
            // Tìm hồ sơ bệnh án mới nhất của bệnh nhân (nếu có)
            var hoSo = await _context.HoSoBenhAn
                .Where(h => h.MaBenhNhan == maBenhNhan)
                .OrderByDescending(h => h.NgayNhapVien)
                .FirstOrDefaultAsync();

            if (hoSo == null)
            {
                TempData["ErrorMessage"] = "Bệnh nhân chưa có hồ sơ bệnh án.";
                return RedirectToAction("ChonBenhNhan", "KetQuaLamSang");
            }

            // Lấy danh sách bác sĩ để chọn (nếu cần)
            var bacSiList = await _context.BacSi
                .Select(bs => new { bs.MaBacSi, bs.HoTen })
                .ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList, "MaBacSi", "HoTen");

            // Truyền thông tin bệnh nhân và hồ sơ sang ViewBag nếu muốn hiển thị
            var benhNhan = await _context.BenhNhan.FindAsync(maBenhNhan);
            ViewBag.BenhNhan = benhNhan;
            ViewBag.HoSo = hoSo;

            // Tạo model mặc định
            var model = new ThamKhamLamSang
            {
                MaHoSo = hoSo.MaHoSo,
                NgayThamKham = DateTime.Now
            };
            return View(model);
        }

        // POST: ThamKhamLamSang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThamKhamLamSang model)
        {
            // Lấy lại danh sách bác sĩ nếu có lỗi
            var bacSiList = await _context.BacSi
                .Select(bs => new { bs.MaBacSi, bs.HoTen })
                .ToListAsync();
            ViewBag.BacSiList = new SelectList(bacSiList, "MaBacSi", "HoTen");

            // Lấy lại thông tin bệnh nhân và hồ sơ nếu có lỗi
            var hoSo = await _context.HoSoBenhAn.FindAsync(model.MaHoSo);
            if (hoSo != null)
            {
                ViewBag.HoSo = hoSo;
                ViewBag.BenhNhan = await _context.BenhNhan.FindAsync(hoSo.MaBenhNhan);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.NgayTao = DateTime.Now;
            _context.ThamKhamLamSang.Add(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm thăm khám lâm sàng thành công!";
            return RedirectToAction("ChonBenhNhan", "KetQuaLamSang");
        }
    }
}
