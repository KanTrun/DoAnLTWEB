using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class DonDieuTriController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Đơn điều trị";
            return View();
        }
    }
}
