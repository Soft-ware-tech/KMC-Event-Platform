using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KMCWeb.Models;
using System.Text;
using System.Text.Json;

namespace KMCWeb.Pages
{
    public class ManagerLoginModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };

        public string? ErrorMessage { get; set; }

        [BindProperty]
        public string Username { get; set; } = "";
        [BindProperty]
        public string Password { get; set; } = "";

        public ManagerLoginModel(IHttpClientFactory _httpClientFactory)
        {
            httpClientFactory = _httpClientFactory;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            var client = httpClientFactory.CreateClient("KmcApi");
            var payload = new { Username, Password };
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Manager/login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var manager = JsonSerializer.Deserialize<ManagerReadModel>(json, JsonOpts);
                if (manager != null)
                {
                    HttpContext.Session.SetInt32("ManagerId", manager.ManagerId);
                    HttpContext.Session.SetString("ManagerName", manager.FullName);
                    return RedirectToPage("/Manager/Dashboard");
                }
            }

            ErrorMessage = "Invalid username or password.";
            return Page();
        }
    }
}