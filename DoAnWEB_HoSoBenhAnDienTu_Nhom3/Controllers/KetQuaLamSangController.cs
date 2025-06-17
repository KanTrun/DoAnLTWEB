using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class KetQuaLamSangController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Kết quả lâm sàng";
            return View();
        }
    }
}
