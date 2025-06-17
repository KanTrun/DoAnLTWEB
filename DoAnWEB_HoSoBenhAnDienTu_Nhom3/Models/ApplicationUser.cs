using Microsoft.AspNetCore.Identity;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
