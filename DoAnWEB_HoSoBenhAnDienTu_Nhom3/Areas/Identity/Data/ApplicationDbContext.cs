using DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Areas.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - GIỮ NGUYÊN CODE CŨ
        public DbSet<TaiKhoanNguoiDung> TaiKhoanNguoiDung { get; set; }
        public DbSet<BenhNhan> BenhNhan { get; set; }
        public DbSet<BacSi> BacSi { get; set; }
        public DbSet<Khoa> Khoa { get; set; }
        public DbSet<HinhThucDieuTri> HinhThucDieuTri { get; set; }
        public DbSet<HoSoBenhAn> HoSoBenhAn { get; set; }
        public DbSet<ThamKhamLamSang> ThamKhamLamSang { get; set; }
        public DbSet<KetQuaLamSang> KetQuaLamSang { get; set; }
        public DbSet<ChiDinhXetNghiem> ChiDinhXetNghiem { get; set; }
        public DbSet<KetQuaXetNghiem> KetQuaXetNghiem { get; set; }
        public DbSet<Benh> Benh { get; set; }
        public DbSet<ChanDoan> ChanDoan { get; set; }
        public DbSet<Thuoc> Thuoc { get; set; }
        public DbSet<ToaThuoc> ToaThuoc { get; set; }
        public DbSet<ChiTietToaThuoc> ChiTietToaThuoc { get; set; }
        public DbSet<DonDieuTri> DonDieuTri { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Tắt cảnh báo PendingModelChanges
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // GIỮ NGUYÊN TẤT CẢ CẤU HÌNH CŨ
            // Cấu hình tên bảng chính xác (không có 's' ở cuối)
            modelBuilder.Entity<TaiKhoanNguoiDung>().ToTable("TaiKhoanNguoiDung");
            modelBuilder.Entity<BenhNhan>().ToTable("BenhNhan");
            modelBuilder.Entity<BacSi>().ToTable("BacSi");
            modelBuilder.Entity<Khoa>().ToTable("Khoa");
            modelBuilder.Entity<HinhThucDieuTri>().ToTable("HinhThucDieuTri");
            modelBuilder.Entity<HoSoBenhAn>().ToTable("HoSoBenhAn");
            modelBuilder.Entity<ThamKhamLamSang>().ToTable("ThamKhamLamSang");
            modelBuilder.Entity<KetQuaLamSang>().ToTable("KetQuaLamSang");
            modelBuilder.Entity<ChiDinhXetNghiem>().ToTable("ChiDinhXetNghiem");
            modelBuilder.Entity<KetQuaXetNghiem>().ToTable("KetQuaXetNghiem");
            modelBuilder.Entity<Benh>().ToTable("Benh");
            modelBuilder.Entity<ChanDoan>().ToTable("ChanDoan");
            modelBuilder.Entity<Thuoc>().ToTable("Thuoc");
            modelBuilder.Entity<ToaThuoc>().ToTable("ToaThuoc");
            modelBuilder.Entity<ChiTietToaThuoc>().ToTable("ChiTietToaThuoc");
            modelBuilder.Entity<DonDieuTri>().ToTable("DonDieuTri");

            // Cấu hình các ràng buộc và quan hệ - GIỮ NGUYÊN
            // TaiKhoanNguoiDung
            modelBuilder.Entity<TaiKhoanNguoiDung>(entity =>
            {
                entity.HasIndex(e => e.TenDangNhap).IsUnique();
                entity.Property(e => e.VaiTro).HasMaxLength(20);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");
            });

            // BenhNhan
            modelBuilder.Entity<BenhNhan>(entity =>
            {
                entity.HasIndex(e => e.MaTaiKhoan).IsUnique();
                entity.HasOne(d => d.TaiKhoan)
                    .WithOne(p => p.BenhNhan)
                    .HasForeignKey<BenhNhan>(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.GioiTinh).HasMaxLength(10);
            });

            // BacSi
            modelBuilder.Entity<BacSi>(entity =>
            {
                entity.HasIndex(e => e.MaTaiKhoan).IsUnique();
                entity.HasOne(d => d.TaiKhoan)
                    .WithOne(p => p.BacSi)
                    .HasForeignKey<BacSi>(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Khoa)
                    .WithMany(p => p.BacSis)
                    .HasForeignKey(d => d.MaKhoa)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // HoSoBenhAn
            modelBuilder.Entity<HoSoBenhAn>(entity =>
            {
                entity.Property(e => e.TrangThai)
                    .HasMaxLength(20)
                    .HasDefaultValue("Đang điều trị");
                entity.HasOne(d => d.BenhNhan)
                    .WithMany(p => p.HoSoBenhAn)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.HinhThucDieuTri)
                    .WithMany(p => p.HoSoBenhAns)
                    .HasForeignKey(d => d.MaHinhThuc)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ThamKhamLamSang
            modelBuilder.Entity<ThamKhamLamSang>(entity =>
            {
                entity.HasOne(d => d.HoSo)
                    .WithMany(p => p.ThamKhams)
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.BacSi)
                    .WithMany()
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // SỬA LẠI: KetQuaLamSang - Liên kết trực tiếp với BenhNhan thay vì ThamKham
            modelBuilder.Entity<KetQuaLamSang>(entity =>
            {
                entity.Property(e => e.NhietDo)
                    .HasColumnType("decimal(5,2)");

                // Liên kết trực tiếp với BenhNhan
                entity.HasOne(d => d.BenhNhan)
                    .WithMany(p => p.KetQuaLamSang)
                    .HasForeignKey(d => d.MaBenhNhan)
                    .OnDelete(DeleteBehavior.Cascade);

                // Thêm index cho NgayKham để tối ưu truy vấn
                entity.HasIndex(e => e.NgayKham);
                entity.HasIndex(e => new { e.MaBenhNhan, e.NgayKham });
            });

            // ChiDinhXetNghiem
            modelBuilder.Entity<ChiDinhXetNghiem>(entity =>
            {
                entity.HasOne(d => d.HoSo)
                    .WithMany(p => p.ChiDinhs)
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.BacSi)
                    .WithMany()
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // KetQuaXetNghiem
            modelBuilder.Entity<KetQuaXetNghiem>(entity =>
            {
                entity.HasOne(d => d.ChiDinh)
                    .WithMany(p => p.KetQuas)
                    .HasForeignKey(d => d.MaChiDinh)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ChanDoan
            modelBuilder.Entity<ChanDoan>(entity =>
            {
                entity.HasOne(d => d.HoSo)
                    .WithMany()
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.Benh)
                    .WithMany(p => p.ChanDoans)
                    .HasForeignKey(d => d.MaBenh)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.BacSi)
                    .WithMany()
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ToaThuoc
            modelBuilder.Entity<ToaThuoc>(entity =>
            {
                entity.HasOne(d => d.HoSo)
                    .WithMany()
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.BacSi)
                    .WithMany()
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // ChiTietToaThuoc - Composite Key
            modelBuilder.Entity<ChiTietToaThuoc>(entity =>
            {
                entity.HasKey(e => new { e.MaToaThuoc, e.MaThuoc });
                entity.HasOne(d => d.ToaThuoc)
                    .WithMany(p => p.ChiTietToaThuocs)
                    .HasForeignKey(d => d.MaToaThuoc)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(d => d.Thuoc)
                    .WithMany(p => p.ChiTietToaThuocs)
                    .HasForeignKey(d => d.MaThuoc)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // DonDieuTri
            modelBuilder.Entity<DonDieuTri>(entity =>
            {
                entity.HasOne(d => d.HoSo)
                    .WithMany(p => p.DonDieuTris)
                    .HasForeignKey(d => d.MaHoSo)
                    .OnDelete(DeleteBehavior.NoAction);
                entity.HasOne(d => d.BacSi)
                    .WithMany()
                    .HasForeignKey(d => d.MaBacSi)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // THÊM MỚI: Seed data cho Identity Roles với giá trị tĩnh
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // THÊM MỚI: Seed Admin User với giá trị tĩnh
            var hasher = new PasswordHasher<IdentityUser>();
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "admin-user-id",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Admin123@"),
                    SecurityStamp = "STATIC-SECURITY-STAMP-12345",
                    ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-12345"
                }
            );

            // THÊM MỚI: Assign Admin Role to Admin User
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1",
                    UserId = "admin-user-id"
                }
            );

            // GIỮ NGUYÊN: Seed data cho HinhThucDieuTri
            modelBuilder.Entity<HinhThucDieuTri>().HasData(
                new HinhThucDieuTri { MaHinhThuc = 1, TenHinhThuc = "Nội trú" },
                new HinhThucDieuTri { MaHinhThuc = 2, TenHinhThuc = "Bán trú" },
                new HinhThucDieuTri { MaHinhThuc = 3, TenHinhThuc = "Ngoại trú" }
            );

            // GIỮ NGUYÊN: Seed data cho Khoa
            modelBuilder.Entity<Khoa>().HasData(
                new Khoa { MaKhoa = 1, TenKhoa = "Khoa Nội", MoTa = "Khoa điều trị các bệnh nội khoa" },
                new Khoa { MaKhoa = 2, TenKhoa = "Khoa Ngoại", MoTa = "Khoa phẫu thuật và điều trị ngoại khoa" },
                new Khoa { MaKhoa = 3, TenKhoa = "Khoa Sản", MoTa = "Khoa sản phụ khoa" },
                new Khoa { MaKhoa = 4, TenKhoa = "Khoa Nhi", MoTa = "Khoa điều trị trẻ em" },
                new Khoa { MaKhoa = 5, TenKhoa = "Khoa Cấp cứu", MoTa = "Khoa cấp cứu và hồi sức" }
            );
        }
    }
}

