using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class HoSoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
