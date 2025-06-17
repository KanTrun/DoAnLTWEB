using System.ComponentModel.DataAnnotations;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class Khoa
    {
        [Key]
        public int MaKhoa { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhoa { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        // Navigation property - SỬA TÊN
        public virtual ICollection<BacSi> BacSis { get; set; } = new List<BacSi>();
    }
}
