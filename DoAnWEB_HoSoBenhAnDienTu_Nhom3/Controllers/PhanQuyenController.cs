using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhanQuyenController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public PhanQuyenController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string roleFilter = "")
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<dynamic>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault() ?? "User";

                // Lấy vai trò thực tế từ TaiKhoanNguoiDung (nếu có)
                var taiKhoan = await _context.TaiKhoanNguoiDung.FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);
                string vaiTroDb = taiKhoan?.VaiTro ?? "";

                // Nếu muốn lọc theo vai trò thực tế trong DB, dùng vaiTroDb thay vì userRole
                if (string.IsNullOrEmpty(roleFilter) || userRole == roleFilter)
                {
                    userViewModels.Add(new
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = userRole,
                        VaiTroDb = vaiTroDb,
                        LockoutEnd = user.LockoutEnd
                    });
                }
            }

            ViewBag.RoleFilter = roleFilter;
            ViewData["Title"] = "Phân quyền người dùng";
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            // Đổi role trong Identity
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            // Đổi VaiTro trong TaiKhoanNguoiDung
            var taiKhoan = await _context.TaiKhoanNguoiDung.FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);
            if (taiKhoan != null)
            {
                // Quy ước: newRole == "Admin" => "BacSi", newRole == "User" => "Bệnh nhân"
                taiKhoan.VaiTro = newRole == "Admin" ? "BacSi" : "Bệnh nhân";
                _context.TaiKhoanNguoiDung.Update(taiKhoan);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = $"Đã cập nhật quyền cho {user.Email} thành {newRole}.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            if (user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now)
            {
                // Khóa tài khoản
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                TempData["SuccessMessage"] = $"Đã khóa tài khoản {user.Email}.";
            }
            else
            {
                // Mở khóa tài khoản
                await _userManager.SetLockoutEndDateAsync(user, null);
                TempData["SuccessMessage"] = $"Đã mở khóa tài khoản {user.Email}.";
            }

            return RedirectToAction("Index");
        }
    }
}
