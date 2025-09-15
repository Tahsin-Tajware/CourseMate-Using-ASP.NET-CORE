using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CourseMate.Controllers
{
    [Route("[controller]/[action]")]
    public class AiChatController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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";

        public AiChatController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        [Authorize] 
        public async Task<IActionResult> Ask([FromForm] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest("Message cannot be empty.");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            var response = await _httpClient.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions",
                new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
            );

            var result = await response.Content.ReadAsStringAsync();

            System.Console.WriteLine("🔍 API Response: " + result);

            var json = JsonConvert.DeserializeObject<ChatCompletionResponse>(result);

            if (json?.choices != null && json.choices.Count > 0)
            {
                string reply = json.choices[0].message.content;
                return Content(reply);
            }
            else if (result.Contains("error"))
            {
                return Content("⚠️ API Error: " + result);
            }
            else
            {
                return Content("⚠️ No reply received. Raw response: " + result);
            }
        }
    }

    public class ChatCompletionResponse
    {
        public List<Choice> choices { get; set; }
    }

    public class Choice
    {
        public Message message { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}