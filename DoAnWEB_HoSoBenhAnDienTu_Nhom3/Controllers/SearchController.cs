
using Microsoft.AspNetCore.Mvc;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SearchController : Controller
    {
        private readonly List<SearchSuggestion> _dummyData = new List<SearchSuggestion>
        {
            new SearchSuggestion { DisplayText = "Hồ sơ bệnh án ", Url = "/HoSoBenhAn/Index/" },
            new SearchSuggestion { DisplayText = "Kết quả xét nghiệm ", Url = "/KetQuaXetNghiem/KetQuaCuaToi/" },
            new SearchSuggestion { DisplayText = "Kết quả lâm sàng ", Url = "/KetQuaLamSang/" },
            new SearchSuggestion { DisplayText = "Thông tin tài khoản ", Url = "/QuanLyNguoiDung/Index/" },
            new SearchSuggestion { DisplayText = "Bệnh nhân mới nhập viện ", Url = "/HoSoBenhAn/Index/" },
            //new SearchSuggestion { DisplayText = "Lịch sử khám bệnh ", Url = "/KetQuaLamSang/Details/" }
        };

        [HttpGet("GetSuggestions")] // Định nghĩa endpoint GET cụ thể để lấy gợi ý
        public IActionResult GetSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Ok(new List<SearchSuggestion>());
            }

            var lowerQuery = query.ToLower();

            // Trong ứng dụng thực tế, bạn sẽ truy vấn cơ sở dữ liệu của mình ở đây.
            // Ví dụ: _dbContext.MedicalRecords.Where(m => m.PatientName.Contains(query)).ToList();
            var suggestions = _dummyData
                .Where(s => s.DisplayText.ToLower().Contains(lowerQuery))
                .Take(5) // Giới hạn số lượng gợi ý
                .ToList();

            return Ok(suggestions);
        }
    }

    // Một mô hình đơn giản cho các gợi ý của bạn
    public class SearchSuggestion
    {
        public string DisplayText { get; set; } // Văn bản hiển thị cho gợi ý
        public string Url { get; set; } // URL mà gợi ý sẽ dẫn đến khi được chọn
    }
}