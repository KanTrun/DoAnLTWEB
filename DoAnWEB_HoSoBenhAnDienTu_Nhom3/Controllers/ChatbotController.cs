using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DoAnWEB_HoSoBenhAnDienTu_Nhom3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ChatbotController> _logger;

        public ChatbotController(IConfiguration config, IHttpClientFactory httpClientFactory, ILogger<ChatbotController> logger)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.message))
                return BadRequest(new { reply = "Tin nhắn không được để trống" });

            string apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("Gemini API key not found in configuration");
                return StatusCode(500, new { reply = "Chưa cấu hình Gemini API key" });
            }

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30);

            // Updated Gemini API endpoint
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

            // Enhanced system message for medical context
            var systemInstruction = @"
Bạn là Dr. AI - Trợ lý ảo chuyên nghiệp cho Hệ thống Hồ sơ Bệnh án Điện tử.

CHUYÊN MÔN:
- Giải thích kết quả xét nghiệm (máu, nước tiểu, X-quang, CT, MRI, siêu âm)
- Tư vấn về hồ sơ bệnh án điện tử và quản lý thông tin y tế
- Giải thích các chỉ số sức khỏe và ý nghĩa lâm sàng
- Hướng dẫn sử dụng hệ thống bệnh viện
- Giải thích thuật ngữ y khoa một cách dễ hiểu
- Tư vấn chăm sóc sức khỏe cơ bản và phòng ngừa bệnh tật
- Hướng dẫn chuẩn bị xét nghiệm và thủ tục y tế

NGUYÊN TẮC:
- Trả lời chuyên nghiệp, thân thiện và dễ hiểu
- Sử dụng thuật ngữ y khoa kèm giải thích đơn giản
- Luôn khuyến khích tham khảo ý kiến bác sĩ khi cần thiết
- Không chẩn đoán bệnh, chỉ cung cấp thông tin tham khảo
- Nếu câu hỏi không liên quan y tế, lịch sự từ chối và hướng dẫn về chủ đề y tế

Hãy trả lời ngắn gọn, súc tích nhưng đầy đủ thông tin cần thiết.";

            // Prepare payload with system instruction
            var payload = new
            {
                contents = new[]
                {
                    new {
                        role = "user",
                        parts = new[] {
                            new { text = $"{systemInstruction}\n\nBệnh nhân/Người dùng hỏi: {request.message}" }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.3, // Giảm để có câu trả lời ổn định hơn
                    maxOutputTokens = 600, // Tăng để có câu trả lời chi tiết hơn
                    topP = 0.8,
                    topK = 10
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                _logger.LogInformation("Sending medical query to Gemini API");
                var response = await httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Gemini API response status: {response.StatusCode}");
                _logger.LogDebug($"Gemini API response: {responseString}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Gemini API error: {response.StatusCode} - {responseString}");
                    string errorMessage = response.StatusCode switch
                    {
                        System.Net.HttpStatusCode.Unauthorized => "🔐 API key không hợp lệ, vui lòng liên hệ quản trị viên",
                        System.Net.HttpStatusCode.TooManyRequests => "⏰ Quá nhiều yêu cầu, vui lòng chờ một chút và thử lại",
                        System.Net.HttpStatusCode.PaymentRequired => "💳 Dịch vụ AI tạm thời không khả dụng, vui lòng liên hệ hỗ trợ",
                        _ => "🔧 Lỗi kết nối hệ thống AI, vui lòng thử lại sau"
                    };

                    return StatusCode(500, new { reply = errorMessage });
                }

                // Parse Gemini response
                var doc = JsonDocument.Parse(responseString);
                if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var firstCandidate = candidates[0];
                    if (firstCandidate.TryGetProperty("content", out var contentProp) &&
                        contentProp.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        var firstPart = parts[0];
                        if (firstPart.TryGetProperty("text", out var textProp))
                        {
                            var reply = textProp.GetString()?.Trim();

                            // Add medical disclaimer for health-related questions
                            if (!string.IsNullOrEmpty(reply) && IsHealthRelatedQuery(request.message))
                            {
                                if (!reply.Contains("bác sĩ") && !reply.Contains("chuyên gia"))
                                {
                                    reply += "\n\n⚠️ *Lưu ý: Thông tin này chỉ mang tính chất tham khảo. Vui lòng tham khảo ý kiến bác sĩ để có chẩn đoán và điều trị chính xác.*";
                                }
                            }

                            _logger.LogInformation("Successfully processed medical query");
                            return Ok(new { reply = reply });
                        }
                    }
                }

                _logger.LogWarning("Invalid response format from Gemini API");
                return Ok(new { reply = "🤖 Xin lỗi, tôi không thể tạo phản hồi lúc này. Vui lòng thử lại hoặc liên hệ hỗ trợ." });
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout to Gemini API");
                return StatusCode(408, new { reply = "⏱️ Yêu cầu bị timeout, vui lòng thử lại với câu hỏi ngắn gọn hơn" });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error calling Gemini API");
                return StatusCode(500, new { reply = "🌐 Lỗi mạng khi kết nối hệ thống AI, vui lòng kiểm tra kết nối" });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error");
                return StatusCode(500, new { reply = "📄 Lỗi xử lý dữ liệu từ hệ thống AI, vui lòng thử lại" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in medical chatbot");
                return StatusCode(500, new { reply = "🔧 Đã xảy ra lỗi không mong muốn, đội ngũ kỹ thuật đã được thông báo" });
            }
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            var apiKey = _config["Gemini:ApiKey"];
            return Ok(new
            {
                service = "Dr. AI Medical Assistant",
                hasApiKey = !string.IsNullOrEmpty(apiKey),
                keyPrefix = apiKey?.Length > 10 ? apiKey.Substring(0, 10) + "..." : "Not configured",
                timestamp = DateTime.UtcNow,
                status = "Ready to assist with medical queries"
            });
        }

        /// <summary>
        /// Kiểm tra xem câu hỏi có liên quan đến sức khỏe không
        /// </summary>
        private bool IsHealthRelatedQuery(string message)
        {
            var healthKeywords = new[] {
                "bệnh", "đau", "triệu chứng", "xét nghiệm", "thuốc", "điều trị",
                "chẩn đoán", "sức khỏe", "y tế", "bác sĩ", "bệnh viện", "khám",
                "huyết áp", "đường huyết", "cholesterol", "gan", "thận", "tim"
            };

            return healthKeywords.Any(keyword =>
                message.ToLower().Contains(keyword.ToLower()));
        }

        public class ChatRequest
        {
            public string message { get; set; }
        }
    }
}
