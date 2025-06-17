using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class QuanLyNguoiDungController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public QuanLyNguoiDungController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: QuanLyNguoiDung/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            // Tìm hoặc tạo TaiKhoanNguoiDung
            var taiKhoan = await _context.TaiKhoanNguoiDung
                .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

            if (taiKhoan == null)
            {
                // Tạo tài khoản người dùng mới
                taiKhoan = new TaiKhoanNguoiDung
                {
                    TenDangNhap = user.UserName,
                    MatKhau = "", // Được quản lý bởi Identity
                    VaiTro = "Bệnh nhân",
                    TrangThai = true,
                    NgayTao = DateTime.Now
                };
                _context.TaiKhoanNguoiDung.Add(taiKhoan);
                await _context.SaveChangesAsync();
            }

            // Tìm thông tin bệnh nhân
            var benhNhan = await _context.BenhNhan
                .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

            var viewModel = new ManageUserViewModel();

            if (benhNhan != null)
            {
                viewModel.HoTen = benhNhan.HoTen;
                viewModel.NgaySinh = benhNhan.NgaySinh;
                viewModel.GioiTinh = benhNhan.GioiTinh;
                viewModel.DiaChi = benhNhan.DiaChi;
                viewModel.SoDienThoai = benhNhan.SoDienThoai;
                viewModel.SoBaoHiem = benhNhan.SoBaoHiem;
                viewModel.NhomMau = benhNhan.NhomMau;
                viewModel.DiUng = benhNhan.DiUng;
                viewModel.IsExisting = true;
            }
            else
            {
                viewModel.NgaySinh = DateTime.Now.AddYears(-25);
            }

            return View(viewModel);
        }

        // POST: QuanLyNguoiDung/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ManageUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate tuổi
            var age = DateTime.Now.Year - model.NgaySinh.Year;
            if (model.NgaySinh.Date > DateTime.Now.AddYears(-age)) age--;

            if (age < 0 || age > 150)
            {
                ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ.");
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Không tìm thấy thông tin người dùng.");
            }

            var taiKhoan = await _context.TaiKhoanNguoiDung
                .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

            if (taiKhoan == null)
            {
                return NotFound("Không tìm thấy tài khoản người dùng.");
            }

            try
            {
                var benhNhan = await _context.BenhNhan
                    .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

                if (benhNhan == null)
                {
                    // Tạo mới bệnh nhân
                    benhNhan = new BenhNhan
                    {
                        MaTaiKhoan = taiKhoan.MaTaiKhoan,
                        HoTen = model.HoTen.Trim(),
                        NgaySinh = model.NgaySinh,
                        GioiTinh = model.GioiTinh,
                        DiaChi = model.DiaChi?.Trim(),
                        SoDienThoai = model.SoDienThoai?.Trim(),
                        SoBaoHiem = model.SoBaoHiem?.Trim(),
                        NhomMau = model.NhomMau,
                        DiUng = model.DiUng?.Trim()
                    };
                    _context.BenhNhan.Add(benhNhan);
                    TempData["SuccessMessage"] = "Thêm thông tin cá nhân thành công!";
                }
                else
                {
                    // Cập nhật thông tin
                    benhNhan.HoTen = model.HoTen.Trim();
                    benhNhan.NgaySinh = model.NgaySinh;
                    benhNhan.GioiTinh = model.GioiTinh;
                    benhNhan.DiaChi = model.DiaChi?.Trim();
                    benhNhan.SoDienThoai = model.SoDienThoai?.Trim();
                    benhNhan.SoBaoHiem = model.SoBaoHiem?.Trim();
                    benhNhan.NhomMau = model.NhomMau;
                    benhNhan.DiUng = model.DiUng?.Trim();

                    _context.BenhNhan.Update(benhNhan);
                    TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu thông tin. Vui lòng thử lại.");
                return View(model);
            }
        }
    }
}
