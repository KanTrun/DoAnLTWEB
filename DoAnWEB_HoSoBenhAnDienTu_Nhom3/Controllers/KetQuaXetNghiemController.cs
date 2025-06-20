using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;
using System.Linq;

[Authorize]
public class KetQuaXetNghiemController : Controller
{
    private readonly ApplicationDbContext _context;

    public KetQuaXetNghiemController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ADMIN: Chọn bệnh nhân đã đánh giá lâm sàng
    [Authorize(Roles = "Admin")]
    public IActionResult ChonBenhNhan()
    {
        var list = _context.BenhNhan
            .Include(bn => bn.KetQuaLamSang)
            .Select(bn => new KetQuaXetNghiemChonBenhNhanViewModel
            {
                MaBenhNhan = bn.MaBenhNhan,
                HoTen = bn.HoTen,
                NgaySinh = bn.NgaySinh,
                GioiTinh = bn.GioiTinh,
                NgayKhamGanNhat = bn.KetQuaLamSang.OrderByDescending(k => k.NgayKham).Select(k => (DateTime?)k.NgayKham).FirstOrDefault(),
                DaDanhGiaLamSang = bn.KetQuaLamSang.Any(),
                DanhSachChiDinh = _context.ChiDinhXetNghiem
                    .Where(cd => cd.HoSo.BenhNhan.MaBenhNhan == bn.MaBenhNhan)
                    .Select(cd => new ChiDinhXetNghiemViewModel
                    {
                        MaChiDinh = cd.MaChiDinh,
                        TenXetNghiem = cd.TenXetNghiem,
                        DaDanhGiaKetQua = _context.KetQuaXetNghiem.Any(kq => kq.MaChiDinh == cd.MaChiDinh)
                    }).ToList()
            })
            .Where(x => x.DaDanhGiaLamSang)
            .ToList();

        return View(list);
    }


    // ADMIN: Hiển thị danh sách chỉ định xét nghiệm cho bệnh nhân
    [Authorize(Roles = "Admin")]
    public IActionResult XemChiDinh(int maBenhNhan)
    {
        var chiDinhs = _context.ChiDinhXetNghiem
            .Include(cd => cd.HoSo)
            .Where(cd => cd.HoSo.BenhNhan.MaBenhNhan == maBenhNhan)
            .ToList();
        ViewBag.MaBenhNhan = maBenhNhan;
        return View(chiDinhs);
    }

    // ADMIN: Chỉ định xét nghiệm mới cho bệnh nhân (GET)
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult ChiDinhMoi(int maBenhNhan)
    {
        // Lấy hồ sơ bệnh án mới nhất của bệnh nhân
        var hoSo = _context.HoSoBenhAn
            .Where(h => h.MaBenhNhan == maBenhNhan)
            .OrderByDescending(h => h.NgayNhapVien)
            .FirstOrDefault();

        if (hoSo == null)
        {
            TempData["Error"] = "Bệnh nhân này chưa có hồ sơ bệnh án. Vui lòng tạo hồ sơ bệnh án trước!";
            return RedirectToAction("ChonBenhNhan");
        }

        // Lấy danh sách bác sĩ: value là MaBacSi (int), text là HoTen
        var bacSiList = _context.BacSi
            .Select(bs => new { bs.MaBacSi, bs.HoTen })
            .ToList();
        ViewBag.BacSiList = new SelectList(bacSiList, "MaBacSi", "HoTen");

        var model = new ChiDinhXetNghiem
        {
            MaHoSo = hoSo.MaHoSo,
            NgayChiDinh = DateTime.Now
        };
        ViewBag.MaBenhNhan = maBenhNhan;
        return View(model);
    }

