using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data;
using DoAnWEB_HoSoBenhAnDienTu_Nhom3.ViewModels;
using X.PagedList; // THÊM namespace này


namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Authorize]
    public class DonDieuTriController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DonDieuTriController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Trang cho Admin (Bác sĩ): xem danh sách bệnh nhân để cấp đơn điều trị
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var benhNhans = await _context.BenhNhan.ToListAsync();
            var ds = new List<BenhNhanDonDieuTriViewModel>();

            foreach (var bn in benhNhans)
            {
                var maHoSoGanNhat = _context.HoSoBenhAn
                    .Where(hs => hs.MaBenhNhan == bn.MaBenhNhan)
                    .OrderByDescending(hs => hs.NgayNhapVien)
                    .Select(hs => (int?)hs.MaHoSo)
                    .FirstOrDefault();

                int? maBenhGanNhat = null;
                if (maHoSoGanNhat.HasValue)
                {
                    maBenhGanNhat = _context.ChanDoan
                        .Where(cd => cd.MaHoSo == maHoSoGanNhat)
                        .OrderByDescending(cd => cd.NgayChanDoan)
                        .Select(cd => (int?)cd.MaBenh)
                        .FirstOrDefault();
                }

                var ngayKhamGanNhat = _context.HoSoBenhAn
                    .Where(hs => hs.MaBenhNhan == bn.MaBenhNhan)
                    .OrderByDescending(hs => hs.NgayNhapVien)
                    .Select(hs => (DateTime?)hs.NgayNhapVien)
                    .FirstOrDefault();

                var tenBenh = "";
                if (maBenhGanNhat.HasValue)
                {
                    tenBenh = _context.Benh
                        .Where(b => b.MaBenh == maBenhGanNhat)
                        .Select(b => b.TenBenh)
                        .FirstOrDefault();
                }

                var loaiChanDoan = "";
                if (maHoSoGanNhat.HasValue)
                {
                    loaiChanDoan = _context.ChanDoan
                        .Where(cd => cd.MaHoSo == maHoSoGanNhat)
                        .OrderByDescending(cd => cd.NgayChanDoan)
                        .Select(cd => cd.LoaiChanDoan)
                        .FirstOrDefault();
                }

                // Kiểm tra đã có bệnh (có record ChanDoan với MaBenh)
                var daCoBenh = maHoSoGanNhat.HasValue &&
                              _context.ChanDoan.Any(cd => cd.MaHoSo == maHoSoGanNhat && cd.MaBenh > 0);

                // KEY: Chỉ coi là "đã có chẩn đoán" khi có LoaiChanDoan và MoTa đầy đủ
                var daCoChanDoan = maHoSoGanNhat.HasValue &&
                                  _context.ChanDoan.Any(cd => cd.MaHoSo == maHoSoGanNhat &&
                                                      !string.IsNullOrEmpty(cd.LoaiChanDoan) &&
                                                      !string.IsNullOrEmpty(cd.GhiChu));

                var daCoDonDieuTri = _context.DonDieuTri.Any(ddt =>
                    _context.HoSoBenhAn.Any(hs => hs.MaHoSo == ddt.MaHoSo && hs.MaBenhNhan == bn.MaBenhNhan));

                ds.Add(new BenhNhanDonDieuTriViewModel
                {
                    MaBenhNhan = bn.MaBenhNhan,
                    HoTen = bn.HoTen,
                    NgaySinh = bn.NgaySinh,
                    GioiTinh = bn.GioiTinh,
                    NgayKhamGanNhat = ngayKhamGanNhat,
                    MaHoSoGanNhat = maHoSoGanNhat,
                    MaBenhGanNhat = maBenhGanNhat,
                    TenBenh = tenBenh,
                    LoaiChanDoan = loaiChanDoan,
                    DaCoBenh = daCoBenh,
                    DaCoChanDoan = daCoChanDoan,
                    DaCoDonDieuTri = daCoDonDieuTri
                });
            }
            // THÊM: Logic cho bảng 2 - Đơn cấp thuốc bệnh nhân
            var dsToaThuoc = new List<BenhNhanToaThuocViewModel>();

            foreach (var bn in benhNhans)
            {
                // Kiểm tra đã có đơn điều trị
                var daCoDonDieuTri = _context.DonDieuTri
                    .Any(ddt => _context.HoSoBenhAn
                        .Any(hs => hs.MaHoSo == ddt.MaHoSo && hs.MaBenhNhan == bn.MaBenhNhan));

                // Lấy ngày kê đơn gần nhất
                var ngayKeDonGanNhat = _context.ToaThuoc
                    .Where(tt => _context.HoSoBenhAn
                        .Any(hs => hs.MaHoSo == tt.MaHoSo && hs.MaBenhNhan == bn.MaBenhNhan))
                    .OrderByDescending(tt => tt.NgayKeDon)
                    .Select(tt => (DateTime?)tt.NgayKeDon)
                    .FirstOrDefault();

                dsToaThuoc.Add(new BenhNhanToaThuocViewModel
                {
                    MaBenhNhan = bn.MaBenhNhan,
                    HoTen = bn.HoTen,
                    DaCoDonDieuTri = daCoDonDieuTri,
                    NgayKeDonGanNhat = ngayKeDonGanNhat
                });
            }

            // THÊM: Truyền dữ liệu bảng 2 vào ViewBag
            ViewBag.DanhSachToaThuoc = dsToaThuoc;


            return View(ds);
        }






        // 2. ADMIN: FORM NHẬP BỆNH cho bệnh nhân (KHÔNG hiển thị danh sách, chỉ là form)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult ChonBenh(int maBenhNhan, int maHoSo)
        {
            ViewBag.MaBenhNhan = maBenhNhan;
            ViewBag.MaHoSo = maHoSo;
            return View(new Benh());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChonBenh(int maBenhNhan, int maHoSo, Benh model)
        {
            ViewBag.MaBenhNhan = maBenhNhan;
            ViewBag.MaHoSo = maHoSo;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Thêm bệnh
            _context.Benh.Add(model);
            await _context.SaveChangesAsync();

            // Kiểm tra xem có chẩn đoán cho hồ sơ này chưa
            var existingChanDoan = _context.ChanDoan
                .FirstOrDefault(cd => cd.MaHoSo == maHoSo && cd.MaBenh == model.MaBenh);

            if (existingChanDoan == null)
            {
                // Tạo chẩn đoán mới (chưa có LoaiChanDoan)
                var chanDoan = new ChanDoan
                {
                    MaHoSo = maHoSo,
                    MaBenh = model.MaBenh,
                    NgayChanDoan = DateTime.Now,
                    NgayTao = DateTime.Now
                    // LoaiChanDoan = null (sẽ nhập ở bước tiếp theo)
                };

                var user = await _userManager.GetUserAsync(User);
                var bacSi = await _context.BacSi
                    .Include(bs => bs.TaiKhoan)
                    .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

                if (bacSi != null)
                {
                    chanDoan.MaBacSi = bacSi.MaBacSi;
                }

                _context.ChanDoan.Add(chanDoan);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Thêm thông tin bệnh thành công! Bây giờ có thể nhập chẩn đoán.";
            return RedirectToAction("Index");
        }


        // 3. ADMIN: Nhập chẩn đoán cho bệnh nhân (hiển thị form)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ChonChanDoan(int maBenhNhan, int maHoSo, int maBenh)
        {
            if (maHoSo == 0 || maBenh == 0)
            {
                TempData["Error"] = "Không tìm thấy mã hồ sơ hoặc mã bệnh hợp lệ. Hãy đảm bảo đã nhập bệnh trước khi chẩn đoán.";
                return RedirectToAction("Index");
            }
            var model = new ChanDoan
            {
                MaHoSo = maHoSo,
                MaBenh = maBenh,
                NgayChanDoan = DateTime.Now
            };
            ViewBag.MaBenhNhan = maBenhNhan;
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChonChanDoan(ChanDoan model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var bacSi = await _context.BacSi
                .Include(bs => bs.TaiKhoan)
                .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

            if (bacSi == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin bác sĩ.");
                return View(model);
            }

            // SỬA: Kiểm tra xem đã có chẩn đoán chưa để cập nhật hoặc tạo mới
            var existingChanDoan = _context.ChanDoan
                .FirstOrDefault(cd => cd.MaHoSo == model.MaHoSo && cd.MaBenh == model.MaBenh);

            try
            {
                if (existingChanDoan != null)
                {
                    // Cập nhật chẩn đoán có sẵn
                    existingChanDoan.LoaiChanDoan = model.LoaiChanDoan;
                    existingChanDoan.GhiChu = model.GhiChu;
                    existingChanDoan.NgayChanDoan = DateTime.Now;
                    existingChanDoan.MaBacSi = bacSi.MaBacSi;

                    _context.ChanDoan.Update(existingChanDoan);
                    TempData["Success"] = "Cập nhật chẩn đoán thành công!";
                }
                else
                {
                    // Tạo chẩn đoán mới
                    model.MaBacSi = bacSi.MaBacSi;
                    model.NgayTao = DateTime.Now;
                    model.NgayChanDoan = DateTime.Now;

                    _context.ChanDoan.Add(model);
                    TempData["Success"] = "Tạo chẩn đoán mới thành công!";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}");
                ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.Message);
                return View(model);
            }
        }




        // 4. ADMIN: Nhập đơn điều trị cho bệnh nhân (hiển thị form)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ChonDonDieuTri(int maBenhNhan, int maHoSo)
        {
            var model = new DonDieuTri
            {
                MaHoSo = maHoSo,
                NgayBatDau = DateTime.Now // Đặt ngày mặc định
            };
            ViewBag.MaBenhNhan = maBenhNhan;
            return View(model);
        }


        // 5. ADMIN: Lưu đơn điều trị (POST)
        // SỬA: Action ChonDonDieuTri POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChonDonDieuTri(DonDieuTri model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MaBenhNhan = Request.Query["maBenhNhan"];
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var bacSi = await _context.BacSi
                .Include(bs => bs.TaiKhoan)
                .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

            if (bacSi == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin bác sĩ.");
                ViewBag.MaBenhNhan = Request.Query["maBenhNhan"];
                return View(model);
            }

            model.MaBacSi = bacSi.MaBacSi;

            try
            {
                // LUÔN tạo đơn điều trị mới (cho phép tạo nhiều đơn cho cùng 1 bệnh nhân)
                _context.DonDieuTri.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Tạo đơn điều trị mới thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lưu database: {ex.Message}");
                ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.Message);
                ViewBag.MaBenhNhan = Request.Query["maBenhNhan"];
                return View(model);
            }
        }




        // XÓA hoặc SỬA action LuuDonDieuTri (không cần thiết nữa)


        // 6. USER: Xem đơn điều trị của mình
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserDonDieuTri(int? page)
        {
            var user = await _userManager.GetUserAsync(User);
            var taiKhoan = await _context.TaiKhoanNguoiDung.FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);
            var benhNhan = await _context.BenhNhan.FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

            // Lấy toàn bộ dữ liệu đơn điều trị
            var dsQuery = (from hs in _context.HoSoBenhAn
                           join ddt in _context.DonDieuTri on hs.MaHoSo equals ddt.MaHoSo
                           join cd in _context.ChanDoan on hs.MaHoSo equals cd.MaHoSo into cds
                           from cd in cds.DefaultIfEmpty()
                           join b in _context.Benh on cd.MaBenh equals b.MaBenh into bs
                           from b in bs.DefaultIfEmpty()
                           join bsObj in _context.BacSi on ddt.MaBacSi equals bsObj.MaBacSi
                           where hs.MaBenhNhan == benhNhan.MaBenhNhan
                           orderby ddt.NgayBatDau descending
                           select new DonDieuTriUserViewModel
                           {
                               MaDonDieuTri = ddt.MaDonDieuTri,
                               HoTen = benhNhan.HoTen,
                               NgaySinh = benhNhan.NgaySinh,
                               GioiTinh = benhNhan.GioiTinh,
                               TenBenh = b.TenBenh,
                               LoaiChanDoan = cd.LoaiChanDoan,
                               TenDonDieuTri = ddt.TenDonDieuTri,
                               NgayBatDau = ddt.NgayBatDau,
                               NgayKetThuc = ddt.NgayKetThuc,
                               BacSi = bsObj.HoTen
                           });

            // Phân trang: 5 mục/trang
            var pageNumber = page ?? 1;
            var pageSize = 5;
            var totalItemCount = await dsQuery.CountAsync();
            var items = await dsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tạo đối tượng phân trang
            var pagedList = new StaticPagedList<DonDieuTriUserViewModel>(
                items,
                pageNumber,
                pageSize,
                totalItemCount
            );

            var model = new DonDieuTriUserTongHopViewModel
            {
                BenhNhan = benhNhan,
                DonDieuTris = pagedList
            };

            return View(model);
        }




        // 7. USER: Xem chi tiết đơn điều trị
        [Authorize(Roles = "User")]
        public async Task<IActionResult> XemDonDieuTri(int maDonDieuTri)
        {
            var don = await _context.DonDieuTri
                .Include(d => d.BacSi)
                .FirstOrDefaultAsync(d => d.MaDonDieuTri == maDonDieuTri);
            if (don == null) return NotFound();
            return View(don);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult KeThuoc(int maBenhNhan)
        {
            // Lấy hồ sơ gần nhất của bệnh nhân
            var maHoSo = _context.HoSoBenhAn
                .Where(hs => hs.MaBenhNhan == maBenhNhan)
                .OrderByDescending(hs => hs.NgayNhapVien)
                .Select(hs => hs.MaHoSo)
                .FirstOrDefault();

            if (maHoSo == 0)
            {
                TempData["Error"] = "Bệnh nhân chưa có hồ sơ bệnh án";
                return RedirectToAction("Index");
            }

            ViewBag.MaBenhNhan = maBenhNhan;
            ViewBag.MaHoSo = maHoSo;

            // Tạo ViewModel cho form kê thuốc
            var viewModel = new KeThuocViewModel
            {
                ThuocMoi = new Thuoc(),
                ChiTietToaThuoc = new ChiTietToaThuoc(),
                DanhSachThuocCoSan = _context.Thuoc.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> KeThuoc(KeThuocViewModel model, int maBenhNhan, int maHoSo)
        {
            // Debug: Kiểm tra dữ liệu nhận được
            System.Diagnostics.Debug.WriteLine($"=== DEBUG KeThuoc ===");
            System.Diagnostics.Debug.WriteLine($"SuDungThuocMoi: {model.SuDungThuocMoi}");
            System.Diagnostics.Debug.WriteLine($"MaThuocCoSan: {model.MaThuocCoSan}");
            System.Diagnostics.Debug.WriteLine($"LieuLuong: '{model.LieuLuong}'");
            System.Diagnostics.Debug.WriteLine($"SoLanUong: '{model.SoLanUong}'");
            System.Diagnostics.Debug.WriteLine($"TenThuocMoi: '{model.TenThuocMoi}'");

            // XÓA validation errors không cần thiết
            ModelState.Remove("ThuocMoi.TenThuoc");
            ModelState.Remove("ChiTietToaThuoc.ToaThuoc");
            ModelState.Remove("ChiTietToaThuoc.Thuoc");
            ModelState.Remove("ChiTietToaThuoc.MaToaThuoc");
            ModelState.Remove("ChiTietToaThuoc.MaThuoc");
            ModelState.Remove("ChiTietToaThuoc.LieuLuong");
            ModelState.Remove("ChiTietToaThuoc.SoLanUong");

            // Validation tùy chỉnh
            bool hasErrors = false;

            if (model.SuDungThuocMoi)
            {
                if (string.IsNullOrWhiteSpace(model.TenThuocMoi))
                {
                    ModelState.AddModelError("TenThuocMoi", "Tên thuốc là bắt buộc khi thêm thuốc mới");
                    hasErrors = true;
                }
            }
            else
            {
                if (model.MaThuocCoSan <= 0)
                {
                    ModelState.AddModelError("MaThuocCoSan", "Vui lòng chọn thuốc có sẵn");
                    hasErrors = true;
                }
            }

            if (string.IsNullOrWhiteSpace(model.LieuLuong))
            {
                ModelState.AddModelError("LieuLuong", "Liều lượng là bắt buộc");
                hasErrors = true;
            }

            if (string.IsNullOrWhiteSpace(model.SoLanUong))
            {
                ModelState.AddModelError("SoLanUong", "Số lần uống là bắt buộc");
                hasErrors = true;
            }

            if (hasErrors)
            {
                // Debug: In ra tất cả lỗi validation
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }

                model.DanhSachThuocCoSan = _context.Thuoc.ToList();
                ViewBag.MaBenhNhan = maBenhNhan;
                ViewBag.MaHoSo = maHoSo;
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            var bacSi = await _context.BacSi
                .Include(bs => bs.TaiKhoan)
                .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

            if (bacSi == null)
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin bác sĩ.");
                model.DanhSachThuocCoSan = _context.Thuoc.ToList();
                ViewBag.MaBenhNhan = maBenhNhan;
                ViewBag.MaHoSo = maHoSo;
                return View(model);
            }

            try
            {
                // BƯỚC 1: Tạo ToaThuoc trước
                var toaThuoc = new ToaThuoc
                {
                    MaHoSo = maHoSo,
                    MaBacSi = bacSi.MaBacSi,
                    NgayKeDon = DateTime.Now,
                    TrangThai = "Đã kê",
                    NgayTao = DateTime.Now
                };

                _context.ToaThuoc.Add(toaThuoc);
                await _context.SaveChangesAsync(); // Lưu để có MaToaThuoc

                System.Diagnostics.Debug.WriteLine($"Đã tạo ToaThuoc với ID: {toaThuoc.MaToaThuoc}");

                // BƯỚC 2: Xử lý thuốc
                int maThuocSuDung;

                if (model.SuDungThuocMoi)
                {
                    // Tạo thuốc mới từ dữ liệu riêng
                    var thuocMoi = new Thuoc
                    {
                        TenThuoc = model.TenThuocMoi,
                        HoatChat = model.HoatChatMoi,
                        DonVi = model.DonViMoi,
                        Gia = model.GiaMoi,
                        CongDung = model.CongDungMoi,
                        CachDung = model.CachDungMoi,
                        SoLuongTon = model.SoLuongTonMoi,
                        HanSuDung = model.HanSuDungMoi
                    };

                    _context.Thuoc.Add(thuocMoi);
                    await _context.SaveChangesAsync();
                    maThuocSuDung = thuocMoi.MaThuoc;

                    System.Diagnostics.Debug.WriteLine($"Đã tạo Thuoc mới với ID: {maThuocSuDung}");
                }
                else
                {
                    maThuocSuDung = model.MaThuocCoSan;
                    System.Diagnostics.Debug.WriteLine($"Sử dụng Thuoc có sẵn ID: {maThuocSuDung}");
                }

                // BƯỚC 3: Tạo chi tiết toa thuốc từ dữ liệu riêng
                var chiTiet = new ChiTietToaThuoc
                {
                    MaToaThuoc = toaThuoc.MaToaThuoc,
                    MaThuoc = maThuocSuDung,
                    LieuLuong = model.LieuLuong,
                    SoLanUong = model.SoLanUong,
                    SoLuong = model.SoLuong ?? 1
                };

                System.Diagnostics.Debug.WriteLine($"Tạo ChiTietToaThuoc: ToaThuoc={chiTiet.MaToaThuoc}, Thuoc={chiTiet.MaThuoc}, LieuLuong={chiTiet.LieuLuong}");

                _context.ChiTietToaThuoc.Add(chiTiet);
                var result = await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Đã lưu {result} bản ghi ChiTietToaThuoc");

                TempData["Success"] = "Kê thuốc thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                ModelState.AddModelError("", "Lỗi lưu dữ liệu: " + ex.Message);

                model.DanhSachThuocCoSan = _context.Thuoc.ToList();
                ViewBag.MaBenhNhan = maBenhNhan;
                ViewBag.MaHoSo = maHoSo;
                return View(model);
            }
        }





        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LuuKeThuoc(int maBenhNhan, int maHoSo, List<ChiTietToaThuoc> chiTietList)
        {
            // Tạo toa thuoc mới
            var user = await _userManager.GetUserAsync(User);
            var bacSi = await _context.BacSi
                .Include(bs => bs.TaiKhoan)
                .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

            var toaThuoc = new ToaThuoc
            {
                MaHoSo = maHoSo,
                MaBacSi = bacSi.MaBacSi,
                NgayKeDon = DateTime.Now,
                TrangThai = "Đã kê"
            };

            _context.ToaThuoc.Add(toaThuoc);
            await _context.SaveChangesAsync();

            // Thêm chi tiết
            foreach (var ct in chiTietList)
            {
                ct.MaToaThuoc = toaThuoc.MaToaThuoc;
                _context.ChiTietToaThuoc.Add(ct);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Kê thuốc thành công!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ToaThuocChiDinh(int maBenhNhan)
        {
            // Lấy hồ sơ gần nhất
            var maHoSo = _context.HoSoBenhAn
                .Where(hs => hs.MaBenhNhan == maBenhNhan)
                .OrderByDescending(hs => hs.NgayNhapVien)
                .Select(hs => hs.MaHoSo)
                .FirstOrDefault();

            if (maHoSo == 0)
            {
                TempData["Error"] = "Bệnh nhân chưa có hồ sơ bệnh án";
                return RedirectToAction("Index");
            }

            var model = new ToaThuoc
            {
                MaHoSo = maHoSo,
                NgayKeDon = DateTime.Now
            };

            ViewBag.MaBenhNhan = maBenhNhan;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToaThuocChiDinh(ToaThuoc model)
        {
            var user = await _userManager.GetUserAsync(User);
            var bacSi = await _context.BacSi
                .Include(bs => bs.TaiKhoan)
                .FirstOrDefaultAsync(bs => bs.TaiKhoan.TenDangNhap == user.UserName);

            model.MaBacSi = bacSi.MaBacSi;
            model.NgayTao = DateTime.Now;

            // CHỈ gán mặc định khi không có giá trị từ form
            if (string.IsNullOrEmpty(model.TrangThai))
            {
                model.TrangThai = "Chờ cấp phát";
            }

            _context.ToaThuoc.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã cấp toa thuốc cho bệnh nhân!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> UserToaThuoc(int? page)
        {
            try
            {
                // Lấy thông tin user hiện tại
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                    return RedirectToAction("Index", "Home");
                }

                // Lấy tài khoản và bệnh nhân
                var taiKhoan = await _context.TaiKhoanNguoiDung
                    .FirstOrDefaultAsync(t => t.TenDangNhap == user.UserName);

                if (taiKhoan == null)
                {
                    TempData["Error"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction("Index", "Home");
                }

                var benhNhan = await _context.BenhNhan
                    .FirstOrDefaultAsync(b => b.MaTaiKhoan == taiKhoan.MaTaiKhoan);

                if (benhNhan == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin bệnh nhân.";
                    return RedirectToAction("Index", "Home");
                }

                // Query danh sách toa thuốc (chưa ToListAsync)
                var toaThuocQuery = _context.ToaThuoc
                    .Include(t => t.BacSi) // Include thông tin bác sĩ
                    .Where(t => _context.HoSoBenhAn
                        .Any(hs => hs.MaHoSo == t.MaHoSo && hs.MaBenhNhan == benhNhan.MaBenhNhan))
                    .OrderByDescending(t => t.NgayKeDon);

                // Phân trang: 5 mục/trang cho toa thuốc
                var pageNumber = page ?? 1;
                var pageSize = 5;
                var totalItemCount = await toaThuocQuery.CountAsync();
                var toaThuocItems = await toaThuocQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Tạo đối tượng phân trang
                var pagedToaThuocs = new StaticPagedList<ToaThuoc>(
                    toaThuocItems,
                    pageNumber,
                    pageSize,
                    totalItemCount
                );

                // Lấy tất cả chi tiết toa thuốc (không phân trang)
                var allToaThuocs = await toaThuocQuery.ToListAsync();
                var maToaThuocs = allToaThuocs.Select(t => t.MaToaThuoc).ToList();
                var chiTietToaThuocs = await _context.ChiTietToaThuoc
                    .Include(ct => ct.Thuoc) // Include thông tin thuốc
                    .Where(ct => maToaThuocs.Contains(ct.MaToaThuoc))
                    .ToListAsync();

                // Tạo ViewModel
                var viewModel = new UserToaThuocViewModel
                {
                    BenhNhan = benhNhan,
                    ToaThuocs = pagedToaThuocs, // Sử dụng dữ liệu đã phân trang
                    ChiTietToaThuocs = chiTietToaThuocs
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi UserToaThuoc: {ex.Message}");
                TempData["Error"] = "Có lỗi xảy ra khi tải thông tin đơn cấp thuốc.";
                return RedirectToAction("Index", "Home");
            }
        }




    }
}
