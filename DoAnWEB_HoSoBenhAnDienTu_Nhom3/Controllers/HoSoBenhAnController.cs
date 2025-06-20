using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using System.Linq;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;

[Authorize(Roles = "Admin,BacSi")]
public class HoSoBenhAnController : Controller
{
    private readonly ApplicationDbContext _context;

    public HoSoBenhAnController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: HoSoBenhAn/Index
    public IActionResult Index()
    {
        var list = _context.BenhNhan
            .Include(bn => bn.HoSoBenhAn)
            .ToList()
            .Select(bn => new
            {
                bn.MaBenhNhan,
                bn.HoTen,
                bn.NgaySinh,
                bn.GioiTinh,
                NgayDanhGiaGanNhat = bn.HoSoBenhAn.OrderByDescending(hs => hs.NgayTao).FirstOrDefault()?.NgayTao,
                DanhGia = bn.HoSoBenhAn.OrderByDescending(hs => hs.NgayTao).FirstOrDefault()?.TomTatBenhAn
            }).ToList();

        return View(list);
    }

    // GET: HoSoBenhAn/DanhGia/5
    public IActionResult DanhGia(int id)
    {
        ViewBag.HinhThucList = new SelectList(_context.HinhThucDieuTri.ToList(), "MaHinhThuc", "TenHinhThuc");
        var hoSoBenhAn = new HoSoBenhAn
        {
            MaBenhNhan = id,
            NgayNhapVien = DateTime.Now
        };
        return View(hoSoBenhAn);
    }

    // POST: HoSoBenhAn/DanhGia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DanhGia(HoSoBenhAn model)
    {
        ViewBag.HinhThucList = new SelectList(_context.HinhThucDieuTri.ToList(), "MaHinhThuc", "TenHinhThuc");
        if (ModelState.IsValid)
        {
            model.NgayTao = DateTime.Now;
            _context.HoSoBenhAn.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }
}