    // ADMIN: Chỉ định xét nghiệm mới cho bệnh nhân (POST)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult ChiDinhMoi(ChiDinhXetNghiem model)
    {
        // Lấy lại danh sách bác sĩ cho dropdown
        var bacSiList = _context.BacSi
            .Select(bs => new { bs.MaBacSi, bs.HoTen })
            .ToList();
        ViewBag.BacSiList = new SelectList(bacSiList, "MaBacSi", "HoTen", model.MaBacSi);

        var hoSo = _context.HoSoBenhAn.FirstOrDefault(h => h.MaHoSo == model.MaHoSo);
        if (hoSo == null)
        {
            ModelState.AddModelError("MaHoSo", "Hồ sơ bệnh án không tồn tại hoặc đã bị xóa!");
            ViewBag.MaBenhNhan = null;
            return View(model);
        }

        // Kiểm tra ModelState và log lỗi nếu có
        if (!ModelState.IsValid || model.MaBacSi == 0)
        {
            if (model.MaBacSi == 0)
                ModelState.AddModelError("MaBacSi", "Bạn phải chọn bác sĩ!");
            ViewBag.MaBenhNhan = hoSo.MaBenhNhan;

            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"ModelState Error - {key}: {error.ErrorMessage}");
                }
            }

            return View(model);
        }

        model.NgayTao = DateTime.Now;
        try
        {
            _context.ChiDinhXetNghiem.Add(model);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
            ViewBag.MaBenhNhan = hoSo.MaBenhNhan;
            return View(model);
        }
        return RedirectToAction("XemChiDinh", new { maBenhNhan = hoSo.MaBenhNhan });
    }

    // ADMIN: Đánh giá kết quả xét nghiệm cho chỉ định
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult DanhGiaKetQua(int maChiDinh)
    {
        var model = new KetQuaXetNghiem { MaChiDinh = maChiDinh, NgayTraKetQua = DateTime.Now };
        var chiDinh = _context.ChiDinhXetNghiem.Include(cd => cd.HoSo).FirstOrDefault(cd => cd.MaChiDinh == maChiDinh);
        ViewBag.MaBenhNhan = chiDinh?.HoSo?.MaBenhNhan;
        return View(model);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public IActionResult DanhGiaKetQua(KetQuaXetNghiem model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            _context.KetQuaXetNghiem.Add(model);

            // Cập nhật trạng thái chỉ định thành "Đã đánh giá"
            var chiDinh = _context.ChiDinhXetNghiem.FirstOrDefault(cd => cd.MaChiDinh == model.MaChiDinh);
            if (chiDinh != null)
            {
                chiDinh.TrangThai = "Đã đánh giá";
                _context.ChiDinhXetNghiem.Update(chiDinh);
            }

            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Lỗi khi lưu: " + ex.Message);
            return View(model);
        }
        return RedirectToAction("ChonBenhNhan");
    }





    // Helper để lấy MaBenhNhan từ MaChiDinh
    private int? GetMaBenhNhanByChiDinh(int maChiDinh)
    {
        var chiDinh = _context.ChiDinhXetNghiem
            .Include(cd => cd.HoSo)
            .FirstOrDefault(cd => cd.MaChiDinh == maChiDinh);
        return chiDinh?.HoSo?.MaBenhNhan;
    }

    // USER: Kết quả của tôi
    [Authorize(Roles = "User")]
    public IActionResult KetQuaCuaToi()
    {
        var userName = User.Identity.Name;
        var benhNhan = _context.BenhNhan.Include(bn => bn.TaiKhoan)
            .FirstOrDefault(bn => bn.TaiKhoan.TenDangNhap == userName);

        if (benhNhan == null)
            return View(new List<KetQuaXetNghiemKetQuaCuaToiViewModel>());

        var list = _context.KetQuaXetNghiem
            .Include(kq => kq.ChiDinh)
                .ThenInclude(cd => cd.BacSi)
            .Include(kq => kq.ChiDinh)
                .ThenInclude(cd => cd.HoSo)
                    .ThenInclude(hs => hs.BenhNhan)
            .Where(kq => kq.ChiDinh.HoSo.BenhNhan.MaBenhNhan == benhNhan.MaBenhNhan)
            .Select(kq => new KetQuaXetNghiemKetQuaCuaToiViewModel
            {
                TenXetNghiem = kq.ChiDinh.TenXetNghiem,
                NgayChiDinh = kq.ChiDinh.NgayChiDinh,
                GiaTriKetQua = kq.GiaTriKetQua,
                KhoangThamChieu = kq.KhoangThamChieu,
                NgayTraKetQua = kq.NgayTraKetQua,
                BacSiDanhGia = kq.ChiDinh.BacSi.HoTen,
                MaKetQuaXN = kq.MaKetQuaXN
            }).ToList();

        return View(list);
    }

    // USER: Xem chi tiết kết quả của tôi
    [Authorize(Roles = "User")]
    public IActionResult ChiTietKetQuaCuaToi(int id)
    {
        var userName = User.Identity.Name;
        var benhNhan = _context.BenhNhan.Include(bn => bn.TaiKhoan)
            .FirstOrDefault(bn => bn.TaiKhoan.TenDangNhap == userName);

        var kq = _context.KetQuaXetNghiem
            .Include(k => k.ChiDinh)
                .ThenInclude(cd => cd.BacSi)
            .Include(k => k.ChiDinh)
                .ThenInclude(cd => cd.HoSo)
                    .ThenInclude(hs => hs.BenhNhan)
            .FirstOrDefault(k => k.MaKetQuaXN == id && k.ChiDinh.HoSo.BenhNhan.MaBenhNhan == benhNhan.MaBenhNhan);

        if (kq == null)
            return NotFound();

        return View(kq);
    }
}
