using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using System.Diagnostics;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Tìm tài khoản người dùng
                    var taiKhoan = await _context.TaiKhoanNguoiDung
                        .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

                    if (taiKhoan != null)
                    {
                        // Tìm thông tin bệnh nhân
                        var benhNhan = await _context.BenhNhan
                            .Include(b => b.TaiKhoan)
                            .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

                        ViewBag.BenhNhan = benhNhan;
                    }
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
