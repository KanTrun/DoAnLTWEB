using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            // PHÂN QUYỀN: Nếu là Bác sĩ/Admin thì chuyển sang form Doctor
            if (taiKhoan.VaiTro == "BacSi" || taiKhoan.VaiTro == "Admin")
            {
                return RedirectToAction("Doctor");
            }

            // Nếu là bệnh nhân thì hiển thị form bệnh nhân như cũ
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

        // GET: QuanLyNguoiDung/Doctor
        public async Task<IActionResult> Doctor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Không tìm thấy thông tin người dùng.");

            var taiKhoan = await _context.TaiKhoanNguoiDung
                .Include(t => t.BacSi)
                .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

            if (taiKhoan == null)
                return NotFound("Không tìm thấy tài khoản người dùng.");

            var model = new ManageDoctorViewModel
            {
                IsExisting = taiKhoan.BacSi != null,
                HoTen = taiKhoan.BacSi?.HoTen,
                ChuyenKhoa = taiKhoan.BacSi?.ChuyenKhoa,
                SoDienThoai = taiKhoan.BacSi?.SoDienThoai,
                Email = taiKhoan.BacSi?.Email,
                MaKhoa = taiKhoan.BacSi?.MaKhoa,
                MaBacSi = taiKhoan.BacSi?.MaBacSi,
                KhoaList = await _context.Khoa
                    .Select(k => new SelectListItem
                    {
                        Value = k.MaKhoa.ToString(),
                        Text = k.TenKhoa
                    }).ToListAsync()
            };
            return View(model);
        }

        // POST: QuanLyNguoiDung/Doctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Doctor(ManageDoctorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.KhoaList = await _context.Khoa
                    .Select(k => new SelectListItem
                    {
                        Value = k.MaKhoa.ToString(),
                        Text = k.TenKhoa
                    }).ToListAsync();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound("Không tìm thấy thông tin người dùng.");

            var taiKhoan = await _context.TaiKhoanNguoiDung
                .Include(t => t.BacSi)
                .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

            if (taiKhoan == null)
                return NotFound("Không tìm thấy tài khoản người dùng.");

            try
            {
                if (taiKhoan.BacSi == null)
                {
                    // Thêm mới bác sĩ
                    var bacSi = new BacSi
                    {
                        MaTaiKhoan = taiKhoan.MaTaiKhoan,
                        HoTen = model.HoTen.Trim(),
                        ChuyenKhoa = model.ChuyenKhoa?.Trim(),
                        SoDienThoai = model.SoDienThoai?.Trim(),
                        Email = model.Email?.Trim(),
                        MaKhoa = model.MaKhoa ?? 0
                    };
                    _context.BacSi.Add(bacSi);
                    TempData["SuccessMessage"] = "Thêm thông tin bác sĩ thành công!";
                }
                else
                {
                    // Cập nhật bác sĩ
                    taiKhoan.BacSi.HoTen = model.HoTen.Trim();
                    taiKhoan.BacSi.ChuyenKhoa = model.ChuyenKhoa?.Trim();
                    taiKhoan.BacSi.SoDienThoai = model.SoDienThoai?.Trim();
                    taiKhoan.BacSi.Email = model.Email?.Trim();
                    taiKhoan.BacSi.MaKhoa = model.MaKhoa ?? 0;
                    _context.BacSi.Update(taiKhoan.BacSi);
                    TempData["SuccessMessage"] = "Cập nhật thông tin bác sĩ thành công!";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi lưu thông tin. Vui lòng thử lại.");
                model.KhoaList = await _context.Khoa
                    .Select(k => new SelectListItem
                    {
                        Value = k.MaKhoa.ToString(),
                        Text = k.TenKhoa
                    }).ToListAsync();
                return View(model);
            }
        }

    }
}
