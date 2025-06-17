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

        public PhanQuyenController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string roleFilter = "")
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<dynamic>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRole = roles.FirstOrDefault() ?? "User";

                if (string.IsNullOrEmpty(roleFilter) || userRole == roleFilter)
                {
                    userViewModels.Add(new
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Role = userRole,
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

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

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
