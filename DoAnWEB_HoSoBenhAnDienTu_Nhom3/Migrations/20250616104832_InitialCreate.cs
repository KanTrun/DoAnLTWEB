using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Benh",
                columns: table => new
                {
                    MaBenh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenBenh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benh", x => x.MaBenh);
                });

            migrationBuilder.CreateTable(
                name: "HinhThucDieuTri",
                columns: table => new
                {
                    MaHinhThuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHinhThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhThucDieuTri", x => x.MaHinhThuc);
                });

            migrationBuilder.CreateTable(
                name: "Khoa",
                columns: table => new
                {
                    MaKhoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoa", x => x.MaKhoa);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoanNguoiDung",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoanNguoiDung", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "Thuoc",
                columns: table => new
                {
                    MaThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuoc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoatChat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DangBaoChe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thuoc", x => x.MaThuoc);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacSi",
                columns: table => new
                {
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    MaKhoa = table.Column<int>(type: "int", nullable: false),
                    SoGiayPhep = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ChuyenKhoa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacSi", x => x.MaBacSi);
                    table.ForeignKey(
                        name: "FK_BacSi_Khoa_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoa",
                        principalColumn: "MaKhoa");
                    table.ForeignKey(
                        name: "FK_BacSi_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BenhNhan",
                columns: table => new
                {
                    MaBenhNhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoBaoHiem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NhomMau = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DiUng = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenhNhan", x => x.MaBenhNhan);
                    table.ForeignKey(
                        name: "FK_BenhNhan_TaiKhoanNguoiDung_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TaiKhoanNguoiDung",
                        principalColumn: "MaTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoSoBenhAn",
                columns: table => new
                {
                    MaHoSo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBenhNhan = table.Column<int>(type: "int", nullable: false),
                    MaHinhThuc = table.Column<int>(type: "int", nullable: false),
                    NgayVaoVien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayRaVien = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Đang điều trị")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoSoBenhAn", x => x.MaHoSo);
                    table.ForeignKey(
                        name: "FK_HoSoBenhAn_BenhNhan_MaBenhNhan",
                        column: x => x.MaBenhNhan,
                        principalTable: "BenhNhan",
                        principalColumn: "MaBenhNhan",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoSoBenhAn_HinhThucDieuTri_MaHinhThuc",
                        column: x => x.MaHinhThuc,
                        principalTable: "HinhThucDieuTri",
                        principalColumn: "MaHinhThuc");
                });

            migrationBuilder.CreateTable(
                name: "ChanDoan",
                columns: table => new
                {
                    MaChanDoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    MaBenh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NgayChanDoan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChanDoan", x => x.MaChanDoan);
                    table.ForeignKey(
                        name: "FK_ChanDoan_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi");
                    table.ForeignKey(
                        name: "FK_ChanDoan_Benh_MaBenh",
                        column: x => x.MaBenh,
                        principalTable: "Benh",
                        principalColumn: "MaBenh");
                    table.ForeignKey(
                        name: "FK_ChanDoan_HoSoBenhAn_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoBenhAn",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "ChiDinhXetNghiem",
                columns: table => new
                {
                    MaChiDinh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    LoaiXetNghiem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayChiDinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiDinhXetNghiem", x => x.MaChiDinh);
                    table.ForeignKey(
                        name: "FK_ChiDinhXetNghiem_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi");
                    table.ForeignKey(
                        name: "FK_ChiDinhXetNghiem_HoSoBenhAn_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoBenhAn",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonDieuTri",
                columns: table => new
                {
                    MaDonDieuTri = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    TenDonDieuTri = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonDieuTri", x => x.MaDonDieuTri);
                    table.ForeignKey(
                        name: "FK_DonDieuTri_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi");
                    table.ForeignKey(
                        name: "FK_DonDieuTri_HoSoBenhAn_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoBenhAn",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "ThamKhamLamSang",
                columns: table => new
                {
                    MaThamKham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    NgayThamKham = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrieuChung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DauHieuThucThe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThamKhamLamSang", x => x.MaThamKham);
                    table.ForeignKey(
                        name: "FK_ThamKhamLamSang_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi");
                    table.ForeignKey(
                        name: "FK_ThamKhamLamSang_HoSoBenhAn_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoBenhAn",
                        principalColumn: "MaHoSo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ToaThuoc",
                columns: table => new
                {
                    MaToaThuoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoSo = table.Column<int>(type: "int", nullable: false),
                    NgayKeDon = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaBacSi = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToaThuoc", x => x.MaToaThuoc);
                    table.ForeignKey(
                        name: "FK_ToaThuoc_BacSi_MaBacSi",
                        column: x => x.MaBacSi,
                        principalTable: "BacSi",
                        principalColumn: "MaBacSi");
                    table.ForeignKey(
                        name: "FK_ToaThuoc_HoSoBenhAn_MaHoSo",
                        column: x => x.MaHoSo,
                        principalTable: "HoSoBenhAn",
                        principalColumn: "MaHoSo");
                });

            migrationBuilder.CreateTable(
                name: "KetQuaXetNghiem",
                columns: table => new
                {
                    MaKetQuaXN = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChiDinh = table.Column<int>(type: "int", nullable: false),
                    GiaTriKetQua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KhoangThamChieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayTraKetQua = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaXetNghiem", x => x.MaKetQuaXN);
                    table.ForeignKey(
                        name: "FK_KetQuaXetNghiem_ChiDinhXetNghiem_MaChiDinh",
                        column: x => x.MaChiDinh,
                        principalTable: "ChiDinhXetNghiem",
                        principalColumn: "MaChiDinh",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaLamSang",
                columns: table => new
                {
                    MaKetQua = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaThamKham = table.Column<int>(type: "int", nullable: false),
                    NhietDo = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HuyetAp = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NhipTim = table.Column<int>(type: "int", nullable: true),
                    NhipTho = table.Column<int>(type: "int", nullable: true),
                    DanhGiaTongThe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaLamSang", x => x.MaKetQua);
                    table.ForeignKey(
                        name: "FK_KetQuaLamSang_ThamKhamLamSang_MaThamKham",
                        column: x => x.MaThamKham,
                        principalTable: "ThamKhamLamSang",
                        principalColumn: "MaThamKham",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietToaThuoc",
                columns: table => new
                {
                    MaToaThuoc = table.Column<int>(type: "int", nullable: false),
                    MaThuoc = table.Column<int>(type: "int", nullable: false),
                    LieuLuong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoLanUong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietToaThuoc", x => new { x.MaToaThuoc, x.MaThuoc });
                    table.ForeignKey(
                        name: "FK_ChiTietToaThuoc_Thuoc_MaThuoc",
                        column: x => x.MaThuoc,
                        principalTable: "Thuoc",
                        principalColumn: "MaThuoc");
                    table.ForeignKey(
                        name: "FK_ChiTietToaThuoc_ToaThuoc_MaToaThuoc",
                        column: x => x.MaToaThuoc,
                        principalTable: "ToaThuoc",
                        principalColumn: "MaToaThuoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HinhThucDieuTri",
                columns: new[] { "MaHinhThuc", "TenHinhThuc" },
                values: new object[,]
                {
                    { 1, "Nội trú" },
                    { 2, "Bán trú" },
                    { 3, "Ngoại trú" }
                });

            migrationBuilder.InsertData(
                table: "Khoa",
                columns: new[] { "MaKhoa", "MoTa", "TenKhoa" },
                values: new object[,]
                {
                    { 1, "Khoa điều trị các bệnh nội khoa", "Khoa Nội" },
                    { 2, "Khoa phẫu thuật và điều trị ngoại khoa", "Khoa Ngoại" },
                    { 3, "Khoa sản phụ khoa", "Khoa Sản" },
                    { 4, "Khoa điều trị trẻ em", "Khoa Nhi" },
                    { 5, "Khoa cấp cứu và hồi sức", "Khoa Cấp cứu" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BacSi_MaKhoa",
                table: "BacSi",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_BacSi_MaTaiKhoan",
                table: "BacSi",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BenhNhan_MaTaiKhoan",
                table: "BenhNhan",
                column: "MaTaiKhoan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChanDoan_MaBacSi",
                table: "ChanDoan",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_ChanDoan_MaBenh",
                table: "ChanDoan",
                column: "MaBenh");

            migrationBuilder.CreateIndex(
                name: "IX_ChanDoan_MaHoSo",
                table: "ChanDoan",
                column: "MaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiDinhXetNghiem_MaBacSi",
                table: "ChiDinhXetNghiem",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_ChiDinhXetNghiem_MaHoSo",
                table: "ChiDinhXetNghiem",
                column: "MaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietToaThuoc_MaThuoc",
                table: "ChiTietToaThuoc",
                column: "MaThuoc");

            migrationBuilder.CreateIndex(
                name: "IX_DonDieuTri_MaBacSi",
                table: "DonDieuTri",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_DonDieuTri_MaHoSo",
                table: "DonDieuTri",
                column: "MaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBenhAn_MaBenhNhan",
                table: "HoSoBenhAn",
                column: "MaBenhNhan");

            migrationBuilder.CreateIndex(
                name: "IX_HoSoBenhAn_MaHinhThuc",
                table: "HoSoBenhAn",
                column: "MaHinhThuc");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaLamSang_MaThamKham",
                table: "KetQuaLamSang",
                column: "MaThamKham");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaXetNghiem_MaChiDinh",
                table: "KetQuaXetNghiem",
                column: "MaChiDinh");

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoanNguoiDung_TenDangNhap",
                table: "TaiKhoanNguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThamKhamLamSang_MaBacSi",
                table: "ThamKhamLamSang",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_ThamKhamLamSang_MaHoSo",
                table: "ThamKhamLamSang",
                column: "MaHoSo");

            migrationBuilder.CreateIndex(
                name: "IX_ToaThuoc_MaBacSi",
                table: "ToaThuoc",
                column: "MaBacSi");

            migrationBuilder.CreateIndex(
                name: "IX_ToaThuoc_MaHoSo",
                table: "ToaThuoc",
                column: "MaHoSo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ChanDoan");

            migrationBuilder.DropTable(
                name: "ChiTietToaThuoc");

            migrationBuilder.DropTable(
                name: "DonDieuTri");

            migrationBuilder.DropTable(
                name: "KetQuaLamSang");

            migrationBuilder.DropTable(
                name: "KetQuaXetNghiem");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Benh");

            migrationBuilder.DropTable(
                name: "Thuoc");

            migrationBuilder.DropTable(
                name: "ToaThuoc");

            migrationBuilder.DropTable(
                name: "ThamKhamLamSang");

            migrationBuilder.DropTable(
                name: "ChiDinhXetNghiem");

            migrationBuilder.DropTable(
                name: "BacSi");

            migrationBuilder.DropTable(
                name: "HoSoBenhAn");

            migrationBuilder.DropTable(
                name: "Khoa");

            migrationBuilder.DropTable(
                name: "BenhNhan");

            migrationBuilder.DropTable(
                name: "HinhThucDieuTri");

            migrationBuilder.DropTable(
                name: "TaiKhoanNguoiDung");
        }
    }
}
