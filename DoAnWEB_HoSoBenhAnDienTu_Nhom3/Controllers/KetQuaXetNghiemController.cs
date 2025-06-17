using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class KetQuaXetNghiemController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Kết quả xét nghiệm";
            return View();
        }
    }
}
